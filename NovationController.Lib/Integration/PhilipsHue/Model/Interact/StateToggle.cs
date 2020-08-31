using Newtonsoft.Json;

namespace NovationController.Lib.Integration.PhilipsHue.Model.Interact
{
    public class StateToggle
    {
        [JsonProperty("on")]
        public bool On { get; set; }
    }
}