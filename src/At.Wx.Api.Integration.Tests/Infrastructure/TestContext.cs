using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net.Http;
using Xunit.Abstractions;

namespace At.Wx.Api.Integration.Tests.Infrastructure
{
    public class TestContext : IDisposable
    {
        private Api _api;
        private readonly string _testName;

        public static ConcurrentDictionary<string, ITestOutputHelper> OutputHelpers { get; }
        public ResourceApiHost ResourceApiHost { get; set; } 
        static TestContext()
        {
            OutputHelpers = new ConcurrentDictionary<string, ITestOutputHelper>();
        }

        public TestContext(ITestOutputHelper outputHelper, Action<ResourceApiHost> resourceApiConfigurator = null)
        {
            ResourceApiHost = new ResourceApiHost();
            _testName = GetTestName();
            OutputHelpers[_testName] = outputHelper;
            resourceApiConfigurator?.Invoke(ResourceApiHost);
        }
        public TestContext Start()
        {
            ResourceApiHost.Start();
            _api = new Api(_testName,ResourceApiHost);
            Client = _api.Client;
            return this;
        }

        public HttpClient Client { get; set; }

        public void Dispose()
        {
            _api?.Dispose();
            ResourceApiHost.Dispose();
        }
        private static string GetTestName()
        {
            var stackTrace = new StackTrace();
            var testName = stackTrace.GetFrame(5).GetMethod().Name;
            if (testName == "InvokeMethod")
            {
                testName = stackTrace.GetFrame(4).GetMethod().Name;
            }

            return testName;
        }
    }
}
