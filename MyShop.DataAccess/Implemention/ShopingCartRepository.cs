using Microsoft.EntityFrameworkCore;
using MyShop.DataAccess.Data;
using MyShop.Entities;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implemention
{
    public class ShopingCartRepository : GenericRepository<ShopingCart>, IShopingCartRepository
    {
        private ApplicationDbContext context;
        public ShopingCartRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Update(ShopingCart entity)
        {
            
        }
        public void IncressCountProduct(ShopingCart entity, int value)
        {
            var product = context.shopingCarts.FirstOrDefault(e=>e.Id ==entity.Id);
            if (product != null)
            {
                product.Count += value;
                context.SaveChanges();

            }
        }
    }

}
