import threading
import sys
import subprocess

MIN_PY = (3, 7)

if sys.version_info < MIN_PY:
    raise RuntimeError(f"Python {MIN_PY[0]}.{MIN_PY[1]} or newer is required")

def _ensure_deps():
    try:
        import mido  # noqa: F401
        import rtmidi  # noqa: F401
    except ImportError:
        print("Installing required packages ...")
        subprocess.check_call([sys.executable, "-m", "pip", "install", "mido", "python-rtmidi"])
    finally:
        globals().update(mido=__import__('mido'), rtmidi=__import__('rtmidi'))

_ensure_deps()

class MinilabListener:
    """Listen to MIDI messages from Arturia Minilab 3."""

    def __init__(self, port_name=None):
        self.port_name = port_name
        self.values = {}
        self.running = False
        self.thread = None
        self._port = None

    def list_ports(self):
        """Return available MIDI input port names."""
        try:
            return mido.get_input_names()
        except Exception as exc:
            print(f"Could not list MIDI ports: {exc}")
            return []

    def _callback(self, message):
        if message.type in {"control_change", "note_on", "note_off"}:
            key = getattr(message, 'control', getattr(message, 'note', None))
            self.values[key] = message.value if hasattr(message, 'value') else message.velocity

    def start(self):
        if self.running:
            return
        self.running = True
        if not self.port_name:
            ports = self.list_ports()
            if not ports:
                raise RuntimeError("No MIDI input ports available")
            self.port_name = ports[0]
        self._port = mido.open_input(self.port_name, callback=self._callback)
        self.thread = threading.Thread(target=self._run, daemon=True)
        self.thread.start()

    def _run(self):
        try:
            while self.running:
                mido.sleep(0.1)
        finally:
            if self._port is not None:
                self._port.close()

    def stop(self):
        self.running = False
        if self.thread:
            self.thread.join()

    def get_values(self):
        """Return dictionary of controller/note states."""
        return dict(self.values)

if __name__ == "__main__":
    listener = MinilabListener()
    print("Available MIDI ports:")
    for idx, name in enumerate(listener.list_ports()):
        print(f"  {idx}: {name}")
    if listener.list_ports():
        listener.start()
        print(f"Listening on {listener.port_name}. Press Ctrl+C to stop.")
        try:
            while True:
                mido.sleep(0.5)
                print(listener.get_values())
        except KeyboardInterrupt:
            listener.stop()
