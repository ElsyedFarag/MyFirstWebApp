using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.ViewModel
{
    public class ShoppingCartView
    {
        public IEnumerable<ShopingCart> shopingCarts {  get; set; }

        public decimal TotalPrice
        { 
            get
            {
				decimal price = 0;
				foreach (var item in this.shopingCarts)
				{
					price += item.Count * item.Product.Price;
				}
				return price;
			} 
        }
        private decimal CountPrice ()
        {
            decimal price = 0;
            foreach (var item in this.shopingCarts)
            {
                price += item.Count * item.Product.Price;
            }
            return price;
        }
    }
}
