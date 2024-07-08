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
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }

        public void Update(OrderDetail entity)
        {
            //var entityForUpdate = _context.catigory.FirstOrDefault(c => c.Id == entity.Id);
            //if (entityForUpdate != null)
            //{
            //    entityForUpdate.Name = entity.Name;
            //    entityForUpdate.Description = entity.Description;
            //    entityForUpdate.DateCreate = entity.DateCreate;
            //}
        }

        public void UpdateOrderStatus(int id, string orderStatus, string paymentStatus)
        {
           // var orderfromDB = _context.orderDetail.FirstOrDefault(x => x.Id == id);
            
            
            //if (orderfromDB != null)
            //{
            //    orderfromDB.OrderStatus = orderStatus;  
            //    if(paymentStatus != null)
            //    {
            //       orderfromDB.PaymentStatus = paymentStatus;
            //    }
            //}
        }
    }
}
