using System;
using System.IO;
using log4net;
using log4net.Config;
using NovationController.Lib.Manager;

namespace NovationController.Lib
{
    public class NovationController
    {
        private ILog Log;
        
        private bool _isRunning = true;

        public LaunchpadManager LaunchpadManager { get; }
        public ProfileManager ProfileManager { get; }
        public IntegrationManager IntegrationManager { get; }

        public NovationController()
        {
            InitLogger();

            LaunchpadManager = new LaunchpadManager(this);
            
            IntegrationManager = new IntegrationManager(this);
            ProfileManager = new ProfileManager(this);

            IntegrationManager.LoadIntegrations();
            ProfileManager.LoadProfiles();
            LaunchpadManager.RegisterButtonListener();
            
            Log.Info("Ready!");
        }

        private void InitLogger()
        {
            LogManager.CreateRepository("nc");
            var logRepository = LogManager.GetRepository("nc");
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            Log = LogManager.GetLogger("nc", "novation-control");
        }

        public void Shutdown()
        {
            Log.Info("Shutting down...");
            _isRunning = false;
            LaunchpadManager.Shutdown();
            IntegrationManager.Shutdown();
            ProfileManager.Shutdown();
            
            Environment.Exit(0);
        }
    }
}