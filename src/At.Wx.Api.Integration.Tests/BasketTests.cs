using System.Collections.Generic;
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

        [Fact]
        public async Task Given_A_Call_To_Sort_Endpoint_It_Should_Return_200_Ok()
        {
            using var testContext = new TestContext(_outputHelper).Start();
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
        public async Task Given_A_Call_To_Sort_Endpoint_With_SortOption_Low_It_Should_Return_200_Ok(SortOption sortOptions)
        {
            using var testContext = new TestContext(_outputHelper).Start();
            var response = await testContext.Client.GetAsync(SortUrl+$"?sortOption={sortOptions}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualResponse = await response.Content.ReadAsAsync<IEnumerable<Product>>();
            actualResponse.Should().NotBeNull();
        }
    }
}
