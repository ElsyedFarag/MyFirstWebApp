using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.ViewModel
{
    public class ProductShopingCart
    {
        public IEnumerable<Product> products { get; set; }  
        public ShopingCart shopingCart { get; set; }
        public int CountShoping { get; set; }
    }
}
