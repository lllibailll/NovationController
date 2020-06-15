using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Lib.Click;
using Lib.Integration.MagicHome.Model;
using Lib.Integration.MagicHome.Model.Request;
using Lib.Integration.MagicHome.Model.Response;
using Lib.Manager;
using Newtonsoft.Json;
using RestSharp;

namespace Lib.Integration.MagicHome
{
    public class MagicHomeIntegration : BaseIntegration
    {
        private const string UserAgent = "Magic Home/1.5.1(ANDROID,10,en-US)";

        private const string StatusOn = "813323610110cbcbcb00040000ae";
        private const string StatusOff = "813324610110cbcbcb00040000af";
        
        private const string ToggleOn = "71230fa3";
        private const string ToggleOff = "71240fa4";

        private MagicHomeConfig _config;

        private RestClient _restClient;

        private List<MagicHomeDevice> _devices = new List<MagicHomeDevice>();

        public MagicHomeIntegration(Lib.NovationController novationController, string name, string actionPrefix) : base(novationController, name, actionPrefix)
        {
        }

        public override void OnLoad()
        {
            _restClient = new RestClient(_config.MagicHomeUrl);
            _restClient.AddDefaultHeader("token", _config.MagicHomeToken);
            _restClient.UserAgent = UserAgent;
            
            LoadDevices();
        }
        
        protected override void LoadConfig()
        {
            var configRaw = GetRawConfig() ?? CreateConfig(new MagicHomeConfig());
            _config = JsonConvert.DeserializeObject<MagicHomeConfig>(configRaw);
        }

        private void LoadDevices()
        {
            Log.Info("Loading devices...");

            var res = _restClient.Get(new RestRequest(DeviceListEndpoint, DataFormat.Json));
            _devices = JsonConvert.DeserializeObject<DeviceListResponse>(res.Content).Data.Select(x => new MagicHomeDevice
            {
                On = false,
                DeviceData = x
            }).ToList();

            _devices.ForEach(x =>
            {
                Log.Debug($"Found {x.DeviceData.DeviceName}");
            });

            _devices
                .Where(x => x.DeviceData.IsOnline)
                .ToList()
                .ForEach(x =>
                {

                    var req = new RestRequest(DeviceStatusEndpoint, DataFormat.Json);
                    req.AddJsonBody(JsonConvert.SerializeObject(new DeviceStatusRequest
                    {
                        MacAddress = x.DeviceData.MacAddress
                    }));

                    var deviceStatusRaw = _restClient.Post(req).Content;

                    var status = JsonConvert.DeserializeObject<DeviceStatusResponse>(deviceStatusRaw);
                    x.On = status.Data.Equals(StatusOn);
                    Log.Debug($"[MagicHome] {x.DeviceData.DeviceName} is on: {x.On}");
                });
        }

        private void Toggle(ClickableButton clickableButton, MagicHomeDevice magicHomeDevice)
        {

            var toggleRequest = new StatusToggleRequest
            {
                DataCommandItems = new List<DataCommandItem>
                {
                    new DataCommandItem
                    {
                        HexData = magicHomeDevice.On ? ToggleOff : ToggleOn,
                        MacAddress = magicHomeDevice.DeviceData.MacAddress
                    }
                }
            };
            
            var req = new RestRequest(DeviceToggleEndpoint, DataFormat.Json);

            req.AddJsonBody(JsonConvert.SerializeObject(toggleRequest));
            
            var res = _restClient.Post(req).Content;

            magicHomeDevice.On = !magicHomeDevice.On;

            CheckButtonColor(clickableButton, magicHomeDevice);
            
            Log.Debug($"[MagicHome] Toggle res: {res}");
        }

        private void CheckButtonColor(ClickableButton clickableButton, MagicHomeDevice magicHomeDevice)
        {
            if (magicHomeDevice.On)
            {
                _novationController.LaunchpadManager.SetButtonColor(clickableButton, Color.Green);
            }
            else
            {
                _novationController.LaunchpadManager.SetButtonColor(clickableButton, Color.Red);
            }
        }

        public MagicHomeDevice GetByMac(string mac)
        {
            return _devices.FirstOrDefault(x => x.DeviceData.MacAddress.ToLower().Equals(mac.ToLower()));
        }

        public MagicHomeDevice GetByName(string name)
        {
            return _devices.FirstOrDefault(x => x.DeviceData.DeviceName.ToLower().Equals(name.ToLower()));
        }

        public string DeviceListEndpoint => $"{_config.MagicHomeUrl}/app/getMyBindDevicesAndState/ZG001";
        public string DeviceToggleEndpoint => $"{_config.MagicHomeUrl}/app/sendCommandBatch/ZG001";
        public string DeviceStatusEndpoint => $"{_config.MagicHomeUrl}/app/sendRequestCommand/ZG001";
        
        protected override void SetupClickAction(ClickableButton clickableButton, string[] data)
        {
            clickableButton.ClickCallbacks.Add(() =>
            {
                switch (data[1])
                {
                    case "Toggle":
                    {
                        Toggle(clickableButton, GetByMac(data[2]));
                        break;
                    }
                }
            });
        }

        protected override void SetupLoadAction(ClickableButton clickableButton, string[] data)
        {
            clickableButton.LoadCallbacks.Add(() =>
            {
                CheckButtonColor(clickableButton, GetByMac(data[1]));
            });
        }
    }
}