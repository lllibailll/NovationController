using System;
using Lib.Manager;

namespace Lib
{
    public class NovationController
    {
        private bool _isRunning = true;

        public LaunchpadManager LaunchpadManager { get; }
        public ProfileManager ProfileManager { get; }
        public IntegrationManager IntegrationManager { get; }

        public NovationController()
        {
            LaunchpadManager = new LaunchpadManager(this);
            
            IntegrationManager = new IntegrationManager(this);
            ProfileManager = new ProfileManager(this);

            IntegrationManager.LoadIntegrations();
            ProfileManager.LoadProfiles();
            LaunchpadManager.RegisterButtonListener();
            
            Console.WriteLine("Novation Controller is ready!");
        }

        public void Shutdown()
        {
            _isRunning = false;
            LaunchpadManager.Shutdown();
            IntegrationManager.Shutdown();
            ProfileManager.Shutdown();
            
            Environment.Exit(0);
        }
    }
}