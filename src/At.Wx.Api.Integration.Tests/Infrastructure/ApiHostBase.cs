using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace At.Wx.Api.Integration.Tests.Infrastructure
{
    public abstract class ApiHostBase : IDisposable
    {
        private IWebHost _host;
        protected int _port;
        protected readonly List<Action<IApplicationBuilder>> BuilderConfigurations;

        protected ApiHostBase()
        {
            BuilderConfigurations = new List<Action<IApplicationBuilder>>();
        }
        public virtual void Start()
        {
            var port = PortHelper.GetNextAvailablePort();
            Start(port);
        }

        public virtual void Start(int port)
        {
            _port = port;
            _host = new WebHostBuilder()
                .Configure(builder =>
                {
                    BuilderConfigurations.ForEach(configure => configure(builder));
                })
                .UseKestrel(options => options.ListenAnyIP(_port))
                .ConfigureLogging(logging => logging.AddConsole())
                .Build();

            _host.Start();
        }

        public void Dispose()
        {
            _host?.Dispose();
        }
    }
}
