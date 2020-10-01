using System.Diagnostics;
using NovationController.Lib.Click;

namespace NovationController.Lib.Integration.Application
{
    public class WebIntegration : BaseIntegration
    {
        public WebIntegration(Lib.NovationController novationController, string name, string actionPrefix) : base(novationController, name, actionPrefix)
        {
            
        }
        
        public override void LoadConfig()
        {
            
        }

        private void OpenWeb(string path)
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
        
        protected override void SetupColorControllerAction(ClickableButton clickableButton, string[] data)
        {
            
        }
    }
}