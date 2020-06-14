using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Core.Launchpad.Button;
using Core.Launchpad.Impl.Mk2;
using Lib.Click;
using Lib.Integration;
using Lib.Integration.Application;
using Lib.Integration.Discord;
using Lib.Integration.MagicHome;
using Lib.Integration.Media;
using Lib.Integration.PhilipsHue;
using Lib.Integration.Steam;
using Newtonsoft.Json;

namespace Lib.Manager
{
    public class LaunchpadManager
    {
        public const string ProfileSplitter = ";|;";
        
        private List<LaunchpadProfile> _profiles = new List<LaunchpadProfile>();

        private LaunchpadProfile _activeProfile;

        private DiscordInt _discordInt;

        private List<BaseIntegration> _integrations = new List<BaseIntegration>();
        
        private MediaIntegration _mediaIntegration;
        private ApplicationIntegration _applicationIntegration;
        private WebIntegration _webIntegration;
        private SteamIntegration _steamIntegration;
        private PhilipsHueIntegration _philipsHueIntegration;
        private MagicHomeIntegration _magicHomeIntegration;

        public LaunchpadMk2 Launchpad { get; }
        
        public LaunchpadManager()
        {
            _mediaIntegration = new MediaIntegration(this, "Media", "Media");
            _applicationIntegration = new ApplicationIntegration(this, "Application", "Proc");
            _webIntegration = new WebIntegration(this, "Web", "Web");
            _steamIntegration = new SteamIntegration(this, "Steam", "Steam");
            
            _philipsHueIntegration = new PhilipsHueIntegration(this, "PhilipsHue", "PhilipsHue");
            _magicHomeIntegration = new MagicHomeIntegration(this, "MagicHome", "MagicHome");

            Launchpad = LaunchpadMk2.GetInstance().Result;

            Launchpad.Clear();

            Launchpad.OnButtonStateChanged += button =>
            {
                if (_activeProfile == null) return;

                if (button.State == LaunchpadButtonState.Pressed)
                {
                    if (button.X == 8)
                    {
                        // Profile changing buttons
                        var profileCandidate = _profiles.FirstOrDefault(x => x.CoordY == button.Y);

                        if (profileCandidate != null)
                        {
                            SetProfileActive(profileCandidate);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Button @ {button.X},{button.Y} of profile {_activeProfile.Name} clicked!");
                        var clickableButton = _activeProfile.Buttons.FirstOrDefault(x => x.X == button.X && x.Y == button.Y);
                        clickableButton?.ClickCallbacks.ForEach(x => x.Invoke());
                    }
                }
            };

            LoadProfiles();
        }

        private void LoadProfiles()
        {
            var json = File.ReadAllText("profiles.json");

            var profiles = JsonConvert.DeserializeObject<List<LaunchpadProfile>>(json);

            profiles.ForEach(p =>
            {
                p.Buttons.ForEach(RegisterButtonActions);

                AddProfile(p);
            });
        }

        public void Shutdown()
        {
            Launchpad.Clear();
        }

        public void SetButtonColor(ClickableButton clickableButton, Color color)
        {
            Launchpad.SetGridButtonColor(clickableButton.X, clickableButton.Y, color);
        }

        private void AddProfile(LaunchpadProfile launchpadProfile)
        {
            if (GetByCoords(launchpadProfile.CoordX, launchpadProfile.CoordY) != null)
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

        private void RegisterButtonActions(ClickableButton clickableButton)
        {
            if (clickableButton.LoadRaws.Count > 0)
            {
                _integrations.ForEach(x =>
                {
                    x.CheckLoadAction(clickableButton);
                });
            }
            
            if (clickableButton.ClickRaws.Count > 0)
            {
                _integrations.ForEach(x =>
                {
                    x.CheckClickAction(clickableButton);
                });
            }
        }

        private LaunchpadProfile GetByCoords(int coordX, int coordY)
        {
            return _profiles.FirstOrDefault(x => x.CoordX == coordX && x.CoordY == coordY);
        }

        private void SetProfileActive(LaunchpadProfile launchpadProfile)
        {
            Launchpad.Clear();
            launchpadProfile.Buttons.ForEach(x =>
            {
                Launchpad.SetGridButtonColor(x.X, x.Y, x.Color);

                x.LoadCallbacks.ForEach(y => y.Invoke());
            });
            Launchpad.SetGridButtonColor(launchpadProfile.CoordX, launchpadProfile.CoordY, Color.Aqua);
            _activeProfile = launchpadProfile;
        }

        public void RegisterIntegration(BaseIntegration integration)
        {
            _integrations.Add(integration);
        }
    }
}