using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyShop.DataAccess.Implemention;
using MyShop.Entities;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;

namespace MyShop.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles ="Admin")]
    public class CatigoryController : Controller
    {

        
        private readonly IUnitOfWork unitOfWork;

        public CatigoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        // GET: CatigoryController1
        public ActionResult Index()
        {
            var catigories = unitOfWork.Catigory.GetAll();
            return View(catigories);
        }

        // GET: CatigoryController1/Details/5
        public ActionResult Details(int id)
        {
            return View(unitOfWork.Catigory.Find(c => c.Id == id,null));
        }

        // GET: CatigoryController1/Create
        public ActionResult CreateCatigory()
        {
            return View();
        }

        // POST: CatigoryController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCatigory(Catigory catigory)
        {
            try
            {
                
                unitOfWork.Catigory.Add(catigory);
                unitOfWork.Complete();
                TempData["TitleCreate"] = "Catigory has Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            catch
            {

                return View();
            }
        }

        // GET: CatigoryController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View(unitOfWork.Catigory.Find(c=>c.Id == id,null));
        }

        // POST: CatigoryController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Catigory catigory)
        {
            try
            {
                unitOfWork.Catigory.Update(catigory);
                unitOfWork.Complete();
                TempData["Update"] = "Ok";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CatigoryController1/Delete/5
        public ActionResult Delete(int id)
        {

            return View(unitOfWork.Catigory.Find(c=>c.Id == id,null));
        }

        // POST: CatigoryController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Catigory collection)
        {
            try
            {
                if (collection == null)
                {
                    NotFound();
                }
                unitOfWork.Catigory.Remove(collection);
                unitOfWork.Complete();      

                TempData["Title"] = "Data has deleted succesfully";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
