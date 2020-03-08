﻿using Store.Models.Data;
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
        // GET: Pages/EditPage
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            PageVM model;
            using(Db db = new Db())
            {
                PagesProduct dto = db.Product.Find(id);
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
        public ActionResult EditProduct(PageVM model)
        {
            if(!ModelState.IsValid )
            {
                return View(model);
            }
            int id = model.Id;

            using (Db db = new Db())
            {
                PagesProduct dto = db.Product.Find(id);               
                dto.Title = model.Title;
                if (db.Product.Where(x => x.Id != id).Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title already exist");
                    return View(model);
                }
                dto.Body = model.Body;
                dto.Price = model.Price;
                db.SaveChanges();
            }
            TempData["SM"] = "You have edited the product";
            return RedirectToAction("EditProduct");
        }
    }
  
}