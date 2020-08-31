using Newtonsoft.Json;

namespace NovationController.Lib.Integration.MagicHome
{
    public class MagicHomeConfig
    {
        [JsonProperty("magic_home_url")]
        public string MagicHomeUrl { get; set; }
        
        [JsonProperty("magic_home_token")]
        public string MagicHomeToken { get; set; }
    }
}