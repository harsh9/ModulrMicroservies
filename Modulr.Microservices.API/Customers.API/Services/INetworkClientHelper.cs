using System;

namespace Customers.API.Services
{
    public interface INetworkClientHelper
    {
        string GetAccessToken(Guid nonce, string timestamp, string key, string secret);
    }
}
