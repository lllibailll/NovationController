using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using Commons.Music.Midi;
using Core.Launchpad.Button;
using Core.Launchpad.Button.Impl;

namespace Core.Launchpad.Impl.Mk2
{
    public class LaunchpadMk2 : Launchpad
    {
       public LaunchpadMk2((IMidiInput, IMidiOutput) ports) : base(ports)
        {
            CreateGridButtons();

            // Listen for messages from the launchpad
            if(_input != null)
                _input.MessageReceived += OnMessageReceived;
        }

        public static async Task<LaunchpadMk2> GetInstance(int index = 0)
        {
            return new LaunchpadMk2(await GetMidiPorts(index));
        }
        
        void CreateGridButtons()
        {
            Grid = new LaunchpadMk2Button[9, 8];
            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 9; x++)
                {
                    Grid[x, y] = new LaunchpadMk2Button(0, x, y, Color.Black, _output);
                }
            }
                
        }

        public override void Clear()
        {
            var command = new byte[] { 240, 0, 32, 41, 2, 24, 14, 0, 247 };
            _output.Send(command, 0, command.Length, 0);
        }

        private void OnMessageReceived(object sender, MidiReceivedEventArgs e)
        {
            try
            {
                switch (e.Data[0])
                {
                    // Grid Button Pressed
                    case 144:
                        var button = Grid[e.Data[1] % 10 - 1, e.Data[1] / 10 - 1];
                        button.State =
                            e.Data[2] == 0
                                ? LaunchpadButtonState.Released
                                : LaunchpadButtonState.Pressed;
                        ButtonStateChanged(button);
                        break;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }

        #region PulseButton
        
        public void PulseButton(byte button, byte color)
        {
            var command = new byte[] { 240, 0, 32, 41, 2, 24, 40, 0, button, color, 247 };
            _output.Send(command, 0, command.Length, 0);
        }

        public void PulseButton(int x, int y, byte color)
        {
            PulseButton(GetButtonId(x, y), color);
        }

        public void PulseButton(int x, int y, int color)
        {
            PulseButton(GetButtonId(x, y), (byte)color);
        }

        public void PulseButton(int x, int y, LaunchpadMk2Color color)
        {
            PulseButton(GetButtonId(x, y), (byte)color);
        }
        #endregion
        
        public void SetGridButtonColor(int id, Color color)
        {
            try
            {
                var button = Grid[id % 10 - 1, id / 10 - 1];
                button.Color = color;
                
                var command = new byte[]
                {
                    240, 0, 32, 41, 2, 24, 11,
                    (byte) id,
                    (byte) (color.R / 4),
                    (byte) (color.G / 4),
                    (byte) (color.B / 4),
                    247
                };
                _output?.Send(command, 0, command.Length, 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to set the grid button color. {ex}");
            }
        }
        
        public void SetGridButtonColor(int x, int y, Color color)
        {
            // Convert x-y to id
            SetGridButtonColor((y + 1) * 10 + x + 1, color);
        }

        public void SetGridColor(Color[,] colors)
        {
            try
            {
                for (var y = 0; y < 8; y++)
                    for (var x = 0; x < 8; x++)
                    {
                        var color = colors[x, y];
                        var id = (y + 1) * 10 + x + 1;
                        // Logisticaly update the button
                        var button = Grid[id % 10 - 1, id / 10 - 1];
                        button.Color = color;
                        var command = new byte[] { 240, 0, 32, 41, 2, 24, 11, (byte)id, (byte)(color.R / 4), (byte)(color.G / 4), (byte)(color.B / 4), 247 };
                        _output?.Send(command, 0, command.Length, 0);
                    }
                
            }
            catch (ObjectDisposedException exception)
            {
                Disconnected();
                Debug.WriteLine(exception);
            }
            catch (Exception exception)
            {
                if (exception.HResult == -2147023279)
                {
                    Disconnected();
                }
                Debug.WriteLine(exception);
            }
        }
        
        public void FlushGridBuffer(bool clearBufferAfter = true)
        {
            try
            {
                var commandBytes = new List<byte>();
                for (var y = 0; y < 8; y++)
                {
                    for (var x = 0; x < 8; x++)
                    {
                        var buttonId = (y + 1) * 10 + (x + 1);
                        Grid[x, y].Color = GridBuffer[x, y];
                        commandBytes.AddRange(new byte[] { 
                            240, 0, 32, 41, 2, 24, 11, 
                            (byte)buttonId, (byte)(GridBuffer[x, y].R / 4), (byte)(GridBuffer[x, y].G / 4),
                            (byte)(GridBuffer[x, y].B / 4), 247 }
                        );

                        if (clearBufferAfter)
                        {
                            GridBuffer[x, y] = Color.Black;
                        }
                    }
                }
                _output.Send(commandBytes.ToArray(), 0, commandBytes.ToArray().Length, 0);

            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}