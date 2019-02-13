using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Watches.Models.Classes;
using System.Data.Entity.Infrastructure;
using System.Data;
using PagedList;

namespace Watches.Controllers
{
    public class TypesWatchesController : Controller
    {
        private WatchContext db = new WatchContext();
        // GET: TypesWatches
        public ActionResult Index(string sort,string search,string filter,int? page)
        {
            ViewBag.sort = sort;
            ViewBag.Head = string.IsNullOrEmpty(sort) ? "head_desc" : "";
            if (search!=null)
            {
                page = 1;
            }
            else
            {
                search = filter;
            }
            ViewBag.filter = search;
            var typeswatches = db.TypesWatches.Include(t => t.Choose);
            if (!string.IsNullOrEmpty(search))
            {
                typeswatches = typeswatches.Where(t => t.Title.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head_desc":
                    typeswatches = typeswatches.OrderByDescending(t => t.Choose.Head);
                    break;
                default:
                    typeswatches = typeswatches.OrderBy(t => t.Choose.Head);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(typeswatches.ToPagedList(PageNumber,PageSize));
        }

        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeWatch typeWatch = db.TypesWatches.Find(id);
            if (typeWatch==null)
            {
                return HttpNotFound();
            }
            return View(typeWatch);
        }
        //Get Create
        public ActionResult Create()
        {
            ViewBag.ChooseID = new SelectList(db.Chooses, "ChooseID", "ChooseID");
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(TypeWatch typeWatch)
        {
            if (ModelState.IsValid)
            {
                db.TypesWatches.Add(typeWatch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChooseID = new SelectList(db.Chooses, "ChooseID", "ChooseID",typeWatch.ChooseID);
            return View(typeWatch);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeWatch typeWatch = db.TypesWatches.Find(id);
            if (typeWatch == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChooseID = new SelectList(db.Chooses, "ChooseID", "ChooseID");
            return View(typeWatch);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(TypeWatch typeWatch)
        {
            try
            {
                db.Entry(typeWatch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (TypeWatch)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.the typewatch was deleted by another user.");
                }
                else
                {
                    var databaseValues = (TypeWatch)databaseEntry.ToObject();
                    if (databaseValues.Title != clientValues.Title)
                        ModelState.AddModelError("Title", "Current Value:" + databaseValues.Title);
                    if (databaseValues.Image != clientValues.Image)
                        ModelState.AddModelError("Image", "Current Value:" + databaseValues.Image);
                    if (databaseValues.ChooseID != clientValues.ChooseID)
                        ModelState.AddModelError("ChooseID", "Current Value:" + db.Chooses.Find(databaseValues.ChooseID));
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit " + "was modified by another user after you got the original value. The" + "edit operation was canceled and the current values in the database " + "have been displayed. If you still want to edit this record, click " + "the Save button again. Otherwise click the Back to List hyperlink.");
                    typeWatch.RowVersion = databaseValues.RowVersion;
                }
                
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            ViewBag.ChooseID = new SelectList(db.Chooses, "ChooseID", "ChooseID", typeWatch.ChooseID);
            return View(typeWatch);

        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeWatch typeWatch = db.TypesWatches.Find(id);
            if (typeWatch == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (typeWatch==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }
            }
            return View(typeWatch);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(TypeWatch typeWatch)
        {
            try
            {
                db.Entry(typeWatch).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {

                return RedirectToAction("Delete", new { concurrencyError = true, id = typeWatch.TypeWatchID });
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete.Try again,and if the problem persists contact your system administrator.");
            }
            return View(typeWatch);
        }
    }
}