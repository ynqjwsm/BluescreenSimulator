using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BluescreenSimulator
{
    class TranslationService
    {
        private const string SubKey = "";
        private const string Endpoint = "https://api.cognitive.microsofttranslator.com/";

        public Task<string> Translate(string From, string To)
        {
            To = $"/translate?api-version=3.0&to={To}";
            return TranslationRequest(To, From);
        }

        public async Task<string> TranslationRequest(string route, string text)
        {
            object[] body = {new {Text = text}};
            var requestBody = JsonConvert.SerializeObject(body);
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(Endpoint + route);
                request.Content = new StringContent(requestBody,Encoding.UTF8,"application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", SubKey);

                var response = await client.SendAsync(request).ConfigureAwait(false);
                var results = await response.Content.ReadAsStringAsync();
                var readableOutput =
                    JsonConvert.DeserializeObject<JsonParse.TranslationResult[]>(results);
                foreach (var o in readableOutput)
                {
                    foreach (var t in o.Translations)
                    {
                        return t.Text; 
                    }
                }

            }

            return null;
        }
    }

    class JsonParse
    {
        public class TranslationResult
        {
            public Translation[] Translations { get; set; }
        }

        public class Translation
        {
            public string Text { get; set; }
            public string To { get; set; }
        }
    }
}
