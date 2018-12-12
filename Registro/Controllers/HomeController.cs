﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Registro.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Your index page.";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult AddOrEdit()
        {
            ViewBag.Message = "Your registration page.";

            return View();
        }

        public ActionResult PaginaPrueba()
        {
            ViewBag.Message = "Your pagina de prueba";
            return View();
        }
    }
}