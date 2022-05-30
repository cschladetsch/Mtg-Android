using System.IO;
using System.Threading.Tasks;

using UnityEngine;

using Flurl;
using Flurl.Http;
using Newtonsoft.Json;

namespace App.Util {
    internal class CostConverter {
        private const string apiKeyName = "cost-converter.api";
        private const string apiHost = "https://free.currconv.com";
        private string apiKey;

        private class CurrencyConversion {
            public string Currency;
            public float Conversion;
        }

        public CostConverter() {
            apiKey = File.ReadAllText(Path.Combine(UnityEngine.Application.persistentDataPath, apiKeyName));
        }

        public async Task<float> UsdToAud(float usd) {
            string request = $"{apiHost}/api/v7/convert?q=USD_AUD&compact=ultra&apiKey={apiKey}";
            var response = await request.GetJsonAsync<CurrencyConversion>();
            return response.Conversion;
        }
    }
}
