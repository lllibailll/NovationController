using Newtonsoft.Json;

namespace NovationController.Lib.Integration.Discord
{
    public class DiscordConfig
    {
        [JsonProperty("enabled")] 
        public bool Enabled { get; set; }
        
        [JsonProperty("application_id")] 
        public ulong ApplicationId { get; set; }

        [JsonProperty("secret")] 
        public string Secret { get; set; }
    }
}