using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Accounts.API.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Accounts.API.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateAccount([FromBody] Account account)
        {
            try
            {
                string response = await SendHttpRequest(HttpMethod.Post, "https://api-sandbox.modulrfinance.com/api-sandbox-token/customers", account);
                //var item = new Account
                //{
                //    CatalogBrandId = product.CatalogBrandId,
                //    CatalogTypeId = product.CatalogTypeId,
                //    Description = product.Description,
                //    Name = product.Name,
                //    PictureFileName = product.PictureFileName,
                //    Price = product.Price
                //};

                //_catalogContext.CatalogItems.Add(item);

                //await _catalogContext.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateAccount), new { id = response }, null);

            }
            catch (Exception exception)
            {
                _logger.LogError("Error in AccountController for CreateAccount", exception);
                return null;
            }
        }

        private async Task<string> SendHttpRequest(HttpMethod httpMethod, string url, Account data)
        {
            string returnValue = null;
            var client = new HttpClient
            {
                BaseAddress = new Uri(url),
                Timeout = new TimeSpan(0, 0, 0, 60 * 5)
            };
            var request = new HttpRequestMessage { Method = httpMethod };
            const string mediaType = "application/json";
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(mediaType));

            if (request.Method.Equals(HttpMethod.Put))
            {
                var serializedData = JsonConvert.SerializeObject(data);
                request.Content = new StringContent(serializedData, Encoding.UTF8, mediaType);
            }

            var response = await client.SendAsync(request);
            if (response != null)
            {
                returnValue = await response.Content.ReadAsStringAsync();
            }

            return returnValue;
        }
    }
}
