using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProjectSem3.App_Start;
using ProjectSem3.Models;

namespace ProjectSem3.Controllers
{
    public class AccountsController : Controller
    {
        private MyDbContext _dbContext;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public MyDbContext DbContext
        {
            get
            {
                return _dbContext ?? HttpContext.GetOwinContext().Get<MyDbContext>();
            }
            private set
            {
                _dbContext = value;
            }
        }
        public AccountsController()
        {

        }
        public AccountsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AppUser account, string password)
        {
            var result = await UserManager.CreateAsync(account, password);

            if (result.Succeeded)
            {
                //UserManager.AddToRole(account.Id, "User");
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity = UserManager.CreateIdentity(account, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                await UserManager.SendEmailAsync(account.Id, "Ngoc gui mail", "I am Ngoc Me Game.");
                return Redirect("/Home");
            }
            return View("Register");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            var user = UserManager.Find(username, password);
            if (user != null)
            {
                var authenticationManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = UserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);
                return Redirect("/Home");
            }
            return View("Login");
        }

        public ActionResult LogOut()
        {
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut();
            return Redirect("/Home");
        }
    }
}