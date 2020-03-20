using Store.Models.Data;
using Store.Models.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Store.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }
        [ActionName("create-account")]
        [HttpGet]
        public ActionResult CreateAccount()
        {

            return View("CreateAccount");
        }
       
        [ActionName("create-account")]
        [HttpPost]
        public ActionResult CreateAccount(UserVM model)         
        {
            if (!ModelState.IsValid)
                return View("Createaccount", model);
            if(!model.Password.Equals(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password do not match!");
                return View("CreateAccount", model);
            }
            using(Db db = new Db())
            {
                if (db.Users.Any(x => x.Username.Equals(model.Username)))
                {
                    ModelState.AddModelError("", $"Username{model.Username} is taken.");
                    model.Username = "";
                    return View("CreateAccount", model);
                }
                User user = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAdress = model.EmailAdress,
                    Username = model.Username,
                    Password = model.Password
                };
                db.Users.Add(user);
                db.SaveChanges();
                int id = user.Id;
                UserRole userRole = new UserRole()
                {
                    UserId = id,
                    RoleId = 2
                };
                db.UserRoles.Add(userRole);
                db.SaveChanges();
            }
            TempData["SM"] = "You are now registered. ";

            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Login()
        {
            string userName = User.Identity.Name;
            if (!string.IsNullOrEmpty(userName))
            {
                return RedirectToAction("user-profile");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginUserVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            bool isValid = false;
            using (Db db = new Db())
            {
                if (db.Users.Any(x => x.Username.Equals(model.Username) && x.Password.Equals(model.Password)))
                    isValid = true;
                if(!isValid)
                {
                    //ModelState.AddModelError("", "Invalid username or password");
                    TempData["Error"] = "Invalid username or password";
                    return Redirect("Login");
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(model.Username, model.RememberMe);
                    return Redirect(FormsAuthentication.GetRedirectUrl(model.Username, model.RememberMe));
                }
            }              
        }
        [HttpGet]
        public ActionResult Logout(LoginUserVM model)
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult UserNavPartial()
        {
            string userName = User.Identity.Name;
            UserNavPartialVM model;
            using(Db db = new Db())
            {
                User dto = db.Users.FirstOrDefault(x => x.Username == userName);
                model = new UserNavPartialVM()
                { 
                  FirstName = dto.FirstName,
                  LastName = dto.LastName
                };
            }
            return PartialView(model);
        }
        [HttpGet]
        [ActionName("user-profile")]
        public ActionResult UserProfile()
        {
            string userName = User.Identity.Name;
            UserProfileVM model;
            using (Db db = new Db())
            {
                User dto = db.Users.FirstOrDefault(x => x.Username == userName);
                model = new UserProfileVM(dto);
            }

            return View("UserProfile",model);
        }
        [HttpPost]
        [ActionName("user-profile")]
        public ActionResult UserProfile(UserProfileVM model)
        {
            bool userNameIsChanged = false;
            if(!ModelState.IsValid)
            {              
                return View("UserProfile",model);
            }
            if(!string.IsNullOrWhiteSpace(model.Password))
            {
                if(!model.Password.Equals(model.ConfirmPassword))
                {
                    ModelState.AddModelError("", "Pssword do not match!");
                    return View("UserProfile", model);
                }
            }
            using(Db db = new Db())
            {
                string userName = User.Identity.Name;
                if (userName != model.Username)
                {
                    userName = model.Username;
                    userNameIsChanged = true;

                    if (db.Users.Where(x => x.Id == model.Id).Any(x => x.Username == userName))
                    {
                        ModelState.AddModelError("", $"Username {model.Username} alredy exsists.");
                        model.Username = "";
                        return View("UserProfile", model);
                    }
                }
                User dto = db.Users.Find(model.Id);
                dto.FirstName = model.FirstName;
                dto.LastName = model.LastName;
                dto.EmailAdress = model.EmailAdress;
                dto.Username = model.Username;
                if(!string.IsNullOrWhiteSpace(model.Password))
                {
                    dto.Password = model.Password;
                }
                db.SaveChanges();
            }
            TempData["SM"] = "You have edited priofile.";
            if(!userNameIsChanged)
                 return View("UserProfile",model);
            else
                return RedirectToAction("Logout");
        }
    }
}