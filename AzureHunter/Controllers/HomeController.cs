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
    // References
    // http://www.codeproject.com/Articles/275056/Custom-Client-Side-Validation-in-ASP-NET-MVC
    // http://www.prideparrot.com/blog/archive/2012/8/uploading_and_returning_files
    // http://www.mikesdotnetting.com/article/259/asp-net-mvc-5-with-ef-6-working-with-files
    // http://stackoverflow.com/questions/5662923/asp-net-mvc3-custom-validation-attribute-client-side-broken


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


        //------------ADD--------------------------------------
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


        //------------DELETE-----------------------------------
        public ActionResult DeleteItem(string id)
        {
            ViewBag.Message = "Delete item";

            Item item;

            using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
            {
                item = ctx.Items.Find(int.Parse(id));
            }

            return View(item);

        }

        [HttpPost]
        public ActionResult DeleteItem(Item item)
        {
            // //set to delete
            // http://stackoverflow.com/questions/15945172/the-object-cannot-be-deleted-because-it-was-not-found-in-the-objectstatemanager
            ViewBag.Message = "Item was deleted.";

            using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
            {
                ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                ctx.Items.Remove(item);
                ctx.SaveChanges();
            }

            return View("ItemWasDeleted", item);
        }


        //------------EDIT-----------------------------------
        public ActionResult EditItem(string id)
        {
            ViewBag.Message = "Edit item";

            Item item;

            using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
            {
                item = ctx.Items.Find(int.Parse(id));
            }

            return View(item);

        }

        [HttpPost]
        public ActionResult EditItem(Item item)
        {
            using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
            {
                ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();
            }

            return View("EditItemConfirm", item);
        }


        //-----------DETAILS----------------------------------
        public ActionResult DetailItem(string id)
        {
            ViewBag.Message = "Detail item";

            Item item;

            using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
            {
                item = ctx.Items.Find(int.Parse(id));
            }

            return View(item);

        }


        //------------ FILES ---------------------------------
        public ActionResult Files(IEnumerable<File> files)
        {
            ViewBag.Message = "Your files";
            if (files == null)
            {
                using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
                {
                    files = ctx.Files.ToList();
                }
            }
            return View(files);
        }


        public ActionResult CreateFile()
        {
            var file = new File();
            return View(file);
        }


        [HttpPost]
        public ActionResult CreateFile(HttpPostedFileBase upload)
        {

            return View();
        }

    }
}