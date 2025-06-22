# ArturiaMinilab3_grasshopper

This repository contains a simple Python module that demonstrates how to
listen to MIDI messages from the **Arturia Minilab 3** controller.  The
module can be used inside a GHPython component in Grasshopper or run as a
stand‑alone script.

## Requirements

- Python 3.7 or newer

No additional packages need to be installed manually.  When the listener
starts it will automatically install the `mido` and `python-rtmidi`
dependencies if they are missing.

## Usage

The `MinilabListener` class exposes a minimal API for connecting to a MIDI
input port, listening for **Control Change** and **Note On/Off** messages
and storing the latest values.  A small command‑line tool is included for
testing outside of Grasshopper.

Run `run_listener.bat` to start the command‑line listener on Windows.  The
batch file checks that Python is installed and then launches the script.

```bash
run_listener.bat
```

When running inside Grasshopper, you can import `MinilabListener` in a
GHPython component and start the listener as part of the solution update
to forward controller values to other components.

The code in this repository is intended to work with Rhino 7 and Rhino 8
on Windows.  If you compile it into a `.gha` plugin, bundle the
`mido` and `python-rtmidi` packages alongside the plugin so Grasshopper
has everything it needs.
## Building a .gha plugin

A minimal C# Grasshopper plugin is provided in the `MinilabPlugin` folder. It uses the
`Sanford.Multimedia.Midi` library to read MIDI messages. To compile the plugin:

1. Install the Rhino 7 or 8 Windows SDK so `RhinoCommon.dll` and `Grasshopper.dll` are available.
2. Restore the NuGet packages listed in `packages.config` (NAudio and Sanford.Multimedia.Midi).
3. Open `MinilabPlugin.csproj` in Visual Studio and build the project. The resulting
   `MinilabPlugin.gha` can be copied to your Grasshopper Libraries folder.

This lets you avoid copy-pasting Python code and provides the same functionality as
`minilab_listener.py`.

