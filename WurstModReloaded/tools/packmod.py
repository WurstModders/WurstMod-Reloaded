﻿import zipfile
import os
from shutil import copyfile
import subprocess

# Define where and what is included in the mod
BIN_FOLDER = "WurstModReloaded/bin/Debug/net35/"
CONTENTS = {
    BIN_FOLDER + "/manifest.json": "manifest.json",
    BIN_FOLDER + "/WurstModReloaded.dll": "WurstModReloaded.dll",
    BIN_FOLDER + "/WurstModReloaded.dll.mdb": "WurstModReloaded.dll.mdb"
}
OUTPUT_FILENAME = "WurstModReloaded.deli"

# Generate the mdb
subprocess.call(("WurstModReloaded/tools/pdb2mdb.exe", BIN_FOLDER + "/WurstModReloaded.dll"))

# Create the zip file
zf = zipfile.ZipFile(OUTPUT_FILENAME, "w", zipfile.ZIP_DEFLATED)
for file, dest in CONTENTS.items():
    zf.write(file, dest)
zf.close()

# If the MODS_DIR envar is set, copy the file there
try:
    copyfile(OUTPUT_FILENAME, os.environ['MODS_DIR'] + OUTPUT_FILENAME)
except:
    print("Could not copy file.")
