﻿using AzureHunter.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureHunter.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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


        public ActionResult Items()
        {
            ViewBag.Message = "Your items";

            using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
            {
                return View(ctx.Items.ToList());
            }
        }
    }
}