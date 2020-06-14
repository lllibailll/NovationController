using Newtonsoft.Json;

namespace Lib.Integration.PhilipsHue
{
    public class PhilipsHueConfig
    {
        [JsonProperty("philips_url")]
        public string PhilipsUrl { get; set; }
        
        [JsonProperty("philips_token")]
        public string PhilipsToken { get; set; }
    }
}