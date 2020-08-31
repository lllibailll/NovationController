using System;

namespace NovationController.ConsoleApp
{
    public class NovationConsoleController
    {
        private Lib.NovationController _novationController;

        public NovationConsoleController()
        {
            _novationController = new Lib.NovationController();

            HandleExitEvents();

            var input = string.Empty;

            while(input != "q")
            {
                Console.WriteLine("Enter a command.");

                input = Console.ReadLine();
                switch(input.ToLower())
                {
                    case "c":
                        _novationController.LaunchpadManager.Launchpad.Clear();
                        break;

                    case "r":
                    {
                        _novationController.ProfileManager.Reload();
                        break;
                    }
                }
            }
        }

        private void HandleExitEvents()
        {
            Console.CancelKeyPress += (sender, args) =>
            {
                _novationController.Shutdown();
            };
        }
    }
}