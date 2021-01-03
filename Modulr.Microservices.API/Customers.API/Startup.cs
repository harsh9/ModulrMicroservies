using Customers.API.Configuration;
using Customers.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Customers.API
{
    public class Startup
    {
        private readonly MasterConfig _masterConfig = new MasterConfig();
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _masterConfig.PopulateConfiguration(Configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCommonServices(_masterConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();

            app.UseSwaggerUI(x => {
                x.SwaggerEndpoint($"/swagger/{_masterConfig.SwaggerConfig.ApiVersion}/swagger.json", $"{_masterConfig.SwaggerConfig.Title} {_masterConfig.SwaggerConfig.ApiVersion}");
            });
        }
    }
}
