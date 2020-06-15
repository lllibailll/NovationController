using System;
using System.Collections.Generic;
using Lib.Integration;
using Lib.Integration.Application;
using Lib.Integration.Discord;
using Lib.Integration.MagicHome;
using Lib.Integration.Media;
using Lib.Integration.NovationController;
using Lib.Integration.PhilipsHue;
using Lib.Integration.Steam;
using log4net;

namespace Lib.Manager
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

        public void Shutdown()
        {
            Integrations.ForEach(x => x.OnStop());
        }
    }
}