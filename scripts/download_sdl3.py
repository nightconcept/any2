import os
import requests
import zipfile
import tarfile
import shutil
import re
import logging
from io import BytesIO

# Configure logging
logging.basicConfig(level=logging.INFO, format='%(levelname)s: %(message)s')

# Configuration
GITHUB_API_URL = "https://api.github.com/repos/libsdl-org/SDL/releases"
PROJECT_ROOT = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
NIGHT_ENGINE_DIR = os.path.join(PROJECT_ROOT, "Night.Engine")
RUNTIMES_DIR = os.path.join(NIGHT_ENGINE_DIR, "runtimes")
SDL3_VERSION_FILE = os.path.join(RUNTIMES_DIR, "sdl3_version.txt")
TEMP_EXTRACT_DIR_BASE = os.path.join(PROJECT_ROOT, "temp_sdl_extract")

PLATFORM_CONFIGS = {
    "win-x64": {
        "asset_patterns": [
            r"SDL3-\d+\.\d+\.\d+.*-win32-x64\.zip", # Primary: Runtime, e.g., SDL3-3.30.3-win32-x64.zip
            r"SDL3-devel-\d+\.\d+\.\d+.*-mingw\.zip", # Secondary: MinGW devel (contains runtime DLL)
            r"SDL3-devel-\d+\.\d+\.\d+.*-msvc\.zip" # Tertiary: MSVC devel (contains runtime DLL)
        ],
        "file_to_extract": "SDL3.dll",
        "archive_paths": [
            "x64/SDL3.dll", # Common in win32-x64.zip and msvc.zip
            "SDL3.dll" # Fallback
        ],
        "target_filename": "SDL3.dll",
        "type": "zip"
    },
    "linux-x64": {
        "asset_patterns": [
            r"SDL3-\d+\.\d+\.\d+.*-linux-x64\.tar\.gz" # Hypothetical
        ],
        "file_to_extract": "libSDL3.so", # PRD target name
        "archive_paths": [ # Paths within archive to check
            "lib/x86_64-linux-gnu/libSDL3.so.0", # Common pattern for versioned .so files
            "lib/libSDL3.so.0",
            "libSDL3.so.0",
            "lib/x86_64-linux-gnu/libSDL3.so",
            "lib/libSDL3.so",
            "libSDL3.so"
        ],
        "target_filename": "libSDL3.so",
        "type": "tar.gz"
    },
    "osx-x64": {
        "asset_patterns": [
            r"SDL3-\d+\.\d+\.\d+.*-macos-x64\.tar\.gz", # Hypothetical tar.gz
            r"SDL3-\d+\.\d+\.\d+.*-macos\.zip",      # Hypothetical zip
            r"SDL3-\d+\.\d+\.\d+.*\.dmg"            # .dmg as last resort
        ],
        "file_to_extract": "libSDL3.dylib", # PRD target name
        "archive_paths": [ # Paths within archive to check (for tar.gz/zip)
            "lib/libSDL3.dylib",
            "libSDL3.dylib",
            "SDL3.framework/Versions/A/SDL3" # Common path in .framework
        ],
        "target_filename": "libSDL3.dylib",
        "type": "tar.gz_or_zip_or_dmg" # Special handling for type
    }
}

def get_latest_sdl3_release():
    """Fetches the latest stable SDL3 release information."""
    logging.info(f"Fetching latest SDL3 release information from {GITHUB_API_URL}...")
    try:
        headers = {"Accept": "application/vnd.github.v3+json"}
        # Fetch all releases and then find the latest non-prerelease, non-rc SDL3
        response = requests.get(GITHUB_API_URL, headers=headers)
        response.raise_for_status()
        releases = response.json()

        for release in releases:
            tag_name = release.get("tag_name", "")
            # SDL3 tags are like "release-3.X.Y"
            # SDL2 tags are like "release-2.X.Y"
            # Want to avoid RCs like release-3.1.0-rc1
            if not release.get("prerelease") and tag_name.startswith("release-3.") and "rc" not in tag_name:
                logging.info(f"Found latest stable SDL3 release: {tag_name}")
                return release
        logging.warning("Could not find a suitable latest stable SDL3 release.")
        return None
    except requests.exceptions.RequestException as e:
        logging.error(f"Error fetching releases: {e}")
        return None
    except ValueError:
        logging.error("Error: Could not decode JSON response from GitHub API.")
        return None

