using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace At.Wx.Api.Infrastructure
{
    public class TokenEnricher: DelegatingHandler
    {
        private readonly ApiConfig _apiConfig;

        public TokenEnricher(ApiConfig apiConfig)
        {
            _apiConfig = apiConfig;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var query = request.RequestUri.ToString();
            var uri = new Uri($"{query}?token={_apiConfig.Token}");
            request.RequestUri = uri;
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }
    }
}
