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
    public class ChoosesController : Controller
    {
        private WatchContext db = new WatchContext();
        // GET: Chooses
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
            var chooses = from c in db.Chooses select c;
            if (!string.IsNullOrEmpty(search))
            {
                chooses = chooses.Where(c => c.Title.ToUpper().Contains(search.ToUpper()) ||
                  c.Head.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head_desc":
                    chooses = chooses.OrderByDescending(c => c.Head);
                    break;
                default:
                    chooses = chooses.OrderBy(c => c.Head);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(chooses.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Choose choose = db.Chooses.Find(id);
            if (choose==null)
            {
                return HttpNotFound();
            }
            return View(choose);
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Choose choose)
        {
            if (ModelState.IsValid)
            {
                db.Chooses.Add(choose);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(choose);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Choose choose = db.Chooses.Find(id);
            if (choose == null)
            {
                return HttpNotFound();
            }
            return View(choose);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Choose choose)
        {
            try
            {
                db.Entry(choose).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Choose)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The choose was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Choose)databaseEntry.ToObject();
                    if (databaseValues.Title != clientValues.Title)
                        ModelState.AddModelError("Title", "Current Value:" + databaseValues.Title);
                    if (databaseValues.Image != clientValues.Image)
                        ModelState.AddModelError("Image", "Current Value:" + databaseValues.Image);
                    if (databaseValues.Head != clientValues.Head)
                        ModelState.AddModelError("Head", "Current Value:" + databaseValues.Head);
                    if (databaseValues.Description != clientValues.Description)
                        ModelState.AddModelError("Description", "Current Value:" + databaseValues.Description);
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit " + "was modified by another user after you got the original value. The" + "edit operation was canceled and the current values in the database " + "have been displayed. If you still want to edit this record, click " + "the Save button again. Otherwise click the Back to List hyperlink.");
                    choose.RowVersion = databaseValues.RowVersion;
                }
                
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.Try again,and if the problem persists contact your system administrator.");
            }

            return View(choose);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Choose choose = db.Chooses.Find(id);
            if (choose == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (choose==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }
            }
            return View(choose);

        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(Choose choose)
        {
            try
            {
                db.Entry(choose).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = choose.ChooseID });
               
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete.Try again,and if the problem persists contact your system administrator.");
            }
            return View(choose);
        }
    }
}