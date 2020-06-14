using Lib.Click;
using Lib.Manager;

namespace Lib.Integration.NovationController
{
    public class NovationControllerIntegration : BaseIntegration
    {
        public NovationControllerIntegration(LaunchpadManager launchpadManager, string name, string actionPrefix) : base(launchpadManager, name, actionPrefix)
        {
        }

        protected override void SetupLoadAction(ClickableButton clickableButton, string[] data)
        {
            
        }

        protected override void SetupClickAction(ClickableButton clickableButton, string[] data)
        {
            clickableButton.ClickCallbacks.Add(() =>
            {
                switch (data[1])
                {
                    case "Exit":
                    {
                        _launchpadManager.Shutdown();
                        break;
                    }

                    case "ToggleLights":
                    {
                        _launchpadManager.ToggleLights();
                        break;   
                    }
                }
            });
        }

        protected override void LoadConfig()
        {
            
        }
    }
}