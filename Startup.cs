using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

namespace ShoppingCart
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>()
                .AddTransientHttpErrorPolicy(p =>
                    p.WaitAndRetryAsync(3,
                        attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt))));

            services.Scan(selector =>
                selector
                    .FromAssemblyOf<Startup>()
                    .AddClasses()
                    .AsMatchingInterface()
                    .WithScopedLifetime());

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
