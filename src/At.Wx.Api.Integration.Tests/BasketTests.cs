using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using At.Wx.Api.Infrastructure;
using At.Wx.Api.Integration.Tests.Infrastructure;
using At.Wx.Api.Models;
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
    }
}
