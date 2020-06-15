using System;
using Lib.Click;
using Lib.DiscordIPC;
using Lib.Manager;
using Newtonsoft.Json;

namespace Lib.Integration.Discord
{
    public class DiscordIntegration : BaseIntegration
    {
        private DiscordConfig _config;
        private DiscordAuthConfig _authConfig;

        private IpcClient _ipcClient;

        private DiscordVoiceStatus _discordVoiceStatus = new DiscordVoiceStatus();

        public DiscordIntegration(Lib.NovationController novationController, string name, string actionPrefix) : base(novationController, name, actionPrefix)
        {
        }

        public override void OnLoad()
        {
            _ipcClient = new IpcClient(_config.ApplicationId);

            ListenDiscordEvents();

            _ipcClient.Init(_config, _authConfig);
        }

        public override void OnStop()
        {
            _ipcClient.Close();
        }

        private void ListenDiscordEvents()
        {
            _ipcClient.OnAuthorize += (sender, args) =>
            {
                _authConfig = args.DiscordAuthConfig;
                
                WriteToFile(JsonConvert.SerializeObject(_authConfig), "auth_details.json");
            };

            _ipcClient.OnError += (sender, args) =>
            {
                Log.Error(args.Message);
            };

            _ipcClient.OnReady += (sender, args) =>
            {
                _ipcClient.Subscribe("VOICE_SETTINGS_UPDATE");
            };
            
            _ipcClient.OnVoiceSettingsUpdate += (sender, args) =>
            {
                _discordVoiceStatus.Mute = args.Mute;
                _discordVoiceStatus.Deaf = args.Deaf;
            };
        }

        protected override void LoadConfig()
        {
            var configRaw = GetRawConfig() ?? CreateConfig(new DiscordConfig());
            _config = JsonConvert.DeserializeObject<DiscordConfig>(configRaw);

            var authDetails = GetRawConfig("auth_details.json");

            if (authDetails != null)
            {
                _authConfig = JsonConvert.DeserializeObject<DiscordAuthConfig>(authDetails);
            }
        }

        private void ToggleVoiceMute()
        {
            _ipcClient.ToggleMuteStatus(!_discordVoiceStatus.Mute);
        }
        
        private void ToggleVoiceDeaf()
        {
            _ipcClient.ToggleDeafStatus(!_discordVoiceStatus.Deaf);
        }

        protected override void SetupLoadAction(ClickableButton clickableButton, string[] data)
        {
        }

        protected override void SetupClickAction(ClickableButton clickableButton, string[] data)
        {
            clickableButton.ClickCallbacks.Add(() =>
            {
                switch (data[1])
                {
                    case "Toggle":
                    {
                        switch (data[2])
                        {
                            case "Mute":
                            {
                                ToggleVoiceMute();
                                break;
                            }
                            
                            case "Deaf":
                            {
                                ToggleVoiceDeaf();
                                break;
                            }
                        }
                        break;
                    }
                }
            });
        }
    }
}