using System.Diagnostics;
using Lib.Click;
using Lib.Manager;

namespace Lib.Integration.Application
{
    public class WebIntegration : BaseIntegration
    {
        public WebIntegration(Lib.NovationController novationController, string name, string actionPrefix) : base(novationController, name, actionPrefix)
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
            Log.Debug($"Opened {path}");
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