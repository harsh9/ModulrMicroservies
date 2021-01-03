using System;
using System.Security.Cryptography;
using System.Text;

namespace Customers.API.Services
{
    public class NetworkClientHelper : INetworkClientHelper
    {
        public string GetAccessToken(Guid nonce, string timestamp, string key, string secret)
        {
            string signatureString = "date: " + timestamp + "\n";
            signatureString += "x-mod-nonce" + ": " + nonce;
            string encodedSignature = GenerateSignature(secret, signatureString);
            string token = CreateAccessToken(key, encodedSignature);
            return token;
        }

        private string CreateAccessToken(string key, string signatureEncoded)
        {
            return $"Signature keyId='{key}',algorithm='hmac-sha1',headers='date x-mod-nonce',signature='{signatureEncoded}'";
        }

        public string GenerateSignature(string key, string content)
        {
            var hmac = new HMACSHA1 { Key = Encoding.UTF8.GetBytes(key) };
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var signature = hmac.ComputeHash(contentBytes);
            return Convert.ToBase64String(signature);
        }
    }
}
