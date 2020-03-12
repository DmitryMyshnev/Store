using Store.Models.Data;
using Store.Models.ViewModels.Pages;
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
        [HttpGet]
        public ActionResult AddNewCategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddNewCategory(string name)
        {
            // string id;
            if (name.Length == 0 || name.StartsWith(" "))
            {
                TempData["Error"] = "Title is empty";
                return RedirectToAction("Categories");
            }

            using (Db db = new Db())
            {
                
                // Инициализируем класс PageDTO
                CategoryProduct dto = new CategoryProduct();

                //Присвоить заголовок модели
                dto.Name = name;
                //  Проверяем уникальность заголовка и краткого описания
                if (db.Categories.Any(x => x.Name == name))
                {
                    // ModelState.AddModelError("Name", "That title already exist");
                    TempData["Error"] = "That title already exist";
                   return RedirectToAction("Categories");
                }
               
                //Присваиваем оставшиеся значения модели

                dto.Slug = name.Replace(" ","-").ToLower();
                
                //Сохраняем модель в базу
                db.Categories.Add(dto);
                db.SaveChanges();
            }
            //Выдать сообщение пользовотелю о результате через TempData
            TempData["SM"] = "You have added a new page!";
            //Переадресовываем пользователя на метод INDEX
            return RedirectToAction("Categories");
           
          
        }
        
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {
                CategoryProduct dto = db.Categories.Find(id);
                db.Categories.Remove(dto);
                db.SaveChanges();

            }
            TempData["SM"] = "You have deleted product";
            return RedirectToAction("Categories");
        }
        public string EditCategory(string name,int id)
        {
            using (Db db = new Db())
            {

                // Инициализируем класс PageDTO
                CategoryProduct dto = db.Categories.Find(id);

                //Присвоить заголовок модели              
                dto.Name = name;
                            
                //Присваиваем оставшиеся значения модели

                dto.Slug = name.Replace(" ", "-").ToLower();

                //Сохраняем модель в базу               
                db.SaveChanges();
            }
            //Выдать сообщение пользовотелю о результате через TempData
            TempData["SM"] = "The category name has been changed!";
            //Переадресовываем пользователя на метод INDEX
            return "ok";         
        }
        [HttpGet]
        public ActionResult AddProduct()
        {
            PageVM model = new PageVM();
               using(Db db = new Db())
            {
                model.Categories = new SelectList(db.Categories.ToList(), dataValueField:"id",dataTextField: "Name");
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AddProduct(PageVM model, HttpPostedFileBase file)
        {
            if(!ModelState.IsValid)
            {
                using(Db db = new Db())
                {
                    model.Categories = new SelectList(db.Categories.ToList(), dataValueField: "Id", dataTextField: "Name");
                    return View(model);
                }
            }
            using (Db db = new Db())
            {
                if(db.Products.Any(x =>x.Name == model.Name))
                {
                    model.Categories = new SelectList(db.Categories.ToList(), dataValueField: "Id", dataTextField: "Name");
                    ModelState.AddModelError("", "This proudct name is taken!");
                    return View(model);
                }
            }
            int id;
            using (Db db = new Db())
            {
                PagesProduct product = new PagesProduct();
                product.Name = model.Name;
                product.Slug = model.Slug;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                CategoryProduct category = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                product.CategoryName = category.Name;
                db.Products.Add(product);
                db.SaveChanges();
                id = product.Id;
            }
                return RedirectToAction("AddProduct");
        }
    }
}