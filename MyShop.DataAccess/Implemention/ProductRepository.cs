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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private ApplicationDbContext context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }

        public void Update(Product entity)
        {
            var entityForUpdate = context.product.FirstOrDefault(c => c.Id == entity.Id);
            if (entityForUpdate != null)
            {
                entityForUpdate.Name = entity.Name;
                entityForUpdate.Description = entity.Description;
                entityForUpdate.Price = entity.Price;
                entityForUpdate.catigory = entity.catigory;
                entityForUpdate.Img = entity.Img;
            }
        }
    }
}
