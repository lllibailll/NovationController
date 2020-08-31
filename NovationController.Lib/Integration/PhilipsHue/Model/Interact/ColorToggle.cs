using System.Collections.Generic;
using Newtonsoft.Json;

namespace NovationController.Lib.Integration.PhilipsHue.Model.Interact
{
    public class ColorToggle
    {
        [JsonProperty("bri")]
        public int Brightness { get; set; }
        
        [JsonProperty("xy")]
        public List<double> XY { get; set; }
    }
}