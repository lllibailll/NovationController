using System;
using Newtonsoft.Json;

namespace Lib.Integration.PhilipsHue.Model
{
    public partial class Swupdate
    {
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("lastinstall")]
        public DateTimeOffset Lastinstall { get; set; }
    }
}