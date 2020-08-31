using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using NovationController.Lib.Click;
using NovationController.Lib.Integration.PhilipsHue.Model;
using NovationController.Lib.Integration.PhilipsHue.Model.Interact;
using RestSharp;

namespace NovationController.Lib.Integration.PhilipsHue
{
    public class PhilipsHueIntegration : BaseIntegration
    {
        private RestClient _restClient;
        private PhilipsHueConfig _config;

        private Dictionary<int, SmartItem> _items = new Dictionary<int, SmartItem>();
        
        // Endpoints
        private string BaseEndpoint => $"{_config.PhilipsUrl}/{_config.PhilipsToken}";
        private string LightsEndpoint => $"{BaseEndpoint}/lights";

        public PhilipsHueIntegration(global::NovationController.Lib.NovationController novationController, string name, string actionPrefix) : base(novationController, name, actionPrefix)
        {
        }

        public override void OnLoad()
        {
            _restClient = new RestClient(BaseEndpoint);
            
            LoadItems();
        }

        protected override void LoadConfig()
        {
            var configRaw = GetRawConfig() ?? CreateConfig(new PhilipsHueConfig());
            _config = JsonConvert.DeserializeObject<PhilipsHueConfig>(configRaw);
        }

        private void Toggle(ClickableButton clickableButton, int id)
        {
            var item = GetById(id);

            if (item == null)
            {
                return;
            }

            SetStatus(clickableButton, id, !item.State.On);

            CheckButtonColor(clickableButton, GetById(id));
        }

        private void SetStatus(ClickableButton clickableButton, int id, bool status)
        {
            var item = GetById(id);

            if (item == null)
            {
                return;
            }
            
            if (item.State.On == status) return;
            
            var req = new RestRequest($"/lights/{id}/state");
            req.AddJsonBody(JsonConvert.SerializeObject(new StateToggle
            {
                On = status
            }));
            
            var res = _restClient.Put(req).Content;
            
            item.State.On = !item.State.On;
            
            _novationController.IntegrationManager.FireColorControl(this);
            
            Log.Debug($"Toggle {id}: {res}");
        }

        private void SetColor(ClickableButton clickableButton, int id, int r, int g, int b, int brightness)
        {
            var item = GetById(id);

            if (item == null)
            {
                return;
            }

            var philipsColor = RgbToXy(r, g, b);
            
            var colorToggle = new ColorToggle
            {
                Brightness = brightness,
                XY = new List<double>
                {
                    philipsColor.X,
                    philipsColor.Y
                }
            };

            var req = new RestRequest($"/lights/{id}/state");
            req.AddJsonBody(JsonConvert.SerializeObject(colorToggle));
            
            var res = _restClient.Put(req).Content;

            Log.Debug($"Color res: {res}");
        }

        private void EnableScene(string id)
        {
            var req = new RestRequest($"/groups/{id}/action");
            req.AddJsonBody(JsonConvert.SerializeObject(new StateToggle
            {
                On = true
            }));
            
            var res = _restClient.Put(req).Content;
        }
        
        private void CheckButtonColor(ClickableButton clickableButton, SmartItem smartItem)
        {
            if (smartItem.State.On)
            {
                _novationController.LaunchpadManager.SetButtonColor(clickableButton, Color.Green);
            }
            else
            {
                _novationController.LaunchpadManager.SetButtonColor(clickableButton, Color.Red);
            }
        }

        private SmartItem GetById(int id)
        {
            return !_items.Keys.Contains(id) ? null : _items[id];
        }

        private void LoadItems()
        {
            Log.Info($"Loading items...");
            
            var res = _restClient.Get(new RestRequest("/lights"));
            _items = JsonConvert.DeserializeObject<Dictionary<int, SmartItem>>(res.Content);
            
            _items.Values.ToList().ForEach(x =>
            {
                Log.Debug($"Found {x.Name}");
            });
        }

        private PhilipsLight RgbToXy(int r, int g, int b)
        {
            var red = NormalizeRGB(r);
            var green = NormalizeRGB(g);
            var blue = NormalizeRGB(b);
            
            var valX = red * 0.664511 + green * 0.154324  + blue * 0.162028;
            var valY = red * 0.283881 + green * 0.668433 + blue * 0.047685;
            var valZ = red * 0.000000 + green * 0.072310 + blue * 0.986039;

            var sum = valX + valY + valZ;

            var philipsLight = new PhilipsLight
            {
                X = 0,
                Y = 0,
            };

            if (Math.Abs(sum) > 0)
            {
                philipsLight.X = valX / sum;
                philipsLight.Y = valY / sum;
            }

            return philipsLight;
        }

        private double NormalizeRGB(double input)
        {
            var normalized = input / 255;

            if (normalized > 0.04045)
            {
                normalized = Math.Pow((normalized + 0.055) / (1.0 + 0.055), 2.4);
            }
            else
            {
                normalized /= 12.92;
            }

            return normalized;
        }

        protected override void SetupClickAction(ClickableButton clickableButton, string[] data)
        {
            clickableButton.ClickCallbacks.Add(() =>
            {
                switch (data[1])
                {
                    case "Toggle":
                    {
                        Toggle(clickableButton, int.Parse(data[2]));
                        break;
                    }
                    
                    case "On":
                    {
                        SetStatus(clickableButton, int.Parse(data[2]), true);
                        break;
                    }
                    
                    case "Off":
                    {
                        SetStatus(clickableButton, int.Parse(data[2]), false);
                        break;
                    }

                    case "Color":
                    {
                        SetColor(clickableButton, int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]), int.Parse(data[6]) * 2);
                        break;
                    }
                    
                    case "Scene":
                    {
                        EnableScene(data[2]);
                        break;
                    }
                }
            });
        }
        
        protected override void SetupLoadAction(ClickableButton clickableButton, string[] data)
        {
            clickableButton.LoadCallbacks.Add(() =>
            {
                CheckButtonColor(clickableButton, GetById(int.Parse(data[1])));
            });
        }

        protected override void SetupColorControllerAction(ClickableButton clickableButton, string[] data)
        {
            clickableButton.ColorControllerCallback = () =>
            {
                switch (data[1])
                {
                    case "Status":
                    {
                        var device = GetById(int.Parse(data[2]));
                        CheckButtonColor(clickableButton, device);
                        break;
                    }
                }
            };
        }
    }
}