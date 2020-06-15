using System;
using System.Drawing;
using Core.Launchpad.Button;
using Core.Launchpad.Impl.Mk2;
using Lib.Click;

namespace Lib.Manager
{
    public class LaunchpadManager
    {
        public const string ProfileSplitter = ";|;";

        private bool _lights = true;

        public LaunchpadMk2 Launchpad { get; }

        private NovationController _novationController;
        
        public LaunchpadManager(NovationController novationController)
        {
            _novationController = novationController;
            Launchpad = LaunchpadMk2.GetInstance().Result;
            Launchpad.Clear();
        }

        public void RegisterButtonListener()
        {
            Launchpad.OnButtonStateChanged += button =>
            {
                if (_novationController.ProfileManager.ActiveProfile == null) return;

                if (button.State == LaunchpadButtonState.Pressed)
                {
                    if (button.X == 8)
                    {
                        _novationController.ProfileManager.HandleSelection(button.X, button.Y);
                    }
                    else
                    {
                        _novationController.ProfileManager.HandleClick(button);
                    }
                }
            };
        }

        public void Shutdown()
        {
            Launchpad.Clear();
        }

        public void ToggleLights()
        {
            if (_lights)
            {
                _novationController.ProfileManager.ActiveProfile.Buttons.ForEach(x => SetButtonColor(x, Color.Empty));
                _novationController.ProfileManager.SetProfileButtonLight(Color.Empty);
            }
            else
            {
                _novationController.ProfileManager.ActiveProfile.Buttons.ForEach(x => SetButtonColor(x, x.Color));
                _novationController.ProfileManager.SetProfileButtonLight(Color.Aqua);
            }

            _lights = !_lights;
        }

        public void SetButtonColor(ClickableButton clickableButton, Color color)
        {
            Launchpad.SetGridButtonColor(clickableButton.X, clickableButton.Y, color);
        }
        
        public void RegisterButtonActions(ClickableButton clickableButton)
        {
            if (clickableButton.LoadRaws.Count > 0)
            {
                _novationController.IntegrationManager.Integrations.ForEach(x =>
                {
                    x.CheckLoadAction(clickableButton);
                });
            }
            
            if (clickableButton.ClickRaws.Count > 0)
            {
                _novationController.IntegrationManager.Integrations.ForEach(x =>
                {
                    x.CheckClickAction(clickableButton);
                });
            }
        }
    }
}