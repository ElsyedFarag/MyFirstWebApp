using MyShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Entities.Repositories
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        public void Update(OrderHeader entity);
        public void UpdateOrderStatus(int id,string orderStatus,string paymentStatus);
    }
}
