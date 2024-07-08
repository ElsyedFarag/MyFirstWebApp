using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork context)
        {
            _unitOfWork = context;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;
            return View(_unitOfWork.ApplicationUser.FindAll(x=>x.Id != userId,null).ToList());
        }
        public IActionResult LockUser(string id)
        {
            var user = _unitOfWork.ApplicationUser.Find(x => x.Id == id, null);
            if (user == null)
            {
                return NotFound();
            }
            if(user.LockoutEnd == null | user.LockoutEnd < DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddYears(1);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }
            _unitOfWork.Complete();
           return RedirectToAction("Index", "Users", new {area="Admin"} );    
        }
     
        public ActionResult Delete(string? id)
        {
            var userDel =_unitOfWork.ApplicationUser.Find(u => u.Id == id,null);
            _unitOfWork.ApplicationUser.Remove(userDel);
            _unitOfWork.Complete();

            return RedirectToAction("Index");
        }
    }
}
