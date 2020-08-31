using NovationController.Lib.Integration.Discord;

namespace NovationController.Lib.DiscordIPC.Event
{
    public class AuthorizeEvent
    {
        public DiscordAuthConfig DiscordAuthConfig { get; }

        public AuthorizeEvent(DiscordAuthConfig discordAuthConfig)
        {
            DiscordAuthConfig = discordAuthConfig;
        }
    }
}