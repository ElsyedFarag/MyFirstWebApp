using MyShop.DataAccess.Data;
using MyShop.Entities;
using MyShop.Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.Implemention
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICatigoryRepository Catigory { get; }

        public IProductRepository Product { get; }

        public IShopingCartRepository ShopingCart { get; }
        public IOrderHeaderRepository OrderHeader { get; }
        public IOrderDetailRepository OrderDetails { get; }
        public IApplicationUserRepository ApplicationUser { get;  }


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Catigory = new CatigoryRepository(context);
            Product = new ProductRepository(context);
            ShopingCart = new ShopingCartRepository(context);
            OrderHeader = new OrderHeaderRepository(context);
            OrderDetails = new OrderDetailRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);
        }

        public int Complete()
        {
            
           return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
