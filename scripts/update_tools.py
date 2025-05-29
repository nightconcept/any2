import os
import requests
import zipfile
import shutil
import json # Added for manifest handling

# Configuration
TOOL_NAME = "crunch"
GITHUB_OWNER = "nightconcept"
GITHUB_REPO = "crunch"
GITHUB_API_URL_LATEST_RELEASE = f"https://api.github.com/repos/{GITHUB_OWNER}/{GITHUB_REPO}/releases/latest"

BASE_TARGET_DIR = os.path.join("tools", TOOL_NAME)
MANIFEST_FILE_PATH = os.path.join("tools", "manifest.json")

PLATFORM_IDENTIFIERS = {
    "linux": "linux",
    "macos": "macos",
    "windows": "windows"
}

def ensure_dir_exists(path):
    """Ensures that a directory exists, creating it if necessary."""
    if not os.path.exists(path):
        os.makedirs(path, exist_ok=True)

def load_manifest():
    """Loads the manifest file."""
    ensure_dir_exists(os.path.dirname(MANIFEST_FILE_PATH))
    if not os.path.exists(MANIFEST_FILE_PATH):
        return {}
    try:
        with open(MANIFEST_FILE_PATH, 'r') as f:
            return json.load(f)
    except (json.JSONDecodeError, FileNotFoundError):
        return {}

def save_manifest(data):
    """Saves data to the manifest file."""
    ensure_dir_exists(os.path.dirname(MANIFEST_FILE_PATH))
    with open(MANIFEST_FILE_PATH, 'w') as f:
        json.dump(data, f, indent=4)

def get_latest_release_info():
    """Fetches the latest release information from GitHub."""
    print(f"Fetching latest release information for {GITHUB_OWNER}/{GITHUB_REPO}...")
    try:
        response = requests.get(GITHUB_API_URL_LATEST_RELEASE, timeout=30)
        response.raise_for_status()
        release_data = response.json()
        version = release_data.get("tag_name")
        assets = release_data.get("assets", [])
        if not version or not assets:
            print("Error: Could not find version or assets in GitHub API response.")
            return None

        print(f"Latest version found: {version}")
        return {"version": version, "assets": assets}
    except requests.exceptions.RequestException as e:
        print(f"Error fetching release info from GitHub: {e}")
        return None
    except json.JSONDecodeError:
        print("Error: Could not decode JSON response from GitHub API.")
        return None


def download_and_extract_asset(asset_name, asset_url, platform_subdir, base_download_path):
    """Downloads a single asset and extracts it into a platform-specific subdirectory."""

    # Temporary download location for the zip file (e.g., in BASE_TARGET_DIR)
    temp_zip_dir = base_download_path
    # Use a generic name for the downloaded zip to avoid issues if asset_name is complex
    # However, for clarity during download, asset_name is fine if it's just the filename.
    zip_file_path = os.path.join(temp_zip_dir, asset_name)

    ensure_dir_exists(temp_zip_dir) # Should already exist if base_download_path is BASE_TARGET_DIR

    extract_to_path = os.path.join(base_download_path, platform_subdir)
    ensure_dir_exists(extract_to_path)

    print(f"Processing {asset_name} for {platform_subdir}...")
    try:
        print(f"  Downloading from {asset_url}...")
        response = requests.get(asset_url, stream=True, timeout=60)
        response.raise_for_status()

        with open(zip_file_path, 'wb') as f:
            for chunk in response.iter_content(chunk_size=8192):
                f.write(chunk)

        print(f"  Extracting to {extract_to_path}...")
        with zipfile.ZipFile(zip_file_path, 'r') as zip_ref:
            zip_ref.extractall(extract_to_path)
        print(f"  Successfully processed {asset_name}")

    except requests.exceptions.RequestException as e:
        print(f"  Error downloading {asset_name}: {e}")
        return False
    except zipfile.BadZipFile:
        print(f"  Error: {asset_name} is not a valid zip file or is corrupted.")
        return False
    except Exception as e:
        print(f"  An unexpected error occurred with {asset_name}: {e}")
        return False
    finally:
        if os.path.exists(zip_file_path):
            os.remove(zip_file_path)
    return True

def main():
    """Main function to download and extract crunch tools."""
    print(f"Starting update process for {TOOL_NAME} tools into {BASE_TARGET_DIR}...")
    ensure_dir_exists(BASE_TARGET_DIR)

    manifest_data = load_manifest()
    current_tool_info = manifest_data.get(TOOL_NAME, {})
    current_version = current_tool_info.get("version")

    latest_release_info = get_latest_release_info()
    if not latest_release_info:
        print(f"Could not retrieve latest release information for {TOOL_NAME}. Exiting.")
        return

    latest_version = latest_release_info["version"]

    if current_version == latest_version:
        print(f"{TOOL_NAME} is already up to date (Version: {current_version}).")
        return

    print(f"New version of {TOOL_NAME} available: {latest_version}. (Current: {current_version or 'None'})")
    print("Attempting to download and extract new version...")

    all_successful_this_run = True
    assets_processed_count = 0

    for asset in latest_release_info["assets"]:
        asset_name = asset.get("name", "").lower()
        asset_url = asset.get("browser_download_url")

        if not asset_name or not asset_url or not asset_name.endswith(".zip"):
            continue # Skip if not a zip or missing essential info

        processed_for_platform = False
        for platform_key, id_string in PLATFORM_IDENTIFIERS.items():
            if id_string in asset_name:
                if not download_and_extract_asset(asset.get("name"), asset_url, platform_key, BASE_TARGET_DIR):
                    all_successful_this_run = False
                    print(f"Failed to process asset {asset.get('name')} for platform {platform_key}.")
                else:
                    assets_processed_count += 1
                processed_for_platform = True
                break # Asset matched a platform

    if all_successful_this_run and assets_processed_count > 0:
        manifest_data[TOOL_NAME] = {"version": latest_version}
        save_manifest(manifest_data)
        print(f"{TOOL_NAME} successfully updated to version {latest_version}.")
        print(f"{assets_processed_count} asset(s) processed.")
    elif assets_processed_count == 0:
        print(f"No suitable assets found for {TOOL_NAME} in version {latest_version}.")
    else:
        print(f"Some assets for {TOOL_NAME} version {latest_version} failed to download or extract. Manifest not updated.")

if __name__ == "__main__":
    main()
