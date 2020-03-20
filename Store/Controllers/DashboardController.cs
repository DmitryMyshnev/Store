using Store.Models.Data;
using Store.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
      //  [HttpGet]
        public ActionResult CategoryMenuPartial()
        {
            List<CategoryVM> categoryVMList;
            using (Db db = new Db())
            {
                categoryVMList = db.Categories.ToArray().Select(x => new CategoryVM(x)).ToList();
            }
            return PartialView("_CategoryMenuPartial", categoryVMList);

        }

        public ActionResult Category(string name)
        {
            List<ProductVM> productVMList;
            using (Db db = new Db())
            {
                CategoryProduct categoryDTO = db.Categories.Where(x => x.Slug == name).FirstOrDefault();
                int catId = categoryDTO.Id;
                productVMList = db.Products.ToArray().Where(x => x.CategoryId == catId).Select(x => new ProductVM(x)).ToList();
                var productCat = db.Products.Where(x => x.CategoryId == catId).FirstOrDefault();
                if(productCat == null)
                {
                    var catName = db.Categories.Where(x => x.Slug == name).Select(x => x.Name).FirstOrDefault();
                    ViewBag.CategoryName = catName;
                }
                else
                {
                    ViewBag.CategoryName = productCat.CategoryName;
                }
            }
                return View(productVMList);
        }
       // [ActionName("product-details")]
        public ActionResult ProductDatalies(string name)
        {
            Product dto;
            ProductVM model;
            int id = 0;
            using(Db db = new Db())
            {
                if(!db.Products.Any(x =>x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }
                dto = db.Products.Where(x => x.Slug == name).FirstOrDefault();
                id = dto.Id;
                model = new ProductVM(dto);
            }
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                               .Select(fn => Path.GetFileName(fn));
            return View("ProductDatalies",model);
        }
    }
}