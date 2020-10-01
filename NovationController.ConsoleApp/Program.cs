using System;
using System.Threading;

namespace NovationController.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"
 _   _                 _   _               _____             _             _ _           
| \ | |               | | (_)             /  __ \           | |           | | |          
|  \| | _____   ____ _| |_ _  ___  _ __   | /  \/ ___  _ __ | |_ _ __ ___ | | | ___ _ __ 
| . ` |/ _ \ \ / / _` | __| |/ _ \| '_ \  | |    / _ \| '_ \| __| '__/ _ \| | |/ _ \ '__|
| |\  | (_) \ V / (_| | |_| | (_) | | | | | \__/\ (_) | | | | |_| | | (_) | | |  __/ |   
\_| \_/\___/ \_/ \__,_|\__|_|\___/|_| |_|  \____/\___/|_| |_|\__|_|  \___/|_|_|\___|_|   
                                                                                         
                                                                                         
");
            new NovationConsoleController();
        }
    }
}