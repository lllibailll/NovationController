using Newtonsoft.Json;

namespace Lib.Integration.MagicHome.Model
{
    public class DeviceData
    {
        [JsonProperty("deviceType")]
        public long DeviceType { get; set; }

        [JsonProperty("ledVersionNum")]
        public long LedVersionNum { get; set; }

        [JsonProperty("moduleID")]
        public string ModuleId { get; set; }

        [JsonProperty("macAddress")]
        public string MacAddress { get; set; }

        [JsonProperty("timeZoneID")]
        public string TimeZoneId { get; set; }

        [JsonProperty("dstOffset")]
        public long DstOffset { get; set; }

        [JsonProperty("rawOffset")]
        public long RawOffset { get; set; }

        [JsonProperty("deviceName")]
        public string DeviceName { get; set; }

        [JsonProperty("state")]
        public object State { get; set; }

        [JsonProperty("firmwareVer")]
        public object FirmwareVer { get; set; }

        [JsonProperty("routerSSID")]
        public object RouterSsid { get; set; }

        [JsonProperty("localIP")]
        public object LocalIp { get; set; }

        [JsonProperty("routerRssi")]
        public long RouterRssi { get; set; }

        [JsonProperty("isOnline")]
        public bool IsOnline { get; set; }
    }
}