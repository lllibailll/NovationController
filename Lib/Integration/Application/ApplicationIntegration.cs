using System.Diagnostics;
using Lib.Click;
using Lib.Manager;
using log4net.Repository.Hierarchy;

namespace Lib.Integration.Application
{
    public class ApplicationIntegration : BaseIntegration
    {
        public ApplicationIntegration(Lib.NovationController novationController, string name, string actionPrefix) : base(novationController, name, actionPrefix)
        {
            
        }
        
        protected override void LoadConfig()
        {
            
        }
        
        private static void StartApp(string path)
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
    }
}