using System.Linq;
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
            clickableButton.LoadRaws
                .Where(x => x.StartsWith(_actionPrefix))
                .ToList()
                .ForEach(x =>
                {
                    SetupLoadAction(clickableButton, GetData(x));
                });
        }

        public void CheckClickAction(ClickableButton clickableButton)
        {
            clickableButton.ClickRaws
                .Where(x => x.StartsWith(_actionPrefix))
                .ToList()
                .ForEach(x =>
                {
                    SetupClickAction(clickableButton, GetData(x));
                });
        }

        protected abstract void SetupLoadAction(ClickableButton clickableButton, string[] data);
        protected abstract void SetupClickAction(ClickableButton clickableButton, string[] data);

        private static string[] GetData(string data)
        {
            return data.Split(LaunchpadManager.ProfileSplitter);
        }
    }
}