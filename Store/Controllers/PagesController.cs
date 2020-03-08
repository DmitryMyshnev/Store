using Store.Models.Data;
using Store.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class PagesController : Controller
    {
        // GET: Pages
        public ActionResult Index()
        {
            //ОбЬявляем список для представления (PagesVM)
            List<PageVM> pagesList;

            // Инициализируем список
            using (Db db = new Db())
            {
                pagesList = db.Product.ToArray().Select(x => new PageVM(x)).ToList();
            }
                // Возвращаем список в представление

                return View(pagesList);
        }
        // GET: Pages/AddPage
        [HttpGet]
        public ActionResult AddProduct()
        {
            return View();
        }
        // POST: Pages/AddPage
        [HttpPost]
        public ActionResult AddProduct(PageVM model)
        {
            //проверка модели на валидность
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {                             
                // Инициализируем класс PageDTO
                PagesProduct dto = new PagesProduct();

                //Присвоить заголовок модели
                dto.Title = model.Title;
              
               
                //Проверяем уникальность заголовка и краткого описания
                if(db.Product.Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exist");
                    return View(model);
                }
               

                //Присваиваем оставшиеся значения модели
              
                dto.Body = model.Body;
                dto.Price = model.Price;
               
                //Сохраняем модель в базу
                db.Product.Add(dto);
                db.SaveChanges();
            }
            //Выдать сообщение пользовотелю о результате через TempData
            TempData["SM"] = "You have added a new page!";
            //Переадресовываем пользователя на метод INDEX
            return RedirectToAction("Index");
                
        }
    }
}