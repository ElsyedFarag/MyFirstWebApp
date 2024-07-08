using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using MyShop.DataAccess.Implemention;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModel;



namespace MyShop.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {

        private readonly IWebHostEnvironment _webHost;
        private readonly IUnitOfWork unitOfWork;

        public ProductController(IWebHostEnvironment webHost,IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            _webHost = webHost;
        }

        
        // GET: CatigoryController1
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetAllData()
        {
           var dataProduct = unitOfWork.Product.FindAll(null,include:"catigory");
            return Json(new { data = dataProduct });
        }
        // GET: CatigoryController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CatigoryController1/Create
        public ActionResult Create()
        {
            var productVm = new ProductVm
            {
                product = new Product(),
                CatigoryList = unitOfWork.Catigory.GetAll()
               .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
               {
                   Value = c.Id.ToString(),
                   Text = c.Name
               }).ToList()
            };
            return View(productVm);
        }

        // POST: CatigoryController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductVm productVm, IFormFile file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rootPath = _webHost.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var upload = Path.Combine(rootPath, @"Images\Products");
                        var ext = Path.GetExtension(file.FileName);
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + ext), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        productVm.product.Img = @"Images\Products\" + fileName + ext;
                    }
                    unitOfWork.Product.Add(productVm.product);
                    unitOfWork.Complete();
                    TempData["TitleCreate"] = "Product has been created successfully";
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    return View(productVm);
                }
            }
            catch
            {
                return View(productVm);
            }
        }


        // GET: CatigoryController1/Edit/5
        public ActionResult Edit(int id)
        {
            var productVm = new ProductVm
            {
                product = unitOfWork.Product.Find(p=>p.Id==id,null),
                CatigoryList = unitOfWork.Catigory.GetAll()
                           .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                           {
                               Value = c.Id.ToString(),
                               Text = c.Name
                           }).ToList()
            };
            return View(productVm);
        }

        // POST: CatigoryController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductVm productVm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var rootPath = _webHost.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(rootPath, @"Images\Products");
                    var ext = Path.GetExtension(file.FileName);
                    if (productVm.product.Img != null)
                    {
                        var oldImge = Path.Combine(rootPath, productVm.product.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImge))
                        {
                            System.IO.File.Delete(oldImge);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + ext), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVm.product.Img = @"Images\Products\" + fileName + ext;
                }

                unitOfWork.Product.Update(productVm.product);
                unitOfWork.Complete();
                TempData["TitleCreate"] = "Product has been created successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                return View(productVm);
            }

        }

        [HttpDelete]
        public ActionResult DeleteData(int id)
        {
            var productInDb = unitOfWork.Product.Find(p => p.Id == id, null );
            if (productInDb == null)
            {
                return Json(new { success = false, message = "Error While Deleted" });
            }
            unitOfWork.Product.Remove(productInDb);
            unitOfWork.Complete();
            var oldImge = Path.Combine(_webHost.WebRootPath, productInDb.Img.TrimStart('\\'));
            if (System.IO.File.Exists(oldImge))
            {
                System.IO.File.Delete(oldImge);
            }
            return Json(new { success = true, message = "File Has Been Deleted" });
        }
    }
}
