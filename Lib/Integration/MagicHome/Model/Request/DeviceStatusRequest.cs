using Newtonsoft.Json;

namespace Lib.Integration.MagicHome.Model.Request
{
    public class DeviceStatusRequest
    {
        [JsonProperty("macAddress")]
        public string MacAddress { get; set; }

        [JsonProperty("hexData")] 
        public string HexData { get; } = "818a8b96";

        [JsonProperty("responseCount")] 
        public long ResponseCount { get; } = 14;
    }
}