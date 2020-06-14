using Newtonsoft.Json;

namespace Lib.Integration.PhilipsHue.Model.Interact
{
    public class StateToggle
    {
        [JsonProperty("on")]
        public bool On { get; set; }
    }
}