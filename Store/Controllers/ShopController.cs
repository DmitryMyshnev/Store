using Store.Models.Data;
using Store.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Categories()
        {
            List<CategoryVM> categoryVMList;
            using (Db db = new Db())
            {
                categoryVMList = db.Categories.ToArray().Select(x => new CategoryVM(x)).ToList();
            }
            return View(categoryVMList);
        }
       
        [HttpPost]
        public ActionResult AddNewCategory(CategoryProduct model)
        {
            return View();
        }
    }
}