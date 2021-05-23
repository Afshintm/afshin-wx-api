using System.Collections.Generic;
using System.Threading.Tasks;
using At.Wx.Api.Models;

namespace At.Wx.Api.Services
{
    public enum SortOptions
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

        public async Task<IEnumerable<Product>> GetProduct(SortOptions sortOptions= SortOptions.Low)
        {
            var result = await _productClient.GetProducts();
            return result;
        }
    }
}
