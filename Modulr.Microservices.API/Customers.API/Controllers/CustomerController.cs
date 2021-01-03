using Customers.API.Models;
using Customers.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Customers.API.Controllers
{
    /// <summary>
    /// Class CustomerController.
    /// Implements the <see>
    ///     <cref>Microsoft.AspNetCore.Mvc.ControllerBase</cref>
    /// </see>
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IHttpClientWebService _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="httpClient">The HTTP client.</param>
        public CustomerController(ILogger<CustomerController> logger, IHttpClientWebService httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Gets the customer.
        /// </summary>
        /// <returns>Customer.</returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="System.Exception"></exception>
        [HttpGet]
        public async Task<Customer> GetCustomer()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.SendHttpRequest(HttpMethod.Get, "https://api-sandbox.modulrfinance.com/api-sandbox/customers");

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = _httpClient.GetErrorMessageFromResponse(response);

                    throw response.StatusCode == HttpStatusCode.BadRequest
                        ? new BadRequestException(errorMessage)
                        : new Exception(errorMessage);
                }

                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Customer>(content);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error in CustomerController for GetCustomer", exception);
                return null;
            }
        }

        /// <summary>
        /// Creates the customer.
        /// </summary>
        /// <param name="customer">The customer.</param>
        /// <returns>Customer.</returns>
        /// <exception cref="BadRequestException"></exception>
        /// <exception cref="System.Exception"></exception>
        [HttpPost]
        public async Task<Customer> CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                string data = JsonConvert.SerializeObject(customer);
                HttpResponseMessage response = await _httpClient.SendHttpRequest(HttpMethod.Post, "https://api-sandbox.modulrfinance.com/api-sandbox-token/customers/cid/accounts", data);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = _httpClient.GetErrorMessageFromResponse(response);

                    throw response.StatusCode == HttpStatusCode.BadRequest
                        ? new BadRequestException(errorMessage)
                        : new Exception(errorMessage);
                }
               
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Customer>(content);
            }
            catch (Exception exception)
            {
                _logger.LogError("Error in CustomerController for CreateCustomer", exception);
                return null;
            }
        }

        /// <summary>
        /// Creates the account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>ActionResult.</returns>
        [HttpPost]
        [Route("{id}/account")]

        public async Task<ActionResult> CreateAccount([FromBody] Account account)
        {
            try
            {
                string data = JsonConvert.SerializeObject(account);
                HttpResponseMessage response = await _httpClient.SendHttpRequest(HttpMethod.Post, "https://api-sandbox.modulrfinance.com/api-sandbox-token/customers/cid/accounts", data);
                return CreatedAtAction(nameof(CreateAccount), new { id = response }, null);

            }
            catch (Exception exception)
            {
                _logger.LogError("Error in AccountController for CreateAccount", exception);
                return null;
            }
        }
    }
}
