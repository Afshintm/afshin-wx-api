﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using At.Wx.Api.Integration.Tests.Infrastructure;
using At.Wx.Api.Models;
using At.Wx.Api.Services;
using FluentAssertions;
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
            new Product {Name = "Product 4", Price = 100.5m, Quantity = 9},
            new Product {Name = "Product 6", Price = 102.5m, Quantity = 7},
            new Product {Name = "Product 3", Price = 10.5m, Quantity = 2},
            new Product {Name = "Product 5", Price = 99.3m, Quantity = 1},
            new Product {Name = "Product 1", Price = 12.15m, Quantity = 3},
            new Product {Name = "Product 2", Price = 101.7m, Quantity = 3},
        };
        [Fact]
        public async Task Given_A_Call_To_Sort_Endpoint_It_Should_Return_200_Ok()
        {
            using var testContext = new TestContext(
                _outputHelper,
                c =>
                {
                    c.SetupGetProductsEndpoint(HttpStatusCode.OK, _productApiStub);

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
                c => { c.SetupGetProductsEndpoint(HttpStatusCode.OK, _productApiStub); }).Start();
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
            };
        }

        [Fact]
        public async Task Given_A_Call_To_Sort_Endpoint_With_SortOption_Low_It_Should_Return_SortedProductList()
        {
            using var testContext = new TestContext(
                _outputHelper,
                c =>
                {
                    c.SetupGetProductsEndpoint(HttpStatusCode.OK, _productApiStub);

                }).Start();
            var response = await testContext.Client.GetAsync(SortUrl + $"?sortOption=Low");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualResponse = await response.Content.ReadAsAsync<IEnumerable<Product>>();
            actualResponse.Should().NotBeNull();
            actualResponse.Should().BeEquivalentTo(_productApiStub.OrderBy(x => x.Price), options => options.WithStrictOrdering());
        }

    }
}