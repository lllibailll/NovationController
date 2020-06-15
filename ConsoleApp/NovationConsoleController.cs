using System;
using Lib;
using Lib.Manager;

namespace ConsoleApp
{
    public class NovationConsoleController
    {
        private NovationController _novationController;

        public NovationConsoleController()
        {
            _novationController = new NovationController();

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
                }
                
                Console.Clear();
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