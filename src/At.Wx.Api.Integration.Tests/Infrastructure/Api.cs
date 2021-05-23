using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace At.Wx.Api.Integration.Tests.Infrastructure
{
    public class Api : IDisposable
    {
        private readonly string _testName;
        public HttpClient Client;
        public int ApiPort { get; }
        private readonly CancellationTokenSource _apiCancellationTokenSource;

        public Api(string testName)
        {
            _testName = testName;
            ApiPort = PortHelper.GetNextAvailablePort();
            var args = new[] { "--urls", $"http://0.0.0.0:{ApiPort}", "--environment", "Development" }; //DevSkim: ignore DS137138 
            _apiCancellationTokenSource = new CancellationTokenSource();
            Program.CreateHostBuilder(args)
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    configuration.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        ["Logging:LogLevel:Default"] = "Information",
                        ["Logging:LogLevel:Microsoft"] = "Warning",
                        ["Logging:LogLevel:Microsoft.Hosting.Lifetime"] = "Information"
                    });
                })
                .Build()
                .RunAsync(_apiCancellationTokenSource.Token);

            Client = new HttpClient { BaseAddress = new Uri($"http://localhost:{ApiPort}") }; //DevSkim: ignore DS137138 
            WaitForApiAsync();

        }

        private void WaitForApiAsync()
        {
            for (var _ = 0; _ < 10; _++)
            {
                try
                {
                    Thread.Sleep(100);
                    var response = Client.GetAsync("/api/user").Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{_testName}: Waiting for API - {ex.Message}");
                }
            }
            throw new Exception($"{_testName}: Failed to connect to API.");
        }

        public void Dispose()
        {
            _apiCancellationTokenSource.Cancel();
        }
    }
}
