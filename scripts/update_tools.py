import os
import requests
import zipfile
import shutil

# Configuration
BASE_URL = "https://github.com/nightconcept/crunch/releases/download/2025.05.29/"
FILES_TO_DOWNLOAD = [
    {"name": "crunch-linux-2025.05.29.zip", "url_suffix": "crunch-linux-2025.05.29.zip", "platform_subdir": "linux"},
    {"name": "crunch-macos-2025.05.29.zip", "url_suffix": "crunch-macos-2025.05.29.zip", "platform_subdir": "macos"},
    {"name": "crunch-windows-2025.05.29.zip", "url_suffix": "crunch-windows-2025.05.29.zip", "platform_subdir": "windows"},
]
BASE_TARGET_DIR = os.path.join("tools", "crunch")

def ensure_dir_exists(path):
    """Ensures that a directory exists, creating it if necessary."""
    if not os.path.exists(path):
        print(f"Creating directory: {path}")
        os.makedirs(path, exist_ok=True)
    # else:
        # print(f"Directory already exists: {path}") # Reduced verbosity

def download_and_extract_file(file_info, base_download_path):
    """Downloads a single file and extracts it into a platform-specific subdirectory."""
    file_name = file_info["name"]
    platform_subdir = file_info["platform_subdir"]
    url = BASE_URL + file_info["url_suffix"]

    # Temporary download location for the zip file (e.g., in BASE_TARGET_DIR)
    temp_zip_dir = base_download_path
    zip_file_path = os.path.join(temp_zip_dir, file_name)

    # Ensure the temporary directory for zip download exists
    ensure_dir_exists(temp_zip_dir)

    # Define the final extraction path
    extract_to_path = os.path.join(base_download_path, platform_subdir)
    ensure_dir_exists(extract_to_path)

    print(f"Processing {file_name} for {platform_subdir}...")
    try:
        print(f"  Downloading from {url}...")
        response = requests.get(url, stream=True, timeout=60)
        response.raise_for_status()  # Raise an exception for HTTP errors

        with open(zip_file_path, 'wb') as f:
            for chunk in response.iter_content(chunk_size=8192):
                f.write(chunk)
        # print(f"Successfully downloaded {file_name}") # Reduced verbosity

        print(f"  Extracting to {extract_to_path}...")
        with zipfile.ZipFile(zip_file_path, 'r') as zip_ref:
            zip_ref.extractall(extract_to_path)
        # print(f"Successfully extracted {file_name}") # Reduced verbosity

    except requests.exceptions.RequestException as e:
        print(f"  Error downloading {file_name}: {e}")
        return False
    except zipfile.BadZipFile:
        print(f"Error: {file_name} is not a valid zip file or is corrupted.")
        return False
    except Exception as e:
        print(f"An unexpected error occurred with {file_name}: {e}")
        return False
    finally:
        # Clean up the downloaded zip file
        if os.path.exists(zip_file_path):
            # print(f"Removing temporary file: {zip_file_path}") # Reduced verbosity
            os.remove(zip_file_path)
    return True

def main():
    """Main function to download and extract crunch tools."""
    print(f"Starting update process for crunch tools into {BASE_TARGET_DIR}...")

    ensure_dir_exists(BASE_TARGET_DIR) # Ensure the main tools/crunch directory exists

    all_successful = True
    for file_info in FILES_TO_DOWNLOAD:
        if not download_and_extract_file(file_info, BASE_TARGET_DIR):
            all_successful = False
            print(f"Failed to process {file_info['name']}.")
            # Depending on desired behavior, you might want to 'break' here
            # or continue trying other files.

    if all_successful:
        print("All crunch tools processed successfully.")
    else:
        print("Some files failed to download or extract. Please check the logs.")

if __name__ == "__main__":
    main()
