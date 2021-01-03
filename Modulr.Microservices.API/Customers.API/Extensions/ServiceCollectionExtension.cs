using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Customers.API.Configuration;
using Customers.API.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Customers.API.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddCommonServices(this IServiceCollection services, MasterConfig masterConfig)
        {
            services.AddSingleton<MasterConfig>(masterConfig);

            services.AddScoped<IHttpClientWebService, HttpClientWebService>();
            services.AddScoped<INetworkClientHelper, NetworkClientHelper>();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(masterConfig.SwaggerConfig.ApiVersion, new OpenApiInfo { Title = masterConfig.SwaggerConfig.Title, Version = masterConfig.SwaggerConfig.ApiVersion, Description = masterConfig.SwaggerConfig.Description });
            });
            
        }
    }
}
