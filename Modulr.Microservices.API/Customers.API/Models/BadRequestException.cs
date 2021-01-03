using System;

namespace Customers.API.Models
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string errorMessage) : base(errorMessage)
        {

        }
    }
}
