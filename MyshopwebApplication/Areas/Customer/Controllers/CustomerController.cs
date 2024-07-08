using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyShop.DataAccess.Data;
using MyShop.DataAccess.Implemention;
using MyShop.Entities;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModel;
using MyShop.Etuilities;
using System.Security.Claims;
using X.PagedList;

namespace MyShop.Areas.Customer.Controllers
{
    [Area("customer")]
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index(int? Page)
        {

            var pageNumber = Page ?? 1;
            var pageSize = 8;
            var productsInDb = unitOfWork.Product.GetAll().ToPagedList(pageNumber,pageSize);
            
            return View(productsInDb);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            ProductShopingCart obj = new ProductShopingCart()
            {
                shopingCart = new ShopingCart()
                {
                    ProductId = id,
                    Product = unitOfWork.Product.Find(x => x.Id == id, include:"catigory"),
                    Count = 1,
                },
                
            };
            var catigoryId = unitOfWork.Product.Find(x => x.Id == id, include: "catigory").catigory.Id;
            var listOfProducts = unitOfWork.Product.FindAll(p => p.CatigoryId == catigoryId, null);
            List<Product> listOfProductsFinally = new List<Product>();
            foreach (var item in listOfProducts)
                if(item.Id != obj.shopingCart.Product.Id)
                    listOfProductsFinally.Add(item);
            obj.products = listOfProductsFinally;  
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShopingCart shopingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shopingCart.ApplicationUserId = claim.Value;

            var shopCart = new ShopingCart
            {
                ApplicationUserId = claim.Value,
                Count = shopingCart.Count,
                ProductId = shopingCart.ProductId
            };

            ShopingCart shopingCartQury = unitOfWork.ShopingCart.Find(x => x.ApplicationUserId == claim.Value && x.ProductId == shopCart.ProductId, null);

            if (shopingCartQury == null)
            {
                unitOfWork.ShopingCart.Add(shopCart);
                unitOfWork.Complete();

                HttpContext.Session.SetInt32(DS.SessionKey, unitOfWork.ShopingCart.FindAll(x => x.ApplicationUserId == claim.Value, null).ToList().Count());

            }
            else
            {
                IncressCountProduct(shopingCartQury, shopingCart.Count);
                unitOfWork.Complete();

            }
            return RedirectToAction("Index");
        }
        public void IncressCountProduct(ShopingCart entity, int value)
        {
            var product = unitOfWork.ShopingCart.Find(e => e.Id == entity.Id, null);
            if (product != null)
            {
                product.Count += value;
                unitOfWork.Complete();
            }
        }

    }
}
