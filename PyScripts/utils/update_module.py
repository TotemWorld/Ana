import shutil
import argparse


# Because the path of libraries in [PythonEngine] is config to:
# "C:\Users\<username>\AppData\Local\Programs\Python\Python39\Lib
# With this script you can copy and paste the file provided to that path, so that it update without
# manually copying and pasting.

parser = argparse.ArgumentParser(description= 'Script to delete Temp files created by flutter')
parser.add_argument('-un', '--username')
args = parser.parse_args()
username = args.username

if username == None:
    raise Exception("User need to be provided with -un flag \n C:\\Users\\<username>\\AppData\\Local\\Temp")

src_file = './PyScripts/question_embedding.py'
dest_dir = f'C:/Users/{username}/AppData/Local/Programs/Python/Python39/Lib/'

shutil.copy2(src_file, dest_dir)


