using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.ViewModel
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShopingCart> CartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Shipping = 14.9m;
    }
}
