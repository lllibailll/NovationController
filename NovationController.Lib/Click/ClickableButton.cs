using System;
using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;

namespace NovationController.Lib.Click
{
    public class ClickableButton
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("x")]
        public int X { get; set; }
        
        [JsonProperty("y")]
        public int Y { get; set; }
        
        [JsonProperty("color")]
        public Color Color { get; set; }
        
        [JsonProperty("click")]
        public List<string> ClickRaws { get; set; }
        
        [JsonProperty("load")]
        public List<string> LoadRaws { get; set; }
        
        [JsonProperty("color_controller")]
        public string ColorControllerRaw { get; set; }

        public List<Action> LoadCallbacks { get; set; } = new List<Action>();
        
        public List<Action> ClickCallbacks { get; set; } = new List<Action>();
        
        public Action ColorControllerCallback { get; set; }
    }
}