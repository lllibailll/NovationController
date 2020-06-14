using Lib.Click;
using Lib.Manager;

namespace Lib.Integration
{
    public abstract class BaseIntegration
    {
        protected LaunchpadManager _launchpadManager;
        private string _actionPrefix;

        public BaseIntegration(LaunchpadManager launchpadManager, string actionPrefix)
        {
            _launchpadManager = launchpadManager;
            _actionPrefix = actionPrefix;
            _launchpadManager.RegisterIntegration(this);
        }
        
        public void CheckLoadAction(ClickableButton clickableButton)
        {
            if (clickableButton.ClickRaw.StartsWith(_actionPrefix))
            {
                SetupLoadAction(clickableButton, GetData(clickableButton));
            }
        }

        public void CheckClickAction(ClickableButton clickableButton)
        {
            if (clickableButton.ClickRaw.StartsWith(_actionPrefix))
            {
                SetupClickAction(clickableButton, GetData(clickableButton));
            }
        }

        protected abstract void SetupLoadAction(ClickableButton clickableButton, string[] data);
        protected abstract void SetupClickAction(ClickableButton clickableButton, string[] data);

        private static string[] GetData(ClickableButton clickableButton)
        {
            return clickableButton.ClickRaw.Split(LaunchpadManager.ProfileSplitter);
        }
    }
}