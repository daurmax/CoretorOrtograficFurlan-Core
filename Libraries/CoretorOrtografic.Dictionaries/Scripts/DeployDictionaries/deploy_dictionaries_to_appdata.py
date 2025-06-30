import os
import platform
import subprocess
import zipfile
import concurrent.futures

def get_appdata_path():
    system = platform.system()
    if system == 'Windows':
        return os.path.join(os.getenv('APPDATA'), "CoretorOrtograficFurlan/Dictionaries")
    elif system == 'Darwin':  # macOS
        return os.path.expanduser('~/Library/Application Support/CoretorOrtograficFurlan/Dictionaries')
    elif system == 'Linux':
        return os.path.expanduser('~/.config/CoretorOrtograficFurlan/Dictionaries')
    else:
        raise Exception(f"Unsupported OS: {system}")

def unzip_file(zip_path, destination_folder):
    if not os.path.exists(zip_path):
        print(f"File not found: {zip_path}. Skipping...")
        return

    # Ensure destination folder exists
    if not os.path.exists(destination_folder):
        os.makedirs(destination_folder)

    # Check if the archive contents are already deployed
    try:
        with zipfile.ZipFile(zip_path, 'r') as archive:
            expected_files = [os.path.join(destination_folder, name) for name in archive.namelist()]
    except zipfile.BadZipFile:
        expected_files = []

    if expected_files and all(os.path.exists(path) for path in expected_files):
        print(f"Files from {zip_path} already exist. Skipping extraction.")
        return

    if platform.system() == 'Windows':
        # Use 7-Zip for Windows
        seven_zip_path = r"C:\Program Files\7-Zip\7z.exe"
        if not os.path.isfile(seven_zip_path):
            print("7-Zip is not installed. Please install 7-Zip to continue.")
            return
        subprocess.run([seven_zip_path, "x", "-o" + destination_folder, zip_path])
    else:
        # Use p7zip for Linux/macOS
        try:
            subprocess.run(['7z', 'x', '-o' + destination_folder, zip_path], check=True)
            print(f"Extracted {zip_path} to {destination_folder}")
        except subprocess.CalledProcessError:
            print(f"Failed to extract {zip_path}. File might be corrupted or a split archive.")

if __name__ == "__main__":
    try:
        appdata_folder = get_appdata_path()
        print(f"Current working directory: {os.getcwd()}")

        base_path = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", "Dictionaries"))
        print(f"Base path for dictionaries: {base_path}")

        if not os.path.exists(base_path):
            print(f"Error: Base path for dictionaries does not exist: {base_path}")
            exit(1)

        # List of zip files
        zip_files = [
            os.path.join(base_path, "Elisions", "elisions.zip"),
            os.path.join(base_path, "Errors", "errors.zip"),
            os.path.join(base_path, "Frec", "frequencies.zip"),
            os.path.join(base_path, "WordsDatabase", "words_split.zip"),  # For split zip
            os.path.join(base_path, "WordsRadixTree", "words_split.zip")  # For split zip
        ]

        with concurrent.futures.ThreadPoolExecutor() as executor:
            futures = [executor.submit(unzip_file, zip_file, appdata_folder) for zip_file in zip_files]
            for future in concurrent.futures.as_completed(futures):
                future.result()

        print(f"All files extracted to {appdata_folder}")
    except Exception as e:
        print(f"An error occurred: {e}")
