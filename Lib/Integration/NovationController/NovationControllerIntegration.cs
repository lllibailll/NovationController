using Lib.Click;
using Lib.Manager;

namespace Lib.Integration.NovationController
{
    public class NovationControllerIntegration : BaseIntegration
    {
        public NovationControllerIntegration(Lib.NovationController novationController, string name, string actionPrefix) : base(novationController, name, actionPrefix)
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
                        _novationController.LaunchpadManager.Shutdown();
                        break;
                    }

                    case "ToggleLights":
                    {
                        _novationController.LaunchpadManager.ToggleLights();
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