using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace At.Wx.Api.Integration.Tests.Infrastructure
{
    public class ResourceApiHost: ApiHostBase
    {
        public string BaseUrl { get; private set; }
        public override void Start()
        {
            base.Start();
            BaseUrl = $"http://localhost:{_port}/"; //DevSkim: ignore DS137138 
        }

        public void SetupGetApiResourceEndpoint(string path, HttpStatusCode status, object response)
        {
            BuilderConfigurations.Add(builder =>
            {
                builder.MapWhen(
                    context => context.Request.Path.StartsWithSegments($"/api/resource/{path}")
                               && context.Request.Method == HttpMethods.Get,
                    app => app.Run(async ctx =>
                        await HandleRequest(ctx, status, response)));
            });
        }

        private async Task HandleRequest(HttpContext context, HttpStatusCode status, object response = null)
        {
            context.Response.StatusCode = (int)status;
            if (response != null)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
