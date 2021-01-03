using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Customers.API.Configuration;
using Customers.API.Models;
using Newtonsoft.Json;

namespace Customers.API.Services
{
    public class HttpClientWebService : IHttpClientWebService
    {
        private readonly INetworkClientHelper _clientHelper;
        private readonly MasterConfig _masterConfig;
        public HttpClientWebService(INetworkClientHelper clientHelper, MasterConfig masterConfig)
        {
            _clientHelper = clientHelper;
            _masterConfig = masterConfig;
        }

        public async Task<HttpResponseMessage> SendHttpRequest(HttpMethod httpMethod, string url, string serializedData = null)
        {
            Guid nonce = Guid.NewGuid();
            string timestamp = DateTime.UtcNow.Date.ToUniversalTime().ToString("ddd, dd MMM yyyy HH:mm:ss");
            var authToken = _clientHelper.GetAccessToken(nonce, timestamp, _masterConfig.SandboxConfig.ClientId, _masterConfig.SandboxConfig.ClientSecret);
            var client = new HttpClient
            {
                BaseAddress = new Uri(url),
                Timeout = new TimeSpan(0, 0, 0, 60 * 5)
                //,DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Signature", authToken) }
            };

            var request = new HttpRequestMessage { Method = httpMethod };
            const string mediaType = "application/json";
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            request.Headers.Add("Date", timestamp + "GMT");
            request.Headers.Add("Authorization", authToken);
            request.Headers.Add("x-mod-nonce", nonce.ToString());

            if (request.Method.Equals(HttpMethod.Post) && !string.IsNullOrEmpty(serializedData))
            {
                request.Content = new StringContent(serializedData, Encoding.UTF8, mediaType);
            }

            return await client.SendAsync(request);
        }


        #region Private Methods

        public string GetErrorMessageFromResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return null;
            }

            return response.StatusCode == System.Net.HttpStatusCode.BadRequest
                ? UnwrapBadRequestMessage(response)
                : response.Content.ReadAsStringAsync().Result;
        }

        private string UnwrapBadRequestMessage(HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync().Result;

            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            try
            {
                var badRequestMessage = JsonConvert.DeserializeObject<BadRequestMessage>(content);

                return badRequestMessage.ErrorMessage;
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        #endregion
    }
}
