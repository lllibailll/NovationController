using System.Diagnostics;
using Lib.Click;
using Lib.Manager;

namespace Lib.Integration.Steam
{
    public class SteamIntegration: BaseIntegration
    {
        public SteamIntegration(LaunchpadManager launchpadManager, string name, string actionPrefix) : base(launchpadManager, name, actionPrefix)
        {
            
        }

        protected override void LoadConfig()
        {
            
        }

        private static void StartApp(long id)
        {
            var ps = new ProcessStartInfo(@$"steam://rungameid/{id}")
            { 
                UseShellExecute = true, 
                Verb = "open" 
            };
            Process.Start(ps);
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
    }
}