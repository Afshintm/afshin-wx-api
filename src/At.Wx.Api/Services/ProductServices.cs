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

        public async Task<IEnumerable<Product>> GetProduct(SortOption sortOption= SortOption.Low)
        {
            var result = await _productClient.GetProducts();
            var result1 = sortOption switch
            {
                SortOption.Low => result.OrderBy(x=>x.Price),
                SortOption.High => result.OrderByDescending(x => x.Price),
                SortOption.Ascending => result.OrderBy(x => x.Name),
                SortOption.Descending => result.OrderByDescending(x => x.Name),
                SortOption.Recommended => throw new NotImplementedException(nameof(sortOption)),
                _ => throw new ArgumentOutOfRangeException(nameof(sortOption), sortOption, null)
            };
            return result1;
        }
    }
}
