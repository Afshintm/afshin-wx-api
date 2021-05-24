using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using At.Wx.Api.Integration.Tests.Infrastructure;
using At.Wx.Api.Models;
using FluentAssertions;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace At.Wx.Api.Integration.Tests
{
    public class BasketTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public const string SortUrl = "api/sort";
        public BasketTests(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

        private readonly List<Product> _productApiStub = new List<Product>
        {
            new Product {Name = "Test Product F", Price = 100.5m},
            new Product {Name = "Test Product C", Price = 102.5m},
            new Product {Name = "Test Product A", Price = 10.5m},
            new Product {Name = "Test Product B", Price = 99.3m},
            new Product {Name = "Test Product D", Price = 12.15m}
        };

        private readonly string _shopperHistory = @"[
              {
                ""customerId"": 123,
                ""products"": [
                  {
                    ""name"": ""Test Product A"",
                    ""price"": 99.99,
                    ""quantity"": 3
                  },
                  {
                    ""name"": ""Test Product B"",
                    ""price"": 101.99,
                    ""quantity"": 1
                  },
                  {
                    ""name"": ""Test Product F"",
                    ""price"": 999999999999,
                    ""quantity"": 1
                  }
                ]
              },
              {
                ""customerId"": 23,
                ""products"": [
                  {
                    ""name"": ""Test Product A"",
                    ""price"": 99.99,
                    ""quantity"": 2
                  },
                  {
                    ""name"": ""Test Product B"",
                    ""price"": 101.99,
                    ""quantity"": 3
                  },
                  {
                    ""name"": ""Test Product F"",
                    ""price"": 999999999999,
                    ""quantity"": 1
                  }
                ]
              },
              {
                ""customerId"": 23,
                ""products"": [
                  {
                    ""name"": ""Test Product C"",
                    ""price"": 10.99,
                    ""quantity"": 2
                  },
                  {
                    ""name"": ""Test Product F"",
                    ""price"": 999999999999,
                    ""quantity"": 2
                  }
                ]
              },
              {
                ""customerId"": 23,
                ""products"": [
                  {
                    ""name"": ""Test Product A"",
                    ""price"": 99.99,
                    ""quantity"": 1
                  },
                  {
                    ""name"": ""Test Product B"",
                    ""price"": 101.99,
                    ""quantity"": 1
                  },
                  {
                    ""name"": ""Test Product C"",
                    ""price"": 10.99,
                    ""quantity"": 1
                  }
                ]
              }
            ]";


        [Fact]
        public async Task Given_A_Call_To_Sort_Endpoint_It_Should_Return_200_Ok()
        {
            using var testContext = new TestContext(
                _outputHelper,
                c =>
                {
                    c.SetupGetApiResourceEndpoint("products", HttpStatusCode.OK, _productApiStub);

                }).Start();
            var response = await testContext.Client.GetAsync(SortUrl);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualResponse = await response.Content.ReadAsAsync<IEnumerable<Product>>();
            actualResponse.Should().NotBeNull();
        }

        [Theory]
        [InlineData(SortOption.Low)]
        [InlineData(SortOption.High)]
        [InlineData(SortOption.Ascending)]
        [InlineData(SortOption.Descending)]
        public async Task Given_A_Call_To_Sort_Endpoint_With_SortOption_It_Should_Return_SortedProductList_In_Right_Order(SortOption sortOption)
        {
            using var testContext = new TestContext(_outputHelper,
                c =>
                    c.SetupGetApiResourceEndpoint("products", HttpStatusCode.OK, _productApiStub)
                ).Start();
            var response = await testContext.Client.GetAsync(SortUrl + $"?sortOption={sortOption}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualResponse = await response.Content.ReadAsAsync<IEnumerable<Product>>();
            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(GetOrderedProducts(_productApiStub,sortOption), options => options.WithStrictOrdering());
        }

        private IEnumerable<Product> GetOrderedProducts(IEnumerable<Product> products, SortOption sortOption)
        {
            return sortOption switch
            {
                SortOption.Low => products.OrderBy(x => x.Price),
                SortOption.High => products.OrderByDescending(x => x.Price),
                SortOption.Ascending => products.OrderBy(x => x.Name),
                SortOption.Descending => products.OrderByDescending(x => x.Name),
                SortOption.Recommended => products.OrderByDescending(x=>x.Quantity)
            };
        }

        [Fact]
        public async Task Given_A_Call_To_Sort_Endpoint_With_SortOption_Low_It_Should_Return_SortedProductList()
        {
            using var testContext = new TestContext(
                _outputHelper,
                c =>
                {
                    c.SetupGetApiResourceEndpoint("products",HttpStatusCode.OK, _productApiStub);

                }).Start();
            var response = await testContext.Client.GetAsync(SortUrl + $"?sortOption=Low");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualResponse = await response.Content.ReadAsAsync<IEnumerable<Product>>();
            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(_productApiStub.OrderBy(x => x.Price), options => options.WithStrictOrdering());
        }

        [Fact]
        public async Task Given_A_Call_To_Sort_Endpoint_With_SortOption_Recommended_It_Should_Return_200_OK()
        {
            using var testContext = new TestContext(_outputHelper).Start();
            var response = await testContext.Client.GetAsync(SortUrl + $"?sortOption=Recommended");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualResponse = await response.Content.ReadAsAsync<IEnumerable<Product>>();
            actualResponse.Should().NotBeNull();
        }

        [Fact]
        public async Task Given_A_Call_To_Sort_Endpoint_With_SortOption_Recommended_It_Should_Return_The_Right_Order_Successfully()
        {
            var shopperHistoryStub = JsonConvert.DeserializeObject<List<Basket>>(_shopperHistory);
            using var testContext = new TestContext(_outputHelper,
                c =>
                {
                    c.SetupGetApiResourceEndpoint("products", HttpStatusCode.OK, _productApiStub);
                    c.SetupGetApiResourceEndpoint("shopperHistory", HttpStatusCode.OK, shopperHistoryStub);

                }).Start();
            var response = await testContext.Client.GetAsync(SortUrl + $"?sortOption=Recommended");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualResponse = await response.Content.ReadAsAsync<IEnumerable<Product>>();
            actualResponse.Should().NotBeNull();
        }

        [Fact]
        public async Task Given_A_Call_To_Sort_Endpoint_With_SortOption_Unknown_Should_Return_BadRequest()
        {
            using var testContext = new TestContext(_outputHelper,
                c=> c.SetupGetApiResourceEndpoint("products", HttpStatusCode.OK, _productApiStub)).Start();
            var response = await testContext.Client.GetAsync(SortUrl + $"?sortOption=Unknown");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        [Fact]
        public async Task Given_Resource_Product_EndPoint_Returns_NotFound_A_Call_To_Sort_Endpoint_Returns_NotFound()
        {
            using var testContext = new TestContext(_outputHelper, c => c.SetupGetApiResourceEndpoint("products", HttpStatusCode.NotFound, null)).Start();
            var response = await testContext.Client.GetAsync(SortUrl + $"?sortOption=Low");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Given_Resource_Product_EndPoint_Returns_InternalServerError_A_Call_To_Sort_Endpoint_Returns_InternalServerError()
        {
            using var testContext = new TestContext(_outputHelper, c => c.SetupGetApiResourceEndpoint("products", HttpStatusCode.InternalServerError, null)).Start();
            var response = await testContext.Client.GetAsync(SortUrl + $"?sortOption=Low");
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

    }
}
