import os
import platform
import shutil

def get_appdata_path():
    if platform.system() == 'Windows':
        return os.path.join(os.getenv('APPDATA'), "ARLeF/CoretorOrtograficFurlan/Dictionaries")
    elif platform.system() == 'Darwin':
        return os.path.expanduser('~/Library/Application Support/ARLeF/CoretorOrtograficFurlan/Dictionaries')
    elif platform.system() == 'Linux':
        return os.path.expanduser('~/.config/ARLeF/CoretorOrtograficFurlan/Dictionaries')
    else:
        raise Exception(f"Unsupported OS: {platform.system()}")

def delete_folder(folder_path):
    if os.path.exists(folder_path):
        try:
            shutil.rmtree(folder_path)
            print(f"Deleted folder: {folder_path}")
        except Exception as e:
            print(f"Failed to delete {folder_path}. Reason: {e}")
    else:
        print(f"Folder not found: {folder_path}")

if __name__ == "__main__":
    try:
        appdata_folder = get_appdata_path()
        delete_folder(appdata_folder)
    except Exception as e:
        print(f"An error occurred: {e}")