def find_asset_for_platform(release, platform_key):
    """Finds a suitable asset for the given platform in the release."""
    config = PLATFORM_CONFIGS[platform_key]
    release_tag = release['tag_name']
    logging.info(f"Searching for assets for {platform_key} in release {release_tag}...")

    for pattern_str in config["asset_patterns"]:
        pattern = re.compile(pattern_str)
        for asset in release.get("assets", []):
            if pattern.fullmatch(asset["name"]):
                logging.info(f"Found matching asset: {asset['name']} using pattern '{pattern_str}'")
                return asset
    logging.warning(f"No suitable asset found for {platform_key} in release {release_tag} using patterns: {config['asset_patterns']}")
    return None

def download_file(url, description):
    """Downloads a file from a URL."""
    logging.info(f"Downloading {description} from {url}...")
    try:
        response = requests.get(url, stream=True)
        response.raise_for_status()
        return response.content
    except requests.exceptions.RequestException as e:
        logging.error(f"Error downloading {description}: {e}")
        return None

def extract_and_place_file(platform_key, archive_content, asset_name, target_dir):
    """Extracts the required file from the archive and places it in the target directory."""
    config = PLATFORM_CONFIGS[platform_key]
    file_to_extract_basename = config["file_to_extract"] # e.g. SDL3.dll
    archive_search_paths = config["archive_paths"] # e.g. x64/SDL3.dll
    target_file_path = os.path.join(target_dir, config["target_filename"])
    
    temp_extract_path = TEMP_EXTRACT_DIR_BASE + "_" + platform_key
    if os.path.exists(temp_extract_path):
        shutil.rmtree(temp_extract_path)
    os.makedirs(temp_extract_path, exist_ok=True)

    extracted_member_path = None
    archive_type = config["type"]

    try:
        if asset_name.endswith(".zip"):
            archive_type_to_use = "zip"
        elif asset_name.endswith(".tar.gz"):
            archive_type_to_use = "tar.gz"
        elif asset_name.endswith(".dmg"):
             archive_type_to_use = "dmg"
        else:
            logging.warning(f"Unknown archive type for asset {asset_name}, trying based on platform config: {archive_type}")
            archive_type_to_use = archive_type


        if archive_type_to_use == "zip":
            with zipfile.ZipFile(BytesIO(archive_content)) as zf:
                for member_path_pattern in archive_search_paths:
                    # Support simple wildcards like **/file.ext
                    actual_member_path_pattern = member_path_pattern.replace("**/", "") 
                    for member in zf.namelist():
                        if member.endswith(actual_member_path_pattern) or os.path.basename(member) == file_to_extract_basename:
                            zf.extract(member, temp_extract_path)
                            extracted_member_path = os.path.join(temp_extract_path, member)
                            logging.info(f"Extracted '{member}' to '{extracted_member_path}'")
                            break
                    if extracted_member_path: break
        
        elif archive_type_to_use == "tar.gz":
            with tarfile.open(fileobj=BytesIO(archive_content), mode="r:gz") as tf:
                for member_path_pattern in archive_search_paths:
                    actual_member_path_pattern = member_path_pattern.replace("**/", "")
                    for member in tf.getmembers():
                        if member.name.endswith(actual_member_path_pattern) or os.path.basename(member.name) == file_to_extract_basename:
                            tf.extract(member, temp_extract_path)
                            extracted_member_path = os.path.join(temp_extract_path, member.name)
                            logging.info(f"Extracted '{member.name}' to '{extracted_member_path}'")
                            break
                    if extracted_member_path: break
        
        elif archive_type_to_use == "dmg":
            logging.warning(f"Asset {asset_name} is a .dmg file. Automatic extraction is not supported by this script.")
            logging.warning(f"Please manually extract '{config['file_to_extract']}' from the downloaded {asset_name} "
                            f"and place it as '{target_file_path}'.")
            # Optionally save the DMG for manual extraction
            dmg_save_path = os.path.join(RUNTIMES_DIR, platform_key, asset_name)
            os.makedirs(os.path.dirname(dmg_save_path), exist_ok=True)
            with open(dmg_save_path, 'wb') as f:
                f.write(archive_content)
            logging.info(f"DMG file saved to {dmg_save_path}")
            return False # Indicate extraction was not successful for DMG

        if not extracted_member_path or not os.path.exists(extracted_member_path):
            logging.error(f"Could not find '{file_to_extract_basename}' (or matching pattern from {archive_search_paths}) in {asset_name}.")
            # Log archive contents for debugging
            if archive_type_to_use == "zip":
                with zipfile.ZipFile(BytesIO(archive_content)) as zf:
                    logging.debug(f"Files in {asset_name}: {zf.namelist()}")
            elif archive_type_to_use == "tar.gz":
                 with tarfile.open(fileobj=BytesIO(archive_content), mode="r:gz") as tf:
                    logging.debug(f"Files in {asset_name}: {[m.name for m in tf.getmembers()]}")
            return False

        os.makedirs(os.path.dirname(target_file_path), exist_ok=True)
        shutil.move(extracted_member_path, target_file_path)
        logging.info(f"Successfully placed '{config['target_filename']}' in '{os.path.dirname(target_file_path)}'")
        return True

    except (zipfile.BadZipFile, tarfile.TarError) as e:
        logging.error(f"Error: Archive {asset_name} is invalid or corrupted: {e}")
        return False
    except Exception as e:
        logging.error(f"An error occurred during extraction or moving for {asset_name}: {e}")
        return False
    finally:
        if os.path.exists(temp_extract_path):
            shutil.rmtree(temp_extract_path)

