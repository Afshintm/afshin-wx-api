using System.Collections.Generic;
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
    public class TrolleyTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public const string TotalTrolleyUrl = "api/trolleyTotal";
        public TrolleyTests(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;
        private readonly string _trolleyJson = @"
                    {
                      ""products"": [
                        {
                          ""name"": ""P1"",
                          ""price"": 50
                        },
                        {
                          ""name"": ""P2"",
                          ""price"": 20
                        },
                        {
                          ""name"": ""P3"",
                          ""price"": 40
                        }
                      ],
                      ""specials"": [
                        {
                          ""quantities"": [
                            {
                              ""name"": ""P1"",
                              ""quantity"": 2
                            },
                            {
                              ""name"": ""P3"",
                              ""quantity"": 2
                            },
                            {
                              ""name"": ""P2"",
                              ""quantity"": 0
                            }
                          ],
                          ""total"": 120
                        }
                      ],
                      ""quantities"": [
                        {
                          ""name"": ""P2"",
                          ""quantity"": 2
                        },
                        {
                          ""name"": ""P1"",
                          ""quantity"": 4
                        },
                        {
                          ""name"": ""P3"",
                          ""quantity"": 2
                        },
                        {
                          ""name"": ""P4"",
                          ""quantity"": 2
                        }
                      ]
                    }
        ";
        [Fact]
        public async Task Given_A_Call_To_TrolleyTotalSort_Endpoint_With_SortOption_Recommended_It_Should_Return_SortedProductList()
        {
            using var testContext = new TestContext(_outputHelper).Start();
            var trolley = JsonConvert.DeserializeObject<Trolley>(_trolleyJson);
            var response = await testContext.Client.PostAsJsonAsync(TotalTrolleyUrl,trolley);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var actualResponse = await response.Content.ReadAsAsync<decimal>();
            actualResponse.Should().Be(260.0m);
        }

    }
}
