using Customers.API.Configuration;
using Microsoft.Extensions.Configuration;

namespace Customers.API.Extensions
{
    public static class MasterConfigExtension
    {
        public static void PopulateConfiguration(this MasterConfig masterConfig, IConfiguration configuration)
        {
            masterConfig.SandboxConfig = configuration.GetSection("Sandbox").Get<Sandbox>();
            masterConfig.SwaggerConfig = configuration.GetSection("Swagger").Get<Swagger>();
        }
    }
}
