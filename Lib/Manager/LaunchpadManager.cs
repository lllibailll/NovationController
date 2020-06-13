using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Core.Launchpad.Button;
using Core.Launchpad.Impl.Mk2;
using Lib.Click;
using Lib.Integration.Discord;
using Lib.Integration.Media;

namespace Lib.Manager
{
public class LaunchpadManager
    {
        private List<LaunchpadProfile> _profiles = new List<LaunchpadProfile>();

        private LaunchpadProfile _activeProfile;

        private LaunchpadMk2 _launchpad;

        private DiscordInt _discordInt;

        public LaunchpadManager()
        {
            _launchpad = LaunchpadMk2.GetInstance().Result;
            
            _launchpad.Clear();

            _launchpad.OnButtonStateChanged += button =>
            {
                if (_activeProfile == null) return;

                if (button.State == LaunchpadButtonState.Pressed)
                {
                    
                    if (button.X == 8)
                    {
                        // Profile changing buttons
                        var profileCandidate = _profiles.FirstOrDefault(x => x.LaunchpadCoord.Y == button.Y);

                        if (profileCandidate != null)
                        {
                            SetProfileActive(profileCandidate);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Button @ {button.X},{button.Y} of profile {_activeProfile.Name} clicked!");
                        var clickableButton = _activeProfile.Buttons.FirstOrDefault(x => x.X == button.X && x.Y == button.Y);
                        var clickCallback = clickableButton?.ClickCallback;
                        clickCallback?.Invoke();
                    }
                }
            };
        }

        public LaunchpadMk2 Launchpad => _launchpad;

        public void Shutdown()
        {
            _launchpad.Clear();
        }

        public void AddProfile(LaunchpadProfile launchpadProfile)
        {
            if (GetByCoords(launchpadProfile.LaunchpadCoord.X, launchpadProfile.LaunchpadCoord.Y) != null)
            {
                Console.WriteLine("A profile already is in that coords.");
                return;
            }
            
            _profiles.Add(launchpadProfile);

            if (_activeProfile == null)
            {
                SetProfileActive(launchpadProfile);
            }
        }

        public LaunchpadProfile GetByCoords(int coordX, int coordY)
        {
            return _profiles.FirstOrDefault(x => x.LaunchpadCoord.X == coordX && x.LaunchpadCoord.Y == coordY);
        }

        public void SetProfileActive(LaunchpadProfile launchpadProfile)
        {
            _launchpad.Clear();
            launchpadProfile.Buttons.ForEach(x =>
            {
                _launchpad.SetGridButtonColor(x.X, x.Y, x.Color);
            });
            var profileButton = launchpadProfile.LaunchpadCoord;
            _launchpad.SetGridButtonColor(profileButton.X, profileButton.Y, Color.Aqua);
            _activeProfile = launchpadProfile;
        }

        public void InitDiscord()
        {
            _discordInt = new DiscordInt();
            _discordInt.Init();
        }

        public DiscordInt DiscordInt => _discordInt;
    }
}