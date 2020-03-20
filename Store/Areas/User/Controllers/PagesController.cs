using Store.Models.Data;
using Store.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Areas.User.Controllers
{
    public class PagesController : Controller
    {
        // GET: User/Pages
        public ActionResult Index(string page = "")
        {
            if(page == "")
                page = "home";
            PageVM model;
            Page dto;
            using(Db db = new Db())
            {
                if(!db.Pages.Any(x =>x.Slug.Equals(page)))
                    return RedirectToAction("Index", new { page = "" });               
            }
            using (Db db = new Db())
            {
                dto = db.Pages.Where(x => x.Slug == page).FirstOrDefault();
            }
            ViewBag.PageTitle = dto.Title;
            //if(dto.HasSidebar == true)     //  добавить HsSidebar
            //{
            //    ViewBag.Sidebar = "Yes";
            //}
            //else
            //{
            //    ViewBag.Sidebar = "No";
            //}
            model = new PageVM(dto);
                return View(model);
        }
        public ActionResult PagesMenuPartial()
        {
            List<PageVM> pageVMList;
            using(Db db = new Db())
            {
                pageVMList = db.Pages.ToArray().Where(x => x.Slug != "home").Select(x => new PageVM(x)).ToList();
            }
            return PartialView("_PagesMnuPartial",pageVMList);
        }
        //public ActionResult SidebarPartial()
        //{
           
        //}
    }
}