using Newtonsoft.Json;

namespace NovationController.Lib.Integration.PhilipsHue.Model
{
    public partial class SmartItem
    {
        [JsonProperty("state")]
        public State State { get; set; }

        [JsonProperty("swupdate")]
        public Swupdate Swupdate { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("modelid")]
        public string Modelid { get; set; }

        [JsonProperty("manufacturername")]
        public string Manufacturername { get; set; }

        [JsonProperty("productname")]
        public string Productname { get; set; }

        [JsonProperty("capabilities")]
        public Capabilities Capabilities { get; set; }

        [JsonProperty("config")]
        public Config Config { get; set; }

        [JsonProperty("uniqueid")]
        public string Uniqueid { get; set; }

        [JsonProperty("swversion")]
        public string Swversion { get; set; }

        [JsonProperty("swconfigid")]
        public string Swconfigid { get; set; }

        [JsonProperty("productid")]
        public string Productid { get; set; }
    }
}