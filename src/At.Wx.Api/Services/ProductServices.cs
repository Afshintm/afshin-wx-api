using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using At.Wx.Api.Models;

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
                SortOption.Recommended => await GetRecommendedOrder(products),
                _ => throw new ArgumentOutOfRangeException(nameof(sortOption), sortOption, null)
            };
            return result;
        }

        private async Task<IEnumerable<Product>> GetRecommendedOrder(IEnumerable<Product> products)
        {
            products = products.ToList();
            var shopperHistory = await _productClient.GetShopperHistory();
            var flatten = shopperHistory.SelectMany(x => x.Products, (x, p) => p);

            var grouped = flatten.GroupBy(x => x.Name)
                .Select(x => new Product()
                {
                    Name = x.Key,
                    Quantity = (int)x.Count() * x.Sum(y => y.Quantity)
                }).OrderByDescending(i => i.Quantity);

            var joined = grouped.Join(products, g => g.Name, p => p.Name,(g,p)=>new Product()
            {
                Name = g.Name,
                Price = p.Price,
                Quantity = g.Quantity
            });
            var rest = products.Where(i => joined.All(x => x.Name != i.Name));
            var result = joined.Concat(rest);
            return result;
        }

        public async Task<decimal> CalculateTrolley(Trolley trolly)
        {
           var result =  await _productClient.PostTrolleyCalculator(trolly);
           return result;

        }
    }
}
