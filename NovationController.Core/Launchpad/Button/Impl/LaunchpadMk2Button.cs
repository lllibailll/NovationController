using System.Drawing;
using Commons.Music.Midi;

namespace NovationController.Core.Launchpad.Button.Impl
{
    public class LaunchpadMk2Button : ILaunchpadButton
    {
        private Color _color;
        private readonly IMidiOutput _outPort;

        public byte Channel { get; set; }
        public Color Color
        {
            get => _color;
            set => _color = value;
        }
        public byte Id { get; set; }
        public bool IsPulsing { get; set; }
        public LaunchpadButtonState State { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public LaunchpadMk2Button(byte channel, byte id, Color color)
        {
            _color = color;
            Channel = channel;
            Id = id;
            IsPulsing = false;
            State = LaunchpadButtonState.Released;
        }

        public LaunchpadMk2Button(byte channel, int x, int y, Color color, IMidiOutput outPort)
        {
            _color = color;
            _outPort = outPort;
            Channel = channel;
            Id = (byte)((y + 1) * 10 + x + 1);
            IsPulsing = false;
            State = LaunchpadButtonState.Released;
            X = x;
            Y = y;
        }
    }
}