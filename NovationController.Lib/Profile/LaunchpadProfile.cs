using System.Collections.Generic;
using Newtonsoft.Json;
using NovationController.Lib.Click;

namespace NovationController.Lib.Profile
{
    public class LaunchpadProfile
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("x")]
        public int CoordX { get; set; }
        
        [JsonProperty("y")]
        public int CoordY { get; set; }

        [JsonProperty("buttons")]
        private List<ClickableButton> _buttons = new List<ClickableButton>();

        public List<ClickableButton> Buttons => _buttons;
    }
}