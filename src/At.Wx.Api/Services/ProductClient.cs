using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using At.Wx.Api.ExceptionHandling;
using At.Wx.Api.Models;

namespace At.Wx.Api.Services
{
    public class ProductClient
    {
        private readonly HttpClient _httpClient;
        public ProductClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var response = await  _httpClient.GetAsync($"api/resource/products");
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new HttpStatusCodeException(response.StatusCode,
                    $"Product endpoint returns{response.StatusCode}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<IEnumerable<Product>>();
            return result;
        }
    }
}
