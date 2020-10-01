﻿using System.Diagnostics;
using NovationController.Lib.Click;

namespace NovationController.Lib.Integration.Steam
{
    public class SteamIntegration: BaseIntegration
    {
        public SteamIntegration(Lib.NovationController novationController, string name, string actionPrefix) : base(novationController, name, actionPrefix)
        {
        }

        public override void LoadConfig()
        {
            
        }

        private void StartApp(long id)
        {
            var ps = new ProcessStartInfo(@$"steam://rungameid/{id}")
            { 
                UseShellExecute = true, 
                Verb = "open" 
            };
            Process.Start(ps);
            Log.Debug($"Launching {id}");
        }

        protected override void SetupClickAction(ClickableButton clickableButton, string[] data)
        {
            clickableButton.ClickCallbacks.Add(() =>
            {
                StartApp(int.Parse(data[1]));
            });
        }
        
        protected override void SetupLoadAction(ClickableButton clickableButton, string[] data)
        {
            
        }
        
        protected override void SetupColorControllerAction(ClickableButton clickableButton, string[] data)
        {
            
        }
    }
}