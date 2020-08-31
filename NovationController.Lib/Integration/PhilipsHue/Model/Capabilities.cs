using Newtonsoft.Json;

namespace NovationController.Lib.Integration.PhilipsHue.Model
{
    public partial class Capabilities
    {
        [JsonProperty("certified")]
        public bool Certified { get; set; }

        [JsonProperty("control")]
        public Control Control { get; set; }

        [JsonProperty("streaming")]
        public Streaming Streaming { get; set; }
    }
}