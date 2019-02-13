using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Watches.Models.Classes;

namespace Watches.Controllers
{
    public class HomeController : Controller
    {
        private WatchContext db = new WatchContext(); 
        public ActionResult Index()
        {
            ViewBag.watch = db.Watches.First();
            ///////////////////////////////////
            ViewBag.feature = db.Features.First();
            ViewBag.fe = db.Features.ToList();
            /////////////////////////////////////
            ViewBag.grade = db.Grades.First();
            ViewBag.gr = db.Grades.ToList();
            ///////////////////////////////////
            ViewBag.choose = db.Chooses.First();
            ViewBag.ch = db.Chooses.ToList();
            ///////////////////////////////////
            ViewBag.typewatch = db.TypesWatches.First();
            ViewBag.tw = db.TypesWatches.ToList();
            //////////////////////////////////////
            ViewBag.update = db.Updates.First();
            ///////////////////////////////////////
            ViewBag.review = db.Reviews.First();
            ViewBag.re = db.Reviews.ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.choose = db.Chooses.First();
            ViewBag.ch = db.Chooses.ToList();
            /////////////////////////////////////
            ViewBag.typewatch = db.TypesWatches.First();
            ViewBag.tw = db.TypesWatches.ToList();
            return View();
        }
        public ActionResult Services()
        {
            ViewBag.feature = db.Features.First();
            ViewBag.fe = db.Features.ToList();
            //////////////////////////////////
            ViewBag.grade = db.Grades.First();
            ViewBag.gr = db.Grades.ToList();
            return View();
        }

        public ActionResult Contact()
        {

            ViewBag.update = db.Updates.First();
            return View();
        }
    }
}