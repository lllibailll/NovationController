using System.Collections.Generic;
using Newtonsoft.Json;

namespace NovationController.Lib.Integration.MagicHome.Model
{
    public class DeviceListResponse
    {
        [JsonProperty("code")]
        public long Code { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("data")]
        public List<DeviceData> Data { get; set; }
    }
}