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
    public class GradesController : Controller
    {
        private WatchContext db = new WatchContext();
        // GET: Grades
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
            var grades = from g in db.Grades select g;
            if (!string.IsNullOrEmpty(search))
            {
                grades = grades.Where(g => g.Head.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "head_desc":
                    grades = grades.OrderByDescending(g => g.Head);
                    break;
                default:
                    grades = grades.OrderBy(g => g.Head);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(grades.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Grade grade = db.Grades.Find(id);
            if (grade==null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }
        //Get Create
        public ActionResult Create()
        {
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Grade grade)
        {
            if (ModelState.IsValid)
            {
                db.Grades.Add(grade);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(grade);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Grade grade = db.Grades.Find(id);
            if (grade == null)
            {
                return HttpNotFound();
            }
            return View(grade);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Grade grade)
        {
            try
            {
                db.Entry(grade).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues =(Grade) entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.the grade was deleted by another user.");
                }
                else
                {
                    var databaseValues = (Grade)databaseEntry.ToObject();
                    if (databaseValues.Image != clientValues.Image)
                        ModelState.AddModelError("Image", "Current Value:" + databaseValues.Image);
                    if (databaseValues.Head != clientValues.Head)
                        ModelState.AddModelError("Head", "Current Value:" + databaseValues.Head);
                    if (databaseValues.Description != clientValues.Description)
                        ModelState.AddModelError("Description1", "Current Value:" + databaseValues.Description);
                 
                    ModelState.AddModelError(string.Empty, "The record you attempted to edit " + "was modified by another user after you got the original value. The" + "edit operation was canceled and the current values in the database " + "have been displayed. If you still want to edit this record, click " + "the Save button again. Otherwise click the Back to List hyperlink.");
                    grade.RowVersion = databaseValues.RowVersion;
                }
                
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes.Try again,and if the problem persists contact your system administrator.");
            }
            return View(grade);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Grade grade = db.Grades.Find(id);
            if (grade == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (grade==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }

            }
            return View(grade);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(Grade grade)
        {
            try
            {
                db.Entry(grade).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = grade.GradeID });
                
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete.Try again,and if the problem persists contact your system administrator.");
            }
            return View(grade);
        }
    }
}