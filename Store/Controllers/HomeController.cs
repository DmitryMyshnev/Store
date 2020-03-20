using PagedList;
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
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index(int? page, int? catId)
        {
            List<ProductVM> listOfProductVM;
            var pageNumber = page ?? 1;
            using (Db db = new Db())
            {
                listOfProductVM = db.Products.ToArray()
                                   .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                                   .Select(x => new ProductVM(x))
                                   .ToList();
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                ViewBag.SelectedCat = catId.ToString();
            }
            var onePageOfProducts = listOfProductVM.ToPagedList(pageNumber, 3);
            ViewBag.onePageOfProducts = onePageOfProducts;
            return View(listOfProductVM);
            
        }
        
        public ActionResult ProductDatalies(string name)
        {
            Product dto;
            ProductVM model;
            int id = 0;
            using (Db db = new Db())
            {
                if (!db.Products.Any(x => x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }
                dto = db.Products.Where(x => x.Slug == name).FirstOrDefault();
                id = dto.Id;
                model = new ProductVM(dto);
            }
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                               .Select(fn => Path.GetFileName(fn));
            return View("ProductDatalies", model);
        }

    }
}