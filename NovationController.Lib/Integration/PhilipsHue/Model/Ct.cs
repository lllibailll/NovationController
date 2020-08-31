using Newtonsoft.Json;

namespace NovationController.Lib.Integration.PhilipsHue.Model
{
    public partial class Ct
    {
        [JsonProperty("min")]
        public long Min { get; set; }

        [JsonProperty("max")]
        public long Max { get; set; }
    }
}