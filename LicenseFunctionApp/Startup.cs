using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(LicenseFunctionApp.Startup))]

namespace LicenseFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton(serviceProvider =>
            {
                var bla = Environment.GetEnvironmentVariable("CosmosDbConnectionString");
                var client = new CosmosClient(Environment.GetEnvironmentVariable("CosmosDbConnectionString"));
                var container = client.GetDatabase("LicenseDB").GetContainer("Licenses");
                return container;
            });

        }
    }
}