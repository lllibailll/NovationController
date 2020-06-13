using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Commons.Music.Midi;
using Core.Launchpad.Button;
using Core.Launchpad.Button.Impl;

namespace Core.Launchpad.Impl
{
    public delegate void OnButtonStateChangedEvent(ILaunchpadButton button);
    public delegate void OnDisconnectedEvent();
    
    public abstract class Launchpad: ILaunchpad
    {
        public event OnButtonStateChangedEvent OnButtonStateChanged;
        public event OnDisconnectedEvent OnDisconnected;

        protected IMidiInput _input;
        protected IMidiOutput _output;
        
        public LaunchpadMk2Button[,] Grid { get; protected set; }
        public Color[,] GridBuffer { get; private set; }

        public Launchpad((IMidiInput, IMidiOutput) ports)
        {
            _input = ports.Item1;
            _output = ports.Item2;
            
            GridBuffer = new Color[9, 8];
        }

        protected static byte GetButtonId(int x, int y)
        {
            return (byte)((y+1) * 10 + (x+1));
        }

        protected static async Task<(IMidiInput, IMidiOutput)> GetMidiPorts(int index = 0)
        {
            // Get all the connected launchpads
            var access = MidiAccessManager.Default;
            var launchpadInputs = access.Inputs.Where(o => o.Name.ToLower().Contains("launchpad")).ToArray();
            var launchpadOutputs = access.Outputs.Where(o => o.Name.ToLower().Contains("launchpad")).ToArray();

            // Attempt to get the launchpad input at the index requested
            var launchpadInput = launchpadInputs.Length > index
                ? await access.OpenInputAsync(launchpadInputs[index].Id)
                : null;

            // Attempt to get teh launchpad output at the index requested
            var launchpadOutput = launchpadInputs.Length > index
                ? await access.OpenOutputAsync(launchpadOutputs[index].Id)
                : null;

            return (launchpadInput , launchpadOutput);
        }

        public void ButtonStateChanged(ILaunchpadButton button)
        {
            OnButtonStateChanged?.Invoke(button);
        }

        public abstract void Clear();

        /// <summary>
        /// Should be called when the system cannot detect the launchpad.
        /// </summary>
        public void Disconnected()
        {
            OnDisconnected?.Invoke();
        }
    }
}
