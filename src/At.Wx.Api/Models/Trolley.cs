using System.Collections.Generic;

namespace At.Wx.Api.Models
{
    public class Qty
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }

    public class Special
    {
        public List<Qty> Quantities { get; set; } = new List<Qty>();
        public decimal Total { get; set; }
    }
    public class Trolley
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Special> Specials { get; set; } = new List<Special>();
        public List<Qty> Quantities { get; set; } = new List<Qty>();
    }
}
