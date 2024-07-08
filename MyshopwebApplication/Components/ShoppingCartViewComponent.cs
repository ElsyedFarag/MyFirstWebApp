using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop.Entities.Repositories;
using MyShop.Etuilities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StartUpWebApllication.Myshop.ViewComponents
{
    public class ShoppingCartViewComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                if (HttpContext.Session.GetInt32(DS.SessionKey) != null)
                {
                    return View(HttpContext.Session.GetInt32(DS.SessionKey));
                }
                else
                {
                    var cartItems =  _unitOfWork.ShopingCart.GetAll().Where(x => x.ApplicationUserId == claim.Value).ToList();
                    int cartItemCount = cartItems.Count();

                    HttpContext.Session.SetInt32(DS.SessionKey, cartItemCount);
                    return View(cartItemCount);
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
