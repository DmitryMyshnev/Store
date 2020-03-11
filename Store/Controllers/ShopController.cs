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
        public string EditCategory(string name, int id)
        {
            return name;
        }
    }
}