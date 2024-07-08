using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositories
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        public void Update(OrderDetail entity);
        public void UpdateOrderStatus(int id,string orderStatus,string paymentStatus);
    }
}
