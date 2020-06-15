using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Core.Launchpad.Button;
using Core.Launchpad.Impl.Mk2;
using Lib.Profile;
using Newtonsoft.Json;

namespace Lib.Manager
{
    public class ProfileManager
    {
        private NovationController _novationController;
        
        private List<LaunchpadProfile> _profiles = new List<LaunchpadProfile>();
        
        public LaunchpadProfile ActiveProfile { get; private set; }

        public ProfileManager(NovationController novationController)
        {
            _novationController = novationController;
        }
        
        public void LoadProfiles()
        {
            Console.WriteLine("Loading profiles...");
            var json = File.ReadAllText("profiles.json");

            var profiles = JsonConvert.DeserializeObject<List<LaunchpadProfile>>(json);
            
            profiles.ForEach(p =>
            {
                p.Buttons.ForEach(x =>
                {
                    _novationController.LaunchpadManager.RegisterButtonActions(x);
                });

                AddProfile(p);
            });
        }
        
        private LaunchpadProfile GetByCoords(int coordX, int coordY)
        {
            return _profiles.FirstOrDefault(x => x.CoordX == coordX && x.CoordY == coordY);
        }
        
        private void AddProfile(LaunchpadProfile launchpadProfile)
        {
            if (GetByCoords(launchpadProfile.CoordX, launchpadProfile.CoordY) != null)
            {
                Console.WriteLine("A profile already is in that coords.");
                return;
            }
            
            Console.WriteLine($"Added profile {launchpadProfile.Name}");

            _profiles.Add(launchpadProfile);

            if (ActiveProfile == null)
            {
                SetProfileActive(launchpadProfile);
            }
        }
        
        private void SetProfileActive(LaunchpadProfile launchpadProfile)
        {
            Launchpad.Clear();
            launchpadProfile.Buttons.ForEach(x =>
            {
                Launchpad.SetGridButtonColor(x.X, x.Y, x.Color);

                x.LoadCallbacks.ForEach(y =>
                {
                    y.Invoke();
                });
            });
            ActiveProfile = launchpadProfile;
            SetProfileButtonLight(Color.Aqua);
        }

        public void SetProfileButtonLight(Color color)
        {
            Launchpad.SetGridButtonColor(ActiveProfile.CoordX, ActiveProfile.CoordY, color);
        }
        
        private LaunchpadMk2 Launchpad => _novationController.LaunchpadManager.Launchpad;

        public void HandleSelection(int buttonX, int buttonY)
        {
            // Profile changing buttons
            var profileCandidate = _profiles.FirstOrDefault(x => x.CoordY == buttonY);

            if (profileCandidate != null)
            {
                SetProfileActive(profileCandidate);
            }
        }

        public void HandleClick(ILaunchpadButton button)
        {
            Console.WriteLine($"Button @ {button.X},{button.Y} of profile {ActiveProfile.Name} clicked!");
            var clickableButton = ActiveProfile.Buttons.FirstOrDefault(x => x.X == button.X && x.Y == button.Y);
            clickableButton?.ClickCallbacks.ForEach(x =>
            {
                x.Invoke();
            });
        }

        public void Shutdown()
        {
            
        }
    }
}