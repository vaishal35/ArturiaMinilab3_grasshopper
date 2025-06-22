using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Midi;

namespace MinilabPlugin
{
    public class MinilabListenerComponent : GH_Component
    {
        private InputDevice _device;
        private Dictionary<int, int> _values = new Dictionary<int, int>();
        private bool _started;

        public MinilabListenerComponent() : base("MinilabListener", "Minilab", "Listens to Arturia Minilab 3", "Params", "Input")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Values", "V", "Controller values", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            if (!_started)
            {
                if (InputDevice.InstalledDevices.Count == 0)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "No MIDI input devices found.");
                    return;
                }
                _device = InputDevice.InstalledDevices[0];
                _device.ControlChange += DeviceOnControlChange;
                _device.NoteOn += DeviceOnNoteOn;
                _device.NoteOff += DeviceOnNoteOff;
                _device.StartReceiving(null);
                _started = true;
            }

            DA.SetData(0, new GH_ObjectWrapper(new Dictionary<int, int>(_values)));
        }

        private void DeviceOnControlChange(ControlChangeMessage msg)
        {
            _values[msg.Control] = msg.Value;
        }

        private void DeviceOnNoteOn(NoteOnMessage msg)
        {
            _values[(int)msg.Pitch] = msg.Velocity;
        }

        private void DeviceOnNoteOff(NoteOffMessage msg)
        {
            _values.Remove((int)msg.Pitch);
        }

        public override Guid ComponentGuid => new Guid("31A04FFA-1DF2-4013-A161-A69D8C9D9BFD");
    }
}
