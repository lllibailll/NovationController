using Newtonsoft.Json;

namespace Lib.Config
{
    public class IntegrationConfig
    {
        [JsonProperty("philips_url")]
        public string PhilipsUrl { get; set; }
        
        [JsonProperty("philips_token")]
        public string PhilipsToken { get; set; }
        
        [JsonProperty("magic_home_url")]
        public string MagicHomeUrl { get; set; }
        
        [JsonProperty("magic_home_token")]
        public string MagicHomeToken { get; set; }
    }
}