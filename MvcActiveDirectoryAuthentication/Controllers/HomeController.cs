using MvcActiveDirectoryAuthentication.Helpers;
using MvcActiveDirectoryAuthentication.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcActiveDirectoryAuthentication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var isAuthenticated = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            return View();
        }        
    }
}
