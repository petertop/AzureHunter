using AzureHunter.Models;
using AzureHunter.Repository;
using AzureHunter.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
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

    // File: http://www.dotnet-tricks.com/Tutorial/mvc/aX9D090113-File-upload-with-strongly-typed-view-and-model-validation.html
    // http://www.dotnet-tricks.com/Tutorial/mvc/aX9D090113-File-upload-with-strongly-typed-view-and-model-validation.html


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
        public ActionResult Files([Bind(Include = "FileName, ContentType, FileType")]IEnumerable<ViewModelFile> files)
        {
            ViewBag.Message = "Your files";
            IEnumerable<ViewModelFile> vmfiles;
            if (files == null)
            {
                using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
                {
                    var result = ctx.Files.ToList();

                    vmfiles = result.Select(fi => new ViewModelFile
                    {
                        Id = fi.Id,
                        FileName = fi.FileName,
                        ContentType = fi.ContentType,
                        FileType = fi.FileType
                    });
                }
            }
            else
            {
                vmfiles = files.Select(fi => new ViewModelFile
                {
                    Id = fi.Id,
                    FileName = fi.FileName,
                    ContentType = fi.ContentType,
                    FileType = fi.FileType
                });
            }
            return View(vmfiles);
        }


        public ActionResult CreateFile()
        {
            var file = new File();
            return View(file);
        }


        [HttpPost]
        public ActionResult CreateFile(HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        var file = new File
                        {
                            FileName = System.IO.Path.GetFileName(upload.FileName),
                            FileType = FileType.Avatar,
                            ContentType = upload.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            file.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
                        {
                            ctx.Files.Add(file);
                            ctx.SaveChanges();
                        }

                        return RedirectToAction("Files");
                    }
                }
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(new File());

        }



        public ActionResult DeleteFile(string id)
        {
            ViewBag.Message = "File item";

            File item;

            using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
            {
                item = ctx.Files.Find(int.Parse(id));
            }

            return View(new ViewModelFile { Id = item.Id,
            FileType = item.FileType,
            FileName = item.FileName
            });

        }


        [HttpPost]
        public ActionResult DeleteFile(File file)
        {
            // //set to delete
            // http://stackoverflow.com/questions/15945172/the-object-cannot-be-deleted-because-it-was-not-found-in-the-objectstatemanager
            ViewBag.Message = "File was deleted.";

            using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
            {
                ctx.Entry(file).State = System.Data.Entity.EntityState.Deleted;
                ctx.Files.Remove(file);
                ctx.SaveChanges();
            }

            return View("FileWasDeleted", file);
        }


        public ActionResult GetFile(string id)
        {
            ViewBag.Message = "File";

            File item;

            using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
            {
                item = ctx.Files.Find(int.Parse(id));
            }

            return View(new ViewModelFile
            {
                Id = item.Id,
                FileType = item.FileType,
                FileName = item.FileName
            });

        }


        [HttpPost]
        public ActionResult GetFileContent(string id)
        {
            ViewBag.Message = "File";

            File item;

            using (var ctx = new HunterDbContext(ConfigurationManager.AppSettings["AzureHunterDatabaseCnn"]))
            {
                item = ctx.Files.Find(int.Parse(id));
            }

            return File(item.Content, item.ContentType);
        }
    }
}