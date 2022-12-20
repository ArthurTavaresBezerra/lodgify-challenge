using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;
using Xunit;

namespace VacationRental.Api.Tests
{
    [CollectionDefinition("Integration")]
    public sealed class IntegrationFixture : IDisposable, ICollectionFixture<IntegrationFixture>
    {
        private readonly TestServer _server;

        public HttpClient Client { get; }

        public IntegrationFixture()
        {
            var path = Directory.GetCurrentDirectory() ;
            _server = new TestServer(
                new WebHostBuilder().UseStartup<Startup>()
                    .UseEnvironment("test")
                    .UseConfiguration(new ConfigurationBuilder().SetBasePath(path)
                        .Build())
                );

            Client = _server.CreateClient();
        }

        public void Dispose()
        {
            Client.Dispose();
            _server.Dispose();
        }

    }
}
