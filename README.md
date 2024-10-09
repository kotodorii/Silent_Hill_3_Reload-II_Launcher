# Silent Hill 3 Reload-II Launcher

## Overview

This is a simple script that helps you run Silent Hill 3 with support for Reloaded-II mods. It simplifies the game startup process by using a configuration file (`launch_config.ini`) to specify paths to game files, making it possible to run on Steam as a non-steam game without any intermediate steps.

## Features

- Launch **Silent Hill 3** using the **Reloaded-II** mod loader.
- Configure game paths through a `.ini` file.
- Automatically monitors and waits for the game to start running.

## Requirements

- **Windows** operating system.
- **Reloaded-II** mod loader.
- Silent Hill 3 game files with configured mods through **Reloaded-II**.

## Installation

1. **Download** the latest version of the launcher from the [Releases](https://github.com/kotodorii/SilentHill3_Launcher/releases) page.
2. Extract the files to a folder of your choice.
3. Ensure you have [Reloaded-II](https://github.com/Reloaded-Project/Reloaded-II/releases/) installed and the required game files available.
4. Edit launcher\_config.ini file with the paths to necessary files.

## Configuration

1. Locate the `launch_config.ini` file in the extracted folder.
2. Edit the `.ini` file to specify the correct paths for your **Reloaded-II**, **Silent Hill 3**, and **working directory** locations:
   ```ini
   [Paths]
   ReloadedIIPath = C:\<FULL_PATH_TO>\Reloaded-II\Reloaded-II.exe
   SilentHill3Path = C:\<FULL_PATH_TO>\SILENT HILL 3\sh3.exe
   ReloadedIIWorkingDirectory = C:\<FULL_PATH_TO>\Reloaded-II
   ```
3. Save the file after updating the paths.

## Usage

1. **Run** the `SilentHill3_Launcher.exe`.
2. Ensure the paths in `launch_config.ini` are correct.
3. The launcher will start **Reloaded-II** and launch **Silent Hill 3** with the specified settings.

## Troubleshooting

- **Configuration File Missing**: If the `launch_config.ini` file is missing, the launcher will create a default version. Make sure to edit the paths accordingly.
- **Incorrect Paths**: Double-check that the paths in `launch_config.ini` point to the correct locations for **Reloaded-II** and **Silent Hill 3**.
- **Game Not Launching**: Ensure all paths are valid and that **Reloaded-II** and **Silent Hill 3** are installed correctly.
