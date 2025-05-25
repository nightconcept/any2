import requests
import zipfile
import os
import shutil
import json
import tempfile

OWNER = "nightconcept"
REPO = "build-sdl3"
ASSET_NAME = "sdl3-bundle.zip"
PREBUILT_DIR = os.path.join(os.path.dirname(__file__), "..", "lib", "SDL3-Prebuilt")
VERSION_FILE = os.path.join(PREBUILT_DIR, "version.txt")

# Ensure PREBUILT_DIR subdirectories exist
os.makedirs(os.path.join(PREBUILT_DIR, "lib64"), exist_ok=True)
os.makedirs(os.path.join(PREBUILT_DIR, "macos"), exist_ok=True)
os.makedirs(os.path.join(PREBUILT_DIR, "win64"), exist_ok=True)

def get_latest_release_info():
    """Fetches the latest release information from GitHub."""
    api_url = f"https://api.github.com/repos/{OWNER}/{REPO}/releases/latest"
    print(f"Fetching latest release info from {api_url}...")
    response = requests.get(api_url)
    response.raise_for_status()  # Raise an exception for HTTP errors
    release_data = response.json()
    return release_data

def find_asset_url(release_data, asset_name):
    """Finds the download URL for a specific asset in the release data."""
    for asset in release_data.get("assets", []):
        if asset["name"] == asset_name:
            return asset["browser_download_url"], release_data["tag_name"]
    return None, None

def download_file(url, dest_path):
    """Downloads a file from a URL to a destination path."""
    print(f"Downloading {url} to {dest_path}...")
    response = requests.get(url, stream=True)
    response.raise_for_status()
    with open(dest_path, "wb") as f:
        for chunk in response.iter_content(chunk_size=8192):
            f.write(chunk)
    print("Download complete.")

def extract_zip(zip_path, extract_to_path):
    """Extracts a zip file to a specified directory."""
    print(f"Extracting {zip_path} to {extract_to_path}...")
    with zipfile.ZipFile(zip_path, "r") as zip_ref:
        zip_ref.extractall(extract_to_path)
    print("Extraction complete.")

def update_prebuilt_files(extract_path):
    """Copies relevant files from the extracted bundle to the prebuilt directory."""
    print(f"Updating files in {PREBUILT_DIR}...")

    # Define source and destination mappings
    # Based on user feedback and image, the zip extracts platform folders directly.
    # Source structure (from zip, within extract_path):
    #   linux-x64/libSDL3.so.0  (Note: may also contain other files, we only need .so.0)
    #   macos/libSDL3.0.dylib
    #   win64/SDL3.dll
    #   android/... (ignored)
    #   ios/... (ignored)
    #
    # Destination structure (in PREBUILT_DIR):
    #   lib64/libSDL3.so.0
    #   macos/libSDL3.0.dylib
    #   win64/SDL3.dll

    # The `extract_path` is the root where 'linux-x64', 'macos', 'win64' folders are.

    mappings = {
        os.path.join(extract_path, "linux-x64", "libSDL3.so.0"): os.path.join(PREBUILT_DIR, "lib64", "libSDL3.so.0"),
        os.path.join(extract_path, "macos", "libSDL3.0.dylib"): os.path.join(PREBUILT_DIR, "macos", "libSDL3.0.dylib"),
        os.path.join(extract_path, "win64", "SDL3.dll"): os.path.join(PREBUILT_DIR, "win64", "SDL3.dll"),
    }

    for src, dest in mappings.items():
        if os.path.exists(src):
            # Ensure destination directory exists
            os.makedirs(os.path.dirname(dest), exist_ok=True)
            shutil.copy2(src, dest)
            print(f"Copied {src} to {dest}")
        else:
            print(f"Warning: Source file {src} not found in extracted bundle.")

    print("File update complete.")

def update_version_file(version_tag):
    """Updates the version.txt file with the new version tag."""
    print(f"Updating {VERSION_FILE} to version {version_tag}...")
    with open(VERSION_FILE, "w") as f:
        f.write(version_tag.lstrip('v')) # Remove 'v' prefix if present
    print("Version file updated.")

def main():
    try:
        release_data = get_latest_release_info()
        asset_url, version_tag = find_asset_url(release_data, ASSET_NAME)

        if not asset_url:
            print(f"Error: Could not find asset '{ASSET_NAME}' in the latest release.")
            return

        print(f"Found asset: {asset_url} (Version: {version_tag})")

        with tempfile.TemporaryDirectory() as tmpdir:
            zip_path = os.path.join(tmpdir, ASSET_NAME)

            download_file(asset_url, zip_path)

            extract_target_path = os.path.join(tmpdir, "extracted_sdl3")
            os.makedirs(extract_target_path, exist_ok=True)
            extract_zip(zip_path, extract_target_path)

            update_prebuilt_files(extract_target_path)
            update_version_file(version_tag)

        print("SDL3 update process completed successfully.")

    except requests.exceptions.RequestException as e:
        print(f"Error during network request: {e}")
    except zipfile.BadZipFile:
        print("Error: Downloaded file is not a valid zip file or is corrupted.")
    except Exception as e:
        print(f"An unexpected error occurred: {e}")

if __name__ == "__main__":
    main()
