using PagedList;
using Store.Models.Data;
using Store.Models.ViewModels.Pages;
using Store.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        [HttpGet]
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
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult AddNewCategory(string name)
        {
            // string id;
            
            if (name.Length == 0 || name.StartsWith(" ", StringComparison.CurrentCulture))
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

                dto.Slug = name.Replace(" ", "-").ToUpperInvariant ();

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
            ProductVM model = new ProductVM();
               using(Db db = new Db())
            {
                model.Categories = new SelectList(db.Categories.ToList(), dataValueField:"id",dataTextField: "Name");
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryTokenAttribute]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
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
                Product product = new Product();
                product.Name = model.Name;
                product.Slug = model.Name.Replace(" ", "-").ToLower(CultureInfo.CurrentCulture); ;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                CategoryProduct category = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                product.CategoryName = category.Name;
                db.Products.Add(product);
                db.SaveChanges();
                id = product.Id;
            }
            TempData["SM"] = "You have added a product!";
            ///////////////////////////////////////////////
            var originalDirectory = new DirectoryInfo(string.Format(CultureInfo.InvariantCulture,$"{Server.MapPath(@"\")}Images\\Uploads"));
            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" +id.ToString(CultureInfo.CurrentCulture));
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" +id.ToString(CultureInfo.CurrentCulture) + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" +id.ToString(CultureInfo.CurrentCulture) + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" +id.ToString(CultureInfo.CurrentCulture) + "\\Gallery\\Thumbs");
            if(!Directory.Exists(pathString1))            
                Directory.CreateDirectory(pathString1);
            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);
            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);
            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);
            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);
            if (file != null && file.ContentLength > 0)
            {
                string ext = file.ContentType.ToLower(CultureInfo.CurrentCulture);
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        model.Categories = new SelectList(db.Categories.ToList(), dataValueField: "Id", dataTextField: "Name");
                        ModelState.AddModelError(key: "", errorMessage: "The image was not uploaded - wrong image extation.");
                        return View(model);
                    }
                }

                string imageName = file.FileName;
                using (Db db = new Db())
                {
                    Product dto = db.Products.Find(id);
                    dto.ImageName = imageName;
                    db.SaveChanges();
                }

                var path = string.Format(CultureInfo.InvariantCulture, $"{pathString2}\\{imageName}"); // оригинальное изображение
                var path2 = string.Format(CultureInfo.InvariantCulture, $"{pathString3}\\{imageName}");//уменьшенная копия
                file.SaveAs(path);
                WebImage img = new WebImage(file.InputStream);
                img.Resize(width: 200,height: 200);
                img.Save(path2);
            }
           
            ///////////////////////////////////////////////

            return RedirectToAction("AddProduct");
        }
        [HttpGet]
        
        public ActionResult Products(int? page, int? catId)
        {
            List<ProductVM> listOfProductVM;
            var pageNumber = page ?? 1;
            using(Db db = new Db())
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
    }
}