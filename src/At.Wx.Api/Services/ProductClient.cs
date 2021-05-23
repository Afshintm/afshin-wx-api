using System;
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
        public async Task<IEnumerable<Basket>> GetShopperHistory()
        {
            var response = await _httpClient.GetAsync($"api/resource/shopperHistory");
            if (response.StatusCode == HttpStatusCode.NotFound)
                throw new HttpStatusCodeException(response.StatusCode,
                    $"Product endpoint returns{response.StatusCode}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsAsync<IEnumerable<Basket>>();
            return result;
        }

        public async Task<decimal> PostTrolleyCalculator(Trolley trolley)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/resource/trolleyCalculator", trolley);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new HttpStatusCodeException(response.StatusCode,
                    $"Product endpoint returns{response.StatusCode}");

            response.EnsureSuccessStatusCode();

            var str = await response.Content.ReadAsAsync<string>();
            decimal.TryParse(str, out var result);
            return result;
        }
    }
}