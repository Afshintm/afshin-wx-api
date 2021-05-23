using System.Collections.Generic;
namespace At.Wx.Api.Models
{
    public class Basket
    {
        public int CustomerId { get; set; }
        public List<Product> Products { get; set; }
    }
}
