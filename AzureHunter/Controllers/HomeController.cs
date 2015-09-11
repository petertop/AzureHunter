using AzureHunter.Models;
using AzureHunter.Repository;
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

        public ActionResult Items(IEnumerable<Item> items)
        {
            ViewBag.Message = "Your items";
            if (items == null)
            {
                using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
                {
                    items = ctx.Items.ToList();
                }
            }
            return View(items);
        }

        public ActionResult AddItem()
        {
            ViewBag.Message = "Add item";

            var item = new Item();

            return View(item);

        }

        [HttpPost]
        public ActionResult AddItem(Item item)
        {
            ViewBag.Message = "Add item";

            List<Item> items = new List<Item>();

            using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
            {
                ctx.Items.Add(item);
                ctx.SaveChanges();

                items = ctx.Items.ToList();
            }
            return View("Items", items);
        }
    }
}