using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Watches.Models.Classes;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Data;
using PagedList;

namespace Watches.Controllers
{
    public class ReviewsController : Controller
    {
        private WatchContext db = new WatchContext();
        // GET: Reviews
        public ActionResult Index(string sort,string search,string filter,int? page)
        {
            ViewBag.sort = sort;
            ViewBag.Name = string.IsNullOrEmpty(sort) ? "name_desc" : "";
            if (search!=null)
            {
                page = 1;
            }
            else
            {
                search = filter;
            }
            ViewBag.filter = search;
            var reviews = from r in db.Reviews select r;
            if (!string.IsNullOrEmpty(search))
            {
                reviews = reviews.Where(r => r.Title.ToUpper().Contains(search.ToUpper()) ||
                  r.Name.ToUpper().Contains(search.ToUpper()) ||
                  r.City.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "name_desc":
                    reviews = reviews.OrderByDescending(r => r.Name);
                    break;
                default:
                    reviews = reviews.OrderBy(r => r.Name);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(reviews.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Review review = db.Reviews.Find(id);
            if (review==null)
            {
                return HttpNotFound();
            }
            return View(review);
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Review review)
        {
            if (ModelState.IsValid)
            {
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(review);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Review review)
        {
            try
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Review)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.the review was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Review)databaseEntry.ToObject();
                    if (databaseValues.Title != clientValues.Title)
                        ModelState.AddModelError("Title", "Current Value:" + databaseValues.Title);
                    if (databaseValues.Description != clientValues.Description)
                        ModelState.AddModelError("Description", "Current Value:" + databaseValues.Description);
                    if (databaseValues.Name != clientValues.Name)
                        ModelState.AddModelError("Head1", "Current Value:" + databaseValues.Name);
                    if (databaseValues.City != clientValues.City)
                        ModelState.AddModelError("Head2", "Current Value:" + databaseValues.City);
                    if (databaseValues.Image != clientValues.Image)
                        ModelState.AddModelError("Image", "Current Value:" + databaseValues.Image);
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit " + "was modified by another user after you got the original value. The" + "edit operation was canceled and the current values in the database " + "have been displayed. If you still want to edit this record, click " + "the Save button again. Otherwise click the Back to List hyperlink.");
                    review.RowVersion = databaseValues.RowVersion;
                }
                
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
            return View(review);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Review review = db.Reviews.Find(id);
            if (review == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Conflict);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (review==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }
            }
            return View(review);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(Review review)
        {
            try
            {
                db.Entry(review).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = review.ReviewID });
                
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete.Try again,and if the problem persists contact your system administrator.");

            }
            return View(review);
        }






























    }
}