using System.Collections.Generic;
using Newtonsoft.Json;

namespace Lib.Integration.PhilipsHue.Model
{
    public partial class State
    {
        [JsonProperty("on")]
        public bool On { get; set; }

        [JsonProperty("bri")]
        public long Bri { get; set; }

        [JsonProperty("hue")]
        public long Hue { get; set; }

        [JsonProperty("sat")]
        public long Sat { get; set; }

        [JsonProperty("effect")]
        public string Effect { get; set; }

        [JsonProperty("xy")]
        public List<double> Xy { get; set; }

        [JsonProperty("ct")]
        public long Ct { get; set; }

        [JsonProperty("alert")]
        public string Alert { get; set; }

        [JsonProperty("colormode")]
        public string Colormode { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("reachable")]
        public bool Reachable { get; set; }
    }
}