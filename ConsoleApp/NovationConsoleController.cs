using System;
using Lib.Manager;

namespace ConsoleApp
{
    public class NovationConsoleController
    {
        private LaunchpadManager _launchpadManager;

        public NovationConsoleController()
        {
            _launchpadManager = new LaunchpadManager();

            HandleExitEvents();

            var input = string.Empty;

            while(input != "q")
            {
                Console.WriteLine("Enter a command.");

                input = Console.ReadLine();
                switch(input.ToLower())
                {
                    case "c":
                        _launchpadManager.Launchpad.Clear();
                        break;
                }
                
                Console.Clear();
            }
        }

        private void HandleExitEvents()
        {
            Console.CancelKeyPress += (sender, args) =>
            {
                _launchpadManager.Shutdown();
            };
        }
    }
}