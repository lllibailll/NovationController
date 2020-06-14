using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Lib.Config
{
    public class IntegrationConfig
    {
        [JsonProperty("philips_url")]
        public string PhilipsUrl { get; set; }
        
        [JsonProperty("philips_token")]
        public string PhilipsToken { get; set; }
    }
}