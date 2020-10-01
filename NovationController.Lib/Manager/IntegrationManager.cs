using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using Newtonsoft.Json;
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
        
        private string _integrationsConfigPath = "Config/Integration/integrations.json";
        
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

            if (!File.Exists(_integrationsConfigPath))
            {
                var defaultConfigs = new List<GenericIntegrationConfig>();
                
                Integrations.ForEach(x => 
                    defaultConfigs.Add(new GenericIntegrationConfig
                    {
                        Name = x.Name,
                        Enabled = false
                    })
                );
                
                var raw = JsonConvert.SerializeObject(defaultConfigs, Formatting.Indented);

                var file = File.Create(_integrationsConfigPath);
                
                var writerStream = new StreamWriter(file);
                writerStream.Write(raw);

                writerStream.Close();
                file.Close();
            }

            var integrationsConfigs = JsonConvert.DeserializeObject<List<GenericIntegrationConfig>>(File.ReadAllText(_integrationsConfigPath));
            
            Integrations.ForEach(x =>
            {
                var integrationConfig = integrationsConfigs.FirstOrDefault(y => y.Name.Equals(x.Name));

                if (integrationConfig != null && integrationConfig.Enabled)
                {
                    x.Enabled = true;
                    x.LoadConfig();
                    x.OnLoad();
                }
            });
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