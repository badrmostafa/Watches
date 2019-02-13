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
    public class ProductsController : Controller
    {
        private WatchContext db = new WatchContext();
        // GET: Products
        public ActionResult Index(string sort, string search, string filter, int? page)
        {
            ViewBag.sort = sort;
            ViewBag.Title = string.IsNullOrEmpty(sort) ? "title_desc" : "";
            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = filter;
            }
            ViewBag.filter = search;
            var products = db.Products.Include(p => p.Feature).Include(p => p.TypeWatch);
            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Feature.Title.ToUpper().Contains(search.ToUpper()) ||
                    p.TypeWatch.Title.ToUpper().Contains(search.ToUpper()));
            }
            switch (sort)
            {
                case "title_desc":
                    products = products.OrderByDescending(p => p.Feature.Title);
                    break;
                default:
                    products = products.OrderBy(p => p.Feature.Title);
                    break;
            }
            int PageNumber = (page ?? 1);
            int PageSize = 3;
            return View(products.ToPagedList(PageNumber,PageSize));
        }
        //Get Details
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Product product = db.Products.Find(id);
            if (product==null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        //Get Create
        public ActionResult Create()
        {
            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureID");
            ViewBag.TypeWatchID = new SelectList(db.TypesWatches, "TypeWatchID", "TypeWatchID");
            return View();
        }
        //Post Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureID",product.FeatureID);
            ViewBag.TypeWatchID = new SelectList(db.TypesWatches, "TypeWatchID", "TypeWatchID",product.TypeWatchID);
            return View(product);
        }
        //Get Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureID");
            ViewBag.TypeWatchID = new SelectList(db.TypesWatches, "TypeWatchID", "TypeWatchID");
            return View(product);
        }
        //Post Edit
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            try
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var entry = ex.Entries.Single();
                var clientValues = (Product)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();
                if (databaseEntry==null)
                {
                    ModelState.AddModelError(string.Empty, "Unable to save changes.The feature was deleted by another  user.");

                }
                else
                {
                    var databaseValues = (Product)databaseEntry.ToObject();
                    if(databaseValues.FeatureID!=clientValues.FeatureID)
                        ModelState.AddModelError("FeatureID","Current Value:"+db.Features.Find(databaseValues.FeatureID));
                    if (databaseValues.TypeWatchID != clientValues.TypeWatchID)
                        ModelState.AddModelError("TypeWatchID", "Current Value:" + db.TypesWatches.Find(databaseValues.TypeWatchID));
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit " + "was modified by another user after you got the original value. The" + "edit operation was canceled and the current values in the database " + "have been displayed. If you still want to edit this record, click " + "the Save button again. Otherwise click the Back to List hyperlink.");
                    product.RowVersion = databaseValues.RowVersion;
                }

            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again, and if the problem persists contact your system administrator.");
            }
              
            
            ViewBag.FeatureID = new SelectList(db.Features, "FeatureID", "FeatureID", product.FeatureID);
            ViewBag.TypeWatchID = new SelectList(db.TypesWatches, "TypeWatchID", "TypeWatchID", product.TypeWatchID);
            return View(product);
        }
        //Get Delete
        public ActionResult Delete(int? id,bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Conflict);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                if (concurrencyError==true)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return HttpNotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                if (product==null)
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was deleted by another user after you got the original values. " + "Click the Back to List hyperlink.";
                }
                else
                {
                    ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete " + "was modified by another user after you got the original values. " + "The delete operation was canceled and the current values in the " + "database have been displayed. If you still want to delete this " + "record, click the Delete button again. Otherwise " + "click the Back to List hyperlink.";
                }
            }
            return View(product);
        }
        //Post Delete
        [HttpPost]
        public ActionResult Delete(Product product)
        {
            try
            {
                db.Entry(product).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = product.ProductID });
                
            }
            catch(DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to delete.Try again,and if the problem persists contact your system administrator.");
            }
            return View(product);
        }
    }
}