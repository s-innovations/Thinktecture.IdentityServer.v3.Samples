﻿using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;

namespace MVC_OWIN_Client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Claims()
        {
            ViewBag.Message = "Claims";

            return View();
        }

        public ActionResult Signout()
        {
            // also possible to pass post logout redirect url via properties
            //var properties = new AuthenticationProperties
            //{
            //    RedirectUri = "http://localhost:2672/"
            //};

            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }
    }
}