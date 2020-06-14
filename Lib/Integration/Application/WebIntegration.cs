using System.Diagnostics;
using Lib.Click;
using Lib.Manager;

namespace Lib.Integration.Application
{
    public class WebIntegration : BaseIntegration
    {
        public WebIntegration(LaunchpadManager launchpadManager, string name, string actionPrefix) : base(launchpadManager, name, actionPrefix)
        {
            
        }
        
        protected override void LoadConfig()
        {
            
        }

        private static void OpenWeb(string path)
        {
            var ps = new ProcessStartInfo(path)
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
                OpenWeb(data[1]);
            });
        }
        
        protected override void SetupLoadAction(ClickableButton clickableButton, string[] data)
        {
            
        }
    }
}