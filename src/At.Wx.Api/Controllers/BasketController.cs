using System.Collections.Generic;
using System.Threading.Tasks;
using At.Wx.Api.Models;
using At.Wx.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace At.Wx.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly ProductServices _productServices;

        public BasketController(ProductServices productServices)
        {
            _productServices = productServices;
        }
        [HttpGet("~/api/sort")]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var result = await _productServices.GetProduct();
            return Ok(result);
        }
    }
}
