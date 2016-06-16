using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class CurrentUserAdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CurrentUserAds
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.HeadlineSortParam = sortOrder == "Headline" ? "headline_desc" : "Headline";
            ViewBag.DescriptionSortParam = sortOrder == "Description" ? "description_desc" : "Description";
            ViewBag.PriceSortParam = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.ApprovedSortParam = String.IsNullOrEmpty(sortOrder) ? "approved_desc" : "";

            var ads = from a in db.Ads
                      select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                ads = ads.Where(a => a.Headline.Contains(searchString) || a.Description.Contains(searchString));
            }

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
                    ads = ads.OrderByDescending(a => a.Approved);
                    break;
                default:
                    ads = ads.OrderBy(a => a.Approved);
                    break;

            }

            ViewBag.UserId = User.Identity.GetUserId();
            return View(ads.ToList());
        }


        // GET: CurrentUserAds/Edit/5
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

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(db));
            ApplicationUser currentUser = userManager.FindById(ad.UserId);
            TempData["UserId"] = ad.UserId;
            TempData["AdStatus"] = ad.Approved;
            TempData["DatePosted"] = ad.DatePosted;
            return View(ad);
        }

        // POST: CurrentUserAds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Headline,Description,Price,Approved,DatePosted,UserId")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                ad.Approved = (bool)TempData["AdStatus"];
                ad.UserId = TempData["UserId"].ToString();
                ad.DatePosted = (DateTime)TempData["DatePosted"];

                db.Entry(ad).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ad);
        }

        // GET: CurrentUserAds/Delete/5
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

        // POST: CurrentUserAds/Delete/5
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
