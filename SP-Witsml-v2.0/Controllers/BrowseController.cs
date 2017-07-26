using SP_Witsml_v2._0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace SP_Witsml_v2._0.Controllers
{
    [Authorize]
    public class BrowseController : Controller
    {
        public ActionResult BrowseFiles()
        {
            string userId = User.Identity.GetUserId();
            var db = new ApplicationDbContext();
            return View(db.LogObjects.Where(c=>c.ApplicationUserId == userId).ToList());
        }

        public ActionResult Delete(int id)
        {
            var db = new ApplicationDbContext();
            return View(db.LogObjects.Where(c => c.Id == id).First());
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection form)
        {
            var db = new ApplicationDbContext();
            LogObject lg = db.LogObjects.Where(l => l.Id == id).First();
            db.LogObjects.Remove(lg);
            db.SaveChanges();

            return RedirectToAction("BrowseFiles");
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, LogObject log)
        {
            var db = new ApplicationDbContext();
            var lg = db.LogObjects.Where(l => l.Id == id).First();
            lg.Name = log.Name;
            db.SaveChanges();
            return RedirectToAction("BrowseFiles");
        }

    }
}