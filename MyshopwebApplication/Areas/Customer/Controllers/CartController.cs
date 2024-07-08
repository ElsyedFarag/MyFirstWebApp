using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModel;
using MyShop.Etuilities;
using Newtonsoft.Json.Converters;
using Stripe;
using Stripe.Checkout;
using Stripe.Issuing;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace MainProject.Web.Areas.Customer.Controllers
{
    [Area("customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        private ShoppingCartView shoppingCartView;
        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]   
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartView = new ShoppingCartView()
            {
                shopingCarts = unitOfWork.ShopingCart.FindAll(s => s.ApplicationUserId == claim.Value, include: "Product"),
            };
            //if (shoppingCartView.shopingCarts.IsNullOrEmpty())
            //{
            //    return Redirect("/customer/Customer/Index");

            //}
            return View(shoppingCartView);
        }
        public IActionResult AddMins(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var shopingCart = unitOfWork.ShopingCart.Find(x => x.Id == id, null);
            if (shopingCart.Count <= 1)
            {
                unitOfWork.ShopingCart.Remove(shopingCart);
                var count = unitOfWork.ShopingCart.FindAll(x => x.ApplicationUserId == shopingCart.ApplicationUserId, null).ToList().Count() - 1;
                HttpContext.Session.SetInt32(DS.SessionKey, count);
                unitOfWork.Complete();

                if (unitOfWork.ShopingCart.FindAll(x => x.ApplicationUserId == claim.Value, null).Count() != 0)
                {
                    return Redirect("/Customer/Cart/Index");
                }
                return Redirect("/Customer/Customer/Index");
            }
            shopingCart.Count -= 1;
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        public IActionResult AddPlus(int id)
        {

            var shopingCart = unitOfWork.ShopingCart.Find(x => x.Id == id, null);
            shopingCart.Count += 1;

            unitOfWork.Complete();

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var shopingCart = unitOfWork.ShopingCart.Find(x => x.Id == id,null);
            unitOfWork.ShopingCart.Remove(shopingCart);
            unitOfWork.Complete();
            HttpContext.Session.SetInt32(DS.SessionKey, unitOfWork.ShopingCart.FindAll(x => x.ApplicationUserId == shopingCart.ApplicationUserId, null).ToList().Count());

            return RedirectToAction("Index");
        }
        public IActionResult Summary()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var shopingOrder = new ShoppingCartVM()
            {
                CartList = unitOfWork.ShopingCart.FindAll(x => x.ApplicationUserId == claim.Value, include: "Product"),
                OrderHeader = new()
            };
            var customer = unitOfWork.ApplicationUser.Find(x => x.Id == claim.Value, null);
            shopingOrder.OrderHeader.ApplicationUser = customer;
            shopingOrder.OrderHeader.Name = customer.Name;
            shopingOrder.OrderHeader.Address = customer.Address;
            shopingOrder.OrderHeader.City = customer.City;
            shopingOrder.OrderHeader.PhoneNumber = customer.PhoneNumber;
            shopingOrder.OrderHeader.ShippingDate = DateTime.Now;
            foreach (var item in shopingOrder.CartList)
                shopingOrder.OrderHeader.TotalPrice += (item.Product.Price * item.Count);
            shopingOrder.OrderHeader.TotalPrice += shopingOrder.Shipping;
            shopingOrder.SubTotal = shopingOrder.OrderHeader.TotalPrice - shopingOrder.Shipping;
            return View(shopingOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostSummary(ShoppingCartVM shoppingCartVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            shoppingCartVM.CartList = unitOfWork.ShopingCart.FindAll(p => p.ApplicationUserId == claim.Value, include: "Product");

            shoppingCartVM.OrderHeader.OrderStatus = "Pending";
            shoppingCartVM.OrderHeader.PaymentStatus = "Pending";
            shoppingCartVM.OrderHeader.ShippingDate = DateTime.Now;
            shoppingCartVM.OrderHeader.ApplicationUser = unitOfWork.ApplicationUser.Find(x => x.Id == claim.Value, null);
            shoppingCartVM.OrderHeader.ApplicationUId = shoppingCartVM.OrderHeader.ApplicationUser.Id;
            foreach (var item in shoppingCartVM.CartList)
            {
                shoppingCartVM.OrderHeader.TotalPrice += (item.Product.Price * item.Count);
            }
            shoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            unitOfWork.OrderHeader.Add(shoppingCartVM.OrderHeader);
            unitOfWork.Complete();

            foreach (var item in shoppingCartVM.CartList)
            {
                OrderDetail orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    Count = item.Count,
                    OrderId = shoppingCartVM.OrderHeader.Id,
                    Price = item.Product.Price,
                };
                unitOfWork.OrderDetails.Add(orderDetail);
                unitOfWork.Complete();
            }

            var fakeUser = unitOfWork.ApplicationUser.FindAll(x => x.Email == shoppingCartVM.OrderHeader.ApplicationUser.Email && x.UserName == null, null);
            if (fakeUser != null)
            {
                unitOfWork.ApplicationUser.RemoveRange(fakeUser);
                unitOfWork.Complete();
            }
            var domain = "https://localhost:44311";

            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"/customer/Cart/OrderConfirmation?id={shoppingCartVM.OrderHeader.Id}",
                CancelUrl = domain + $"/customer/Cart/Index",
            };

            foreach (var item in shoppingCartVM.CartList)
            {
                var sessionlineoptions = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.Price) * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },
                    },
                    Quantity = item.Count,
                };
                options.LineItems.Add(sessionlineoptions);
            }

            try
            {
                var service = new SessionService();
                Session session = service.Create(options);
                shoppingCartVM.OrderHeader.SessionId = session.Id;
                unitOfWork.Complete();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }
            catch
            {
                return Redirect("/Customer/Cart/PageError");
            }

        }


        public IActionResult OrderConfirmation(int id)
        {
            try
            {
                OrderHeader orderHeader = unitOfWork.OrderHeader.Find(x => x.Id == id, null);

                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    unitOfWork.OrderHeader.UpdateOrderStatus(orderHeader.Id, "Approve", "Approve");
                    orderHeader.PaymentIntentId = session.PaymentIntentId;
                    unitOfWork.Complete();
                }

                List<ShopingCart> shopingCarts = unitOfWork.ShopingCart.FindAll(x => x.ApplicationUserId == orderHeader.ApplicationUId, null).ToList();
                unitOfWork.ShopingCart.RemoveRange(shopingCarts);
                unitOfWork.Complete();
                HttpContext.Session.SetInt32(DS.SessionKey, 0);
                return View(orderHeader.Id);
            }
            catch (Exception ex)
            {
                return Redirect("/Customer/Cart/PageError");
            }
        }


       
        public IActionResult PageError()
        {
            return View();
        }

    }
}

