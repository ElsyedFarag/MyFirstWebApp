using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        public ICatigoryRepository Catigory { get; }
        public IProductRepository Product { get; }
        public IShopingCartRepository ShopingCart { get; }
        public IApplicationUserRepository ApplicationUser { get; }
        public IOrderHeaderRepository OrderHeader { get; }
        public IOrderDetailRepository OrderDetails { get; }
        int Complete();
    }
}
