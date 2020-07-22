using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;

namespace BluescreenSimulator
{
    class TranslationService
    {
        private static RestClient _client;
        private static Translation _translation = new Translation();

        /// <summary>
        /// Translates a string into a different language
        /// </summary>
        /// <param name="text"></param>
        /// <param name="to"></param>
        /// <returns>Translated string</returns>
        public static Task<string> Translate(string text, string to)
        {
            //for some reason this is needed otherwise response is gonna be null :/
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            _client = new RestClient($"https://translationwebapi.azurewebsites.net/api/Translation?text={text}&to={to}");
           var request = new RestRequest(Method.GET);
           var response = _client.Execute(request);
           _translation = JsonConvert.DeserializeObject<Translation>(response.Content);
           return Task.FromResult(_translation.Text);
        }

        private class Translation
        {
            public string Text { get; set; }
        }
    }
}
