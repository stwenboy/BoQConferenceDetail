using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BoQConferenceDetail.ConferenceClient
{
    public class Client : IConferenceClient
    {
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _clientFactory;

        public Client(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _config = configuration;
            _clientFactory = clientFactory;
        }

        private string GetAuthKey()
        {
            return _config.GetSection("Settings").GetSection("AuthKey").Value; // ideally this is in an IOptions interface so can be tested also
        }
        
        private string GetFullUrl(string endpoint)
        {
            return $"{_config.GetSection("Settings").GetSection("BaseUrl").Value}{endpoint}"; // again better in an IOptions interface
        }

        //private async Task<string> GetConferenceData()

        private string GetFullUrl(string endpoint, Dictionary<string, string> parameters)
        {
            return QueryHelpers.AddQueryString(GetFullUrl(endpoint), parameters);
        }

        private HttpRequestMessage PrepareGetRequestMessage(string Url)
        {
           
            var request = new HttpRequestMessage(HttpMethod.Get, Url);

            request.Headers.Add("User-Agent", "ST-Test");
            request.Headers.Add("Ocp-Apim-Subscription-Key", GetAuthKey());
            request.Headers.Add("Api-version", "v1");

            return request;
        }

        private async Task<IList<PocoReturnData>> RunRequest(HttpRequestMessage requestMessage)
        {
            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(requestMessage);
            CheckReponse(response);

            var json = JObject.Parse(await response.Content.ReadAsStringAsync());

            var items = json.SelectToken("$..items");
            var result = JsonConvert.DeserializeObject<IList<PocoReturnData>>(items.ToString());

            return result;
        }

        public async Task<IList<PocoReturnData>> GetSessionTopicsAsync(string sessionId)
        {
            var request = PrepareGetRequestMessage(GetFullUrl($"session/{sessionId}/topics"));

            var sessions = await RunRequest(request);

            return sessions;
        }

        public async Task<IList<PocoReturnData>> GetSpeakersAsync(string speakerName)
        {
            var queryParams = new Dictionary<string, string>
                            {
                                { "speakername", speakerName }
                            };
            var request = PrepareGetRequestMessage(GetFullUrl("speakers", queryParams));
            
            var speakers = await RunRequest(request);

            return speakers;
        }

        private void CheckReponse(HttpResponseMessage res)
        {
            if (!res.IsSuccessStatusCode)
            {
                throw new InvalidOperationException();
            }
        }

        public async Task<IList<PocoReturnData>> GetSpeakerSessionsAsync(string speakerId)
        {
            var request = PrepareGetRequestMessage(GetFullUrl($"speaker/{speakerId}/sessions"));

            var sessions = await RunRequest(request);

            return sessions;
        }
    }
}
