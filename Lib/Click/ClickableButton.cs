using System;
using System.Drawing;
using Newtonsoft.Json;

namespace Lib.Click
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
        public string ClickRaw { get; set; }
        
        [JsonProperty("load")]
        public string LoadRaw { get; set; }

        public Action LoadCallback { get; set; }
        
        public Action ClickCallback { get; set; }
    }
}