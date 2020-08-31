using System.Collections.Generic;
using Newtonsoft.Json;

namespace NovationController.Lib.Integration.PhilipsHue.Model
{
    public partial class Control
    {
        [JsonProperty("mindimlevel")]
        public long Mindimlevel { get; set; }

        [JsonProperty("maxlumen")]
        public long Maxlumen { get; set; }

        [JsonProperty("colorgamuttype")]
        public string Colorgamuttype { get; set; }

        [JsonProperty("colorgamut")]
        public List<List<double>> Colorgamut { get; set; }

        [JsonProperty("ct")]
        public Ct Ct { get; set; }
    }
}