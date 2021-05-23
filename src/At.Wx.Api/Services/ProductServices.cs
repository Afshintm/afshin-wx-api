using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using At.Wx.Api.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace At.Wx.Api.Services
{
    public enum SortOption
    {
        Low,
        High,
        Ascending,
        Descending,
        Recommended
    }

    public class ProductServices
    {
        private readonly ProductClient _productClient;

        public ProductServices(ProductClient productClient)
        {
            _productClient = productClient;
        }

        public async Task<IEnumerable<Product>> GetProduct(SortOption sortOption = SortOption.Low)
        {
            var products = await _productClient.GetProducts();
            var result = sortOption switch
            {
                SortOption.Low => products.OrderBy(x=>x.Price),
                SortOption.High => products.OrderByDescending(x => x.Price),
                SortOption.Ascending => products.OrderBy(x => x.Name),
                SortOption.Descending => products.OrderByDescending(x => x.Name),
                //SortOption.Recommended => await GetRecommendedOrder(),
                SortOption.Recommended => (await GetRecommendedOrder())?.Select(i=>products.FirstOrDefault(x=>x.Name==i.Name)),
                _ => throw new ArgumentOutOfRangeException(nameof(sortOption), sortOption, null)
            };
            return result;
        }

        private async Task<IEnumerable<Product>> GetRecommendedOrder()
        {
            var result = await _productClient.GetShopperHistory();
            var products = result.SelectMany(x => x.Products, (x, p) => p)
                .GroupBy(x => x.Name)
                .Select(x=> new Product
                {
                    Name = x.Key,
                    Quantity = x.Sum(y=>y.Quantity),
                }).OrderByDescending(i=>i.Quantity).AsEnumerable();
            return products;
        }
    }
}
