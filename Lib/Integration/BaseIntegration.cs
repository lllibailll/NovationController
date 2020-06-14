using System;
using System.IO;
using System.Linq;
using Lib.Click;
using Lib.Manager;
using Newtonsoft.Json;

namespace Lib.Integration
{
    public abstract class BaseIntegration
    {
        protected LaunchpadManager _launchpadManager;
        private string _name;
        private string _actionPrefix;

        private string IntegrationPath;
        private string ConfigPath;

        public BaseIntegration(LaunchpadManager launchpadManager, string name, string actionPrefix)
        {
            _launchpadManager = launchpadManager;
            _name = name;
            _actionPrefix = actionPrefix;
            _launchpadManager.RegisterIntegration(this);
            
            Console.WriteLine("Loading config...");
            IntegrationPath = $"Config/Integration/{_name}";
            ConfigPath = $"{IntegrationPath}/config.json";
            LoadConfig();
            
            OnLoad();
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

        protected string GetRawConfig()
        {
            if (!Directory.Exists(IntegrationPath))
            {
                Directory.CreateDirectory($"Config/Integration/{_name}");
            }

            if (File.Exists(ConfigPath))
            {
                return File.ReadAllText(ConfigPath);
            }

            return null;
        }

        protected string CreateConfig<T>(T config)
        {
            Console.WriteLine($"Creating config for {_name}");
            var file = File.Create($"Config/Integration/{_name}/config.json");

            var raw = JsonConvert.SerializeObject(config);

            var writerStream = new StreamWriter(file);
            writerStream.Write(raw);

            writerStream.Close();
            file.Close();

            return raw;
        }

        public virtual void OnLoad()
        {
            
        }

        protected abstract void SetupLoadAction(ClickableButton clickableButton, string[] data);
        protected abstract void SetupClickAction(ClickableButton clickableButton, string[] data);
        protected abstract void LoadConfig();

        private static string[] GetData(string data)
        {
            return data.Split(LaunchpadManager.ProfileSplitter);
        }
    }
}