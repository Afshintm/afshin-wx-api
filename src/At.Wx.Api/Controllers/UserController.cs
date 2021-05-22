using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using At.Wx.Api.Infrastructure;

namespace At.Wx.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApiConfig _apiConfig;

        public UserController(ApiConfig apiConfig)
        {
            _apiConfig = apiConfig;
        }
        // GET: api/<User>
        [HttpGet]
        public async Task<ApiConfig> Get()
        {
            return await Task.FromResult(_apiConfig);
        }

        [HttpGet("~/health")]
        public async Task<ActionResult> GetApiHealth()
        {
            return await Task.FromResult(Ok(new {Health = "OK"}));
        }
    }
}
