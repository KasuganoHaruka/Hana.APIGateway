using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIGateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var providerKey = "UserGatewayKey";
            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(providerKey, option =>
                {
                    option.Authority = "http://localhost:15201";
                    option.ApiName = "api1";
                    option.RequireHttpsMetadata = false;
                    option.SupportedTokens = SupportedTokens.Both;
                });

            services.AddOcelot()
                    .AddConsul()
                    .AddCacheManager(x => 
                    {
                        x.WithDictionaryHandle();
                    });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOcelot();
        }
    }
}
