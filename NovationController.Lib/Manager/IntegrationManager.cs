using System.Collections.Generic;
using System.Linq;
using log4net;
using NovationController.Lib.Integration;
using NovationController.Lib.Integration.Application;
using NovationController.Lib.Integration.Discord;
using NovationController.Lib.Integration.MagicHome;
using NovationController.Lib.Integration.Media;
using NovationController.Lib.Integration.NovationController;
using NovationController.Lib.Integration.PhilipsHue;
using NovationController.Lib.Integration.Steam;

namespace NovationController.Lib.Manager
{
    public class IntegrationManager
    {
        private readonly ILog Log = LogManager.GetLogger("nc", "integration-manager");
        private NovationController _novationController;

        public List<BaseIntegration> Integrations { get; } = new List<BaseIntegration>();

        public IntegrationManager(NovationController novationController)
        {
            _novationController = novationController;
        }

        public void LoadIntegrations()
        {
            Log.Info("Loading integrations...");
            new NovationControllerIntegration(_novationController, "NovationController", "NovationController");
            new MediaIntegration(_novationController, "Media", "Media");
            new ApplicationIntegration(_novationController, "Application", "Proc");
            new WebIntegration(_novationController, "Web", "Web");
            new SteamIntegration(_novationController, "Steam", "Steam");
            new DiscordIntegration(_novationController, "Discord", "Discord");
            new PhilipsHueIntegration(_novationController, "PhilipsHue", "PhilipsHue");
            new MagicHomeIntegration(_novationController, "MagicHome", "MagicHome");
        }

        public void RegisterIntegration(BaseIntegration integration)
        {
            Integrations.Add(integration);
        }

        public void FireColorControl(BaseIntegration integration)
        {
            _novationController.ProfileManager.ActiveProfile.Buttons
                .Where(x => x.ColorControllerCallback != null && x.ColorControllerRaw.StartsWith(integration.Name))
                .ToList()
                .ForEach(x =>
                {
                    x.ColorControllerCallback.Invoke();
                });
        }

        public void Shutdown()
        {
            Integrations.ForEach(x => x.OnStop());
        }
    }
}