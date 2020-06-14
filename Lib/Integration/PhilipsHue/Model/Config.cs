using Newtonsoft.Json;

namespace Lib.Integration.PhilipsHue.Model
{
    public partial class Config
    {
        [JsonProperty("archetype")]
        public string Archetype { get; set; }

        [JsonProperty("function")]
        public string Function { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("startup")]
        public Startup Startup { get; set; }
    }
}