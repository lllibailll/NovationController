using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using Lib.Click;
using Lib.Integration.PhilipsHue.Model;
using Lib.Integration.PhilipsHue.Model.Interact;
using Lib.Manager;
using Newtonsoft.Json;

namespace Lib.Integration.PhilipsHue
{
    public class PhilipsHueIntegration : BaseIntegration
    {
        private string _baseUrl;
        private string _token;

        private WebClient _webClient;
        
        private Dictionary<int, SmartItem> _items = new Dictionary<int, SmartItem>();

        public PhilipsHueIntegration(LaunchpadManager launchpadManager, string actionPrefix, string baseUrl, string token) : 
            base(launchpadManager, actionPrefix)
        {
            _baseUrl = baseUrl;
            _token = token;
            _webClient = new WebClient();
            
            LoadItems();
        }

        private void Toggle(ClickableButton clickableButton, int id)
        {
            var item = GetById(id);

            if (item == null)
            {
                return;
            }
            
            var res = _webClient.UploadString($"{LightsEndpoint}/{id}/state", WebRequestMethods.Http.Put, JsonConvert.SerializeObject(new StateToggle
            {
                On = !item.State.On
            }));

            item.State.On = !item.State.On;
            
            CheckButtonColor(clickableButton, GetById(id));
            
            Console.WriteLine($"[Philips] Toggle {id}: {res}");
        }

        private void CheckButtonColor(ClickableButton clickableButton, SmartItem smartItem)
        {
            if (smartItem.State.On)
            {
                _launchpadManager.SetButtonColor(clickableButton, Color.Green);
            }
            else
            {
                _launchpadManager.SetButtonColor(clickableButton, Color.Red);
            }
        }

        private SmartItem GetById(int id)
        {
            return !_items.Keys.Contains(id) ? null : _items[id];
        }

        private void LoadItems()
        {
            Console.WriteLine($"[Philips] Loading items...");
            
            var res = _webClient.DownloadString(LightsEndpoint);
            _items = JsonConvert.DeserializeObject<Dictionary<int, SmartItem>>(res);
            
            _items.Values.ToList().ForEach(x =>
            {
                Console.WriteLine($"[Philips] Found {x.Name}");
            });
        }

        private string BaseEndpoint => $"{_baseUrl}/{_token}";
        private string LightsEndpoint => $"{BaseEndpoint}/lights";
        
        protected override void SetupClickAction(ClickableButton clickableButton, string[] data)
        {
            clickableButton.ClickCallback = () =>
            {
                Toggle(clickableButton, int.Parse(data[1]));
            };
        }
        
        protected override void SetupLoadAction(ClickableButton clickableButton, string[] data)
        {
            clickableButton.LoadCallback = () =>
            {
                CheckButtonColor(clickableButton, GetById(int.Parse(data[1])));
            };
        }
    }
}