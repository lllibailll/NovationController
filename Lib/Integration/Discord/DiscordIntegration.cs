﻿using System;
using System.Threading;
using CreateFlags = Lib.Integration.Discord.SDK.CreateFlags;
using LogLevel = Lib.Integration.Discord.SDK.LogLevel;
using OAuth2Token = Lib.Integration.Discord.SDK.OAuth2Token;
using Result = Lib.Integration.Discord.SDK.Result;

namespace Lib.Integration.Discord
{
    public class DiscordInt
    {
        private Discord.SDK.Discord _discord;

        private const long DiscordClientId = 721396708088479825;

        private Thread _thread;

        private bool _isRunning = false;

        public void Init()
        {
            Console.WriteLine($"Initializing Discord Integration for {DiscordClientId}");
            _isRunning = true;
            _discord = new Discord.SDK.Discord(DiscordClientId, (ulong) CreateFlags.Default);
            
            RequestAuth();
            
            var thread = new Thread(() =>
            {
                while (_isRunning)
                {
                    _discord.RunCallbacks();
                    Thread.Sleep(1000 / 60);
                }
            });
            
            thread.Start();
        }

        public Discord.SDK.Discord Discord => _discord;

        public void RequestAuth()
        {
            _discord.GetApplicationManager().GetOAuth2Token((Result result, ref OAuth2Token token) =>
            {
                Console.WriteLine($"res={result} token={token}");
            });
        }
    }
}