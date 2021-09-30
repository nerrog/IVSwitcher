# IVSwitcher

[[English](README.md)|[日本語](README-JP.md)]

Software to change GTAV's mod environment (ASI loader) and online environment (vanilla) as needed.

This README was translated and made by deepl
You may find it strange.

# Requirements

* Windows10 (11?)
* .NET Framework 4.7.1
* Grand Theft Auto V (Confirmed to work with Epic version)

# usage

Copy the following folders and files in the `Grand Theft Auto V` folder, create a `mods` folder, and copy them into it.

* `Update` folder
* `x64` folder
* `common.rpf` file
* `x64a.rpf` ~ `x64w.rpf` files all

2. install ASILoader from `Open IV`.

3. run `IVSwitcher.exe`.


* For the URL to be entered in the settings, create a shortcut from Steam or Epic Games Launcher, then copy and paste the URL of the shortcut.
* The ASILoader `dinput8.dll` is disabled in the vanilla environment without adding it by default, but other files you want to disable should be added in the settings.

# For advanced users

You can change the settings later by editing the generated `settings.json`.

* GTAV_PATH Specify the GTAV directory (string)
* dlls List of files to be disabled (list string)
* exec_url URL to be actually executed (string)
* use_epic Configuration when using EPIC Launcher (bool)
