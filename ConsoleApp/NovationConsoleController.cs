using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using Lib;
using Lib.Click;
using Lib.Integration;
using Lib.Integration.Application;
using Lib.Integration.Discord.SDK;
using Lib.Integration.Media;
using Lib.Manager;

namespace ConsoleApp
{
    public class NovationConsoleController
    {
        private LaunchpadManager _launchpadManager;
        
        private long DiscordUserId = 221659451231436800;

        public NovationConsoleController()
        {
            _launchpadManager = new LaunchpadManager();
            
            // _launchpadManager.InitDiscord();
            
            // var userManager = _launchpadManager.DiscordInt.Discord.GetUserManager();

            // userManager.OnCurrentUserUpdate += () =>
            // {
            //     var currentUser = userManager.GetCurrentUser();
            //
            //     Console.WriteLine(currentUser.Username);
            //     Console.WriteLine(currentUser.Id);
            //     Console.WriteLine(currentUser.Discriminator);
            //     Console.WriteLine(currentUser.Avatar);
            // };

            // _launchpadManager.DiscordInt.Discord.SetLogHook(LogLevel.Debug, (level, message) =>
            // {
            //     Console.WriteLine("Log[{0}] {1}", level, message);
            // });

            HandleExitEvents();
            
            LoadProfiles();

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
        
        private void LoadProfiles()
        {
            var profile = new LaunchpadProfile
            {
                Name = "Default profile",
                Id = 0,
                LaunchpadCoord = new LaunchpadCoord(8, 7)
            };

            profile.AddButton(new ClickableButton
            {
                Name = "Exit",
                X = 0,
                Y = 0,
                Color = Color.DarkRed,
                ClickCallback = () =>
                {
                    Environment.Exit(0);
                }
            });
            
            profile.AddButton(new ClickableButton
            {
                Name = "YouTube",
                X = 0,
                Y = 7,
                Color = Color.Red,
                ClickCallback = () =>
                {
                    ApplicationIntegration.OpenWeb("https://youtube.com");
                }
            });
            
            profile.AddButton(new ClickableButton
            {
                Name = "Twitter",
                X = 1,
                Y = 7,
                Color = Color.MediumBlue,
                ClickCallback = () =>
                {
                    ApplicationIntegration.OpenWeb("https://twitter.com");
                }
            });
            
            profile.AddButton(new ClickableButton
            {
                Name = "Calc",
                X = 0,
                Y = 6,
                Color = Color.Beige,
                ClickCallback = () =>
                {
                    ApplicationIntegration.StartApp("calc.exe");
                }
            });
            
            profile.AddButton(new ClickableButton
            {
                Name = "Minecraft",
                X = 0,
                Y = 4,
                Color = Color.Green,
                ClickCallback = () =>
                {
                    ApplicationIntegration.StartApp(@"C:\Program Files (x86)\Minecraft Launcher\MinecraftLauncher.exe");
                }
            });
            
            profile.AddButton(new ClickableButton
            {
                Name = "CS:GO",
                X = 1,
                Y = 4,
                Color = Color.Yellow,
                ClickCallback = () =>
                {
                    SteamIntegration.StartApp(730);
                }
            });

            profile.AddButton(new ClickableButton
            {
                Name = "MusicPrev",
                X = 5,
                Y = 1,
                Color = Color.LawnGreen,
                ClickCallback = MediaIntegration.Prev
            });
            
            profile.AddButton(new ClickableButton
            {
                Name = "MusicNext",
                X = 6,
                Y = 1,
                Color = Color.LimeGreen,
                ClickCallback = MediaIntegration.Next
            });
            
            profile.AddButton(new ClickableButton
            {
                Name = "MusicStop",
                X = 7,
                Y = 1,
                Color = Color.DarkGreen,
                ClickCallback = MediaIntegration.PlayPause
            });
            
            _launchpadManager.AddProfile(profile);

            var profileTest = new LaunchpadProfile
            {
                Id = 1,
                Name = "Test profile",
                LaunchpadCoord = new LaunchpadCoord(8, 6)
            };
            
            profileTest.AddButton(new ClickableButton
            {
                Name = "YouTube 2",
                X = 0,
                Y = 7,
                Color = Color.Red,
                ClickCallback = () =>
                {
                    ApplicationIntegration.OpenWeb("https://youtube.com/lllibailll");
                }
            });
            
            _launchpadManager.AddProfile(profileTest);
        }

        private void HandleExitEvents()
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
            {
                _launchpadManager.Shutdown();
            };

            Console.CancelKeyPress += (sender, args) =>
            {
                _launchpadManager.Shutdown();
            };
        }
    }
}