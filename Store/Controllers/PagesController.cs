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
                pagesList = db.Pages.ToArray().Select(x => new PageVM(x)).ToList();
            }
                // Возвращаем список в представление

                return View(pagesList);
        }
        // GET: Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }
        // POST: Pages/AddPage
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult AddPage(PageVM model)
        {
            //проверка модели на валидность
            if(!ModelState.IsValid || model == null)
            {
                return View(model);
            }
            using (Db db = new Db())
            {                             
                // Инициализируем класс PageDTO
                Page dto = new Page();

                //Присвоить заголовок модели
                dto.Title = model.Title;
              
               
                //Проверяем уникальность заголовка и краткого описания
                if(db.Pages.Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exist");
                    return View(model);
                }               
                //Присваиваем оставшиеся значения модели
              
                dto.Slug = model.Slug;
                dto.Body = model.Body;
               
                //Сохраняем модель в базу
                db.Pages.Add(dto);
                db.SaveChanges();
            }
            //Выдать сообщение пользовотелю о результате через TempData
            TempData["SM"] = "You have added a new page!";
            //Переадресовываем пользователя на метод INDEX
            return RedirectToAction("Index");
                
        }
        // GET: Pages/EditPage
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            PageVM model;
            using(Db db = new Db())
            {
                Page dto = db.Pages.Find(id);
                if(dto == null)
                {
                    return Content("The product does not exist");
                }
                model = new PageVM(dto);
                
            }
            return View(model);
        }
        // POST: Pages/EditPage
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult EditPage(PageVM model)
        {
            if(!ModelState.IsValid )
            {
                return View(model);
            }
            int id = model.Id;

            using (Db db = new Db())
            {
                Page dto = db.Pages.Find(id);               
                dto.Title = model.Title;
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exist");
                    return View(model);
                }
                dto.Slug = model.Slug;
                dto.Body = model.Body;
                db.SaveChanges();
            }
            TempData["SM"] = "You have edited the product";
            return RedirectToAction("EditProduct");
        }
        [HttpGet]
        public ActionResult PageDetails(int id)
        {
            PageVM model;
            using (Db db = new Db())
            {
                Page dto = db.Pages.Find(id);
                if (dto == null)
                {
                    return Content("The product does not exist");
                }
                model = new PageVM(dto);

            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult DeletePage(int id)
        {          
            using (Db db = new Db())
            {
                Page dto = db.Pages.Find(id);
                db.Pages.Remove(dto);
                db.SaveChanges();

            }
            TempData["SM"] = "You have deleted product";
            return RedirectToAction("Index");
        }

    }

}