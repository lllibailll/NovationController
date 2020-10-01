using System.Diagnostics;
using NovationController.Lib.Click;

namespace NovationController.Lib.Integration.Application
{
    public class ApplicationIntegration : BaseIntegration
    {
        public ApplicationIntegration(Lib.NovationController novationController, string name, string actionPrefix) : base(novationController, name, actionPrefix)
        {
            
        }
        
        protected override void LoadConfig()
        {
            
        }
        
        private void StartApp(string path)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(startInfo);
            Log.Debug($"Started {path}");
        }

        protected override void SetupClickAction(ClickableButton clickableButton, string[] data)
        {
            clickableButton.ClickCallbacks.Add(() =>
            {
                StartApp(data[1]);
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