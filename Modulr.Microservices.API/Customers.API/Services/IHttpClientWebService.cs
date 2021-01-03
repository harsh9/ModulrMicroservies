using System.Net.Http;
using System.Threading.Tasks;

namespace Customers.API.Services
{
    public interface IHttpClientWebService
    {
        Task<HttpResponseMessage> SendHttpRequest(HttpMethod httpMethod, string url, string serializedData = null);
        string GetErrorMessageFromResponse(HttpResponseMessage response);
    }
}
