using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.Integration.MagicHome.Model.Request
{
    public class StatusToggleRequest
    {
        [JsonProperty("dataCommandItems")]
        public List<DataCommandItem> DataCommandItems { get; set; }
    }

    public class DataCommandItem
    {
        [JsonProperty("hexData")]
        public string HexData { get; set; }

        [JsonProperty("macAddress")]
        public string MacAddress { get; set; }
    }
}