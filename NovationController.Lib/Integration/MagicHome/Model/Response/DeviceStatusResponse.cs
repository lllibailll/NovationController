using Newtonsoft.Json;

namespace NovationController.Lib.Integration.MagicHome.Model.Response
{
    public class DeviceStatusResponse
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }
}