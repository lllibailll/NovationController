using Newtonsoft.Json;

namespace NovationController.Lib.Integration.PhilipsHue.Model
{
    public partial class Startup
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("configured")]
        public bool Configured { get; set; }
    }
}