def main():
    logging.info("Starting SDL3 download and setup process...")
    os.makedirs(RUNTIMES_DIR, exist_ok=True)

    release = get_latest_sdl3_release()
    if not release:
        logging.error("Could not determine the latest SDL3 release. Aborting.")
        return

    sdl3_version = release["tag_name"]
    logging.info(f"Targeting SDL3 version: {sdl3_version}")

    all_successful = True
    for platform_key in PLATFORM_CONFIGS.keys():
        logging.info(f"--- Processing platform: {platform_key} ---")
        platform_target_dir = os.path.join(RUNTIMES_DIR, platform_key)
        os.makedirs(platform_target_dir, exist_ok=True)

        asset = find_asset_for_platform(release, platform_key)
        if not asset:
            logging.warning(f"Could not find a suitable asset for {platform_key}. Skipping.")
            all_successful = False # Or handle this as per requirements - is it a failure if one platform is missing?
            continue

        asset_url = asset["browser_download_url"]
        asset_name = asset["name"]

        archive_content = download_file(asset_url, asset_name)
        if not archive_content:
            logging.error(f"Failed to download {asset_name} for {platform_key}. Skipping.")
            all_successful = False
            continue
        
        if not extract_and_place_file(platform_key, archive_content, asset_name, platform_target_dir):
            logging.error(f"Failed to extract and place library for {platform_key} from {asset_name}.")
            all_successful = False
            # If DMG, it's not a "failure" of the script's attempt, but a known limitation.
            if PLATFORM_CONFIGS[platform_key]["type"] == "tar.gz_or_zip_or_dmg" and asset_name.endswith(".dmg"):
                 pass # Already logged instructions
            else:
                all_successful = False


    if all_successful or True: # For now, write version even if some platforms fail, as per "create/update"
        try:
            with open(SDL3_VERSION_FILE, "w") as f:
                f.write(sdl3_version)
            logging.info(f"Successfully wrote SDL3 version '{sdl3_version}' to {SDL3_VERSION_FILE}")
        except IOError as e:
            logging.error(f"Failed to write SDL3 version file: {e}")
            all_successful = False # This is a critical failure

    if all_successful:
        logging.info("SDL3 setup process completed successfully for all targeted platforms.")
    else:
        logging.warning("SDL3 setup process completed with some issues. Please check logs.")

if __name__ == "__main__":
    main()