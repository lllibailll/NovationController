using System;
using System.IO;
using System.Linq;
using Lib.Click;
using Lib.Manager;
using log4net;
using Newtonsoft.Json;

namespace Lib.Integration
{
    public abstract class BaseIntegration
    {
        protected static ILog Log;
        protected Lib.NovationController _novationController;
        private string _name;
        private string _actionPrefix;

        private string IntegrationPath;

        public BaseIntegration(Lib.NovationController novationController, string name, string actionPrefix)
        {
            Log = LogManager.GetLogger("nc", $"{name.ToLower()}-integration");
            Log.Info("Loading...");
            _novationController = novationController;
            _name = name;
            _actionPrefix = actionPrefix;
            _novationController.IntegrationManager.RegisterIntegration(this);
            
            IntegrationPath = $"Config/Integration/{_name}";
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

        protected string CreateConfig<T>(T config)
        {
            Log.Debug($"Creating config for {_name}");
            var file = File.Create($"Config/Integration/{_name}/config.json");

            var raw = JsonConvert.SerializeObject(config);

            var writerStream = new StreamWriter(file);
            writerStream.Write(raw);

            writerStream.Close();
            file.Close();

            return raw;
        }

        protected string GetRawConfig(string name = "config.json")
        {
            if (!Directory.Exists(IntegrationPath))
            {
                Directory.CreateDirectory($"Config/Integration/{_name}");
            }

            var path = $"{IntegrationPath}/{name}";
            
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        protected void WriteToFile(string content, string fileName)
        {
            var fileDir = $"{IntegrationPath}/{fileName}";
            var file = File.Open(fileDir, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var writer = new StreamWriter(file);
            writer.Write(content);
            
            writer.Close();
            file.Close();
        }

        public virtual void OnLoad()
        {
            
        }
        
        public virtual void OnStop()
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