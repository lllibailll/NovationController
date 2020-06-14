using Lib.Integration.Discord;

namespace Lib.DiscordIPC.Event
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