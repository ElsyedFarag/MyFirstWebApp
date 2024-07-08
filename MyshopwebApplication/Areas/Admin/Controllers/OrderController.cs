using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess.Implemention;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModel;
using Stripe;

namespace StartUpWebApllication.Myshop.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles ="Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM _orderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult GetAllData()
        {
            var ordersHeader = _unitOfWork.OrderHeader.FindAll(null, include: "ApplicationUser");
            return Json(new { data = ordersHeader });
        }
        [HttpGet]
        public IActionResult Detail(int id)
        {
            OrderVM orderVM = new OrderVM
            {
                OrderHeader = _unitOfWork.OrderHeader.Find(o=>o.Id == id , include:"ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetails.FindAll(o=>o.OrderId ==id,include:"Product")
            };
            return View(orderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OrderUpdate()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Find(o => o.Id == _orderVM.OrderHeader.Id, include: "ApplicationUser");
            orderHeader.Name = _orderVM.OrderHeader.Name;
            orderHeader.Address = _orderVM.OrderHeader.Address;
            orderHeader.City = _orderVM.OrderHeader.City;
            orderHeader.PhoneNumber = _orderVM.OrderHeader.PhoneNumber;
            
            _unitOfWork.OrderHeader.Update(orderHeader);
            _unitOfWork.Complete();
            TempData["UpdateOrder"] = "Order has been update successfully";
            return Redirect($"/Admin/Order/Detail/{orderHeader.Id}");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcs()
        {

            _unitOfWork.OrderHeader.UpdateOrderStatus(_orderVM.OrderHeader.Id,"Processing",null);
            _unitOfWork.Complete();
            TempData["OrderProcess"] = "ok";
            return Redirect($"/Admin/Order/Detail/{_orderVM.OrderHeader.Id}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartShip()
        {

           var orderfromDb = _unitOfWork.OrderHeader.Find(o => o.Id == _orderVM.OrderHeader.Id, null);
            orderfromDb.TrackingNumber = _orderVM.OrderHeader.TrackingNumber;
            orderfromDb.Carrier = _orderVM.OrderHeader.Carrier;
            orderfromDb.ShippingDate = DateTime.Now;
            _unitOfWork.OrderHeader.Update(orderfromDb);
            _unitOfWork.Complete();

            TempData["OrderShip"] = "ok";
            return Redirect($"/Admin/Order/Detail/{_orderVM.OrderHeader.Id}");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelOrder()
        {
            var orderfromDb = _unitOfWork.OrderHeader.Find(o => o.Id == _orderVM.OrderHeader.Id, null);
            if(orderfromDb.PaymentStatus == "Approve")
            {
                var option = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = orderfromDb.PaymentIntentId
                };
                var service = new RefundService();
                Refund refund = service.Create(option);
                _unitOfWork.OrderHeader.UpdateOrderStatus(_orderVM.OrderHeader.Id, "Cancelled", "Refund");
            }
            else
            {
                _unitOfWork.OrderHeader.UpdateOrderStatus(_orderVM.OrderHeader.Id, "Cancelled", "Cancelled");
            }
            _unitOfWork.Complete();

            TempData["OrderCancel"] = "ok";
            return Redirect($"/Admin/Order/Detail/{_orderVM.OrderHeader.Id}");
        }
    }
}
