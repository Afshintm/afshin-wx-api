using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using At.Wx.Api.Infrastructure;
using At.Wx.Api.Integration.Tests.Infrastructure;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace At.Wx.Api.Integration.Tests
{
    public class UserTests
    {
        private readonly ITestOutputHelper _outputHelper;
        public const string UserUrl = "api/user";
        public UserTests(ITestOutputHelper outputHelper) => _outputHelper = outputHelper;

        [Fact]
        public async Task Given_A_Call_To_Get_User_Endpoint_Should_Return_200_Ok()
        {
            using var testContext = new TestContext(_outputHelper).Start();
            var response = await testContext.Client.GetAsync(UserUrl);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var actualResponse = await response.Content.ReadAsAsync<ApiConfig>();
            actualResponse.Should().BeEquivalentTo(new ApiConfig{Name="Afshin Teymoori", Token = "b58e8da4-3299-4311-b194-95200692a780" });
        }
    }
}
