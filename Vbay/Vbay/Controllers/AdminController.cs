using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vbay.Models;

namespace Vbay.Controllers
{
    [Authorize(Roles = RoleNames.ROLE_ADMIN)]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index(string sortOrder)
        {
            ViewBag.HeadlineSortParam = sortOrder == "Headline" ? "headline_desc" : "Headline";
            ViewBag.DescriptionSortParam = sortOrder == "Description" ? "description_desc" : "Description";
            ViewBag.PriceSortParam = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.ApprovedSortParam = String.IsNullOrEmpty(sortOrder) ? "approved_desc" : "";

            var ads = from a in db.Ads
                      select a;

            switch (sortOrder)
            {
                case "Headline":
                    ads = ads.OrderBy(a => a.Headline);
                    break;
                case "headline_desc":
                    ads = ads.OrderByDescending(a => a.Headline);
                    break;

                case "Description":
                    ads = ads.OrderBy(a => a.Description);
                    break;
                case "description_desc":
                    ads = ads.OrderByDescending(a => a.Description);
                    break;

                case "Price":
                    ads = ads.OrderByDescending(a => a.Price);
                    break;
                case "price_desc":
                    ads = ads.OrderBy(a => a.Price);
                    break;

                case "date_desc":
                    ads = ads.OrderByDescending(a => a.DatePosted);
                    break;
                case "Date":
                    ads = ads.OrderBy(a => a.DatePosted);
                    break;

                case "approved_desc":
                    ads = ads.OrderBy(a => a.Approved);
                    break;
                default:
                    ads = ads.OrderByDescending(a => a.Approved);
                    break;

            }
            return View(ads.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }

       
        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Headline,Description,Price,Approved,DatePosted,UserId")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ad);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ad ad = db.Ads.Find(id);
            db.Ads.Remove(ad);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
