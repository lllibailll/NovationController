using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Lib.Integration.PhilipsHue.Model;
using Lib.Integration.PhilipsHue.Model.Interact;
using Newtonsoft.Json;

namespace Lib.Integration.PhilipsHue
{
    public class PhilipsHueIntegration
    {
        private string _baseUrl;
        private string _token;

        private WebClient _webClient;
        
        private Dictionary<int, SmartItem> _items = new Dictionary<int, SmartItem>();

        public PhilipsHueIntegration(string baseUrl, string token)
        {
            _baseUrl = baseUrl;
            _token = token;
            _webClient = new WebClient();
            
            LoadItems();
        }

        public void Toggle(int id, bool status)
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
            
            Console.WriteLine($"[Philips] Toggle {id}: {res}");
        }

        public SmartItem GetById(int id)
        {
            if (!_items.Keys.Contains(id))
            {
                return null;
            }

            return _items[id];
        }

        public void LoadItems()
        {
            Console.WriteLine($"[Philips] Loading items...");
            
            var res = _webClient.DownloadString(LightsEndpoint);
            _items = JsonConvert.DeserializeObject<Dictionary<int, SmartItem>>(res);
            
            _items.Values.ToList().ForEach(x =>
            {
                Console.WriteLine($"[Philips] Found {x.Name}");
            });
        }

        public string BaseEndpoint => $"{_baseUrl}/{_token}";
        public string LightsEndpoint => $"{BaseEndpoint}/lights";
    }
}