using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Repositories;

namespace StartUpWebApllication.Myshop.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles ="Admin")]
    public class DashboardController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DashboardController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.orders = unitOfWork.OrderHeader.FindAll(x => x.OrderStatus == "Approve", null).ToList().Count();
            ViewBag.allorders = unitOfWork.OrderHeader.GetAll().ToList().Count();
            ViewBag.products = unitOfWork.Product.GetAll().ToList().Count();
            int value = unitOfWork.ApplicationUser.GetAll().ToList().Count();
            value--;
            ViewBag.users = value;

            return View();
        }
    }
}
