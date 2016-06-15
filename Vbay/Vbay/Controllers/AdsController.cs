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
    public class AdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Ads
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.HeadlineSortParam = sortOrder == "Headline" ? "headline_desc" : "Headline";
            ViewBag.DescriptionSortParam = sortOrder == "Description" ? "description_desc" : "Description";
            ViewBag.PriceSortParam = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";

            var ads = from a in db.Ads
                      select a;

            if (!String.IsNullOrEmpty(searchString))
            {
                ads = ads.Where(a => a.Headline.Contains(searchString) ||
                a.Description.Contains(searchString));
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
                default:
                    ads = ads.OrderBy(a => a.DatePosted);
                    break;
                
            }

            ViewBag.UserId = User.Identity.GetUserId();
            return View(ads.ToList());
        }

        // GET: Ads/Details/5
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

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(db));
            var currentUser = userManager.FindById(ad.UserId);

            ViewBag.UserFirstName = currentUser.FirstName;
            ViewBag.UserLastName = currentUser.LastName;
            ViewBag.UserPhone = currentUser.PhoneNumber;
            ViewBag.UserEmail = currentUser.Email;
            return View(ad);
        }

        // GET: Ads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Headline,Description,Price,Approved,DatePosted,UserId")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                ad.UserId = User.Identity.GetUserId();
                ad.DatePosted = DateTime.Today;

                db.Ads.Add(ad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ad);
        }

        // GET: Ads/Edit/5
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

        // POST: Ads/Edit/5
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

        // GET: Ads/Delete/5
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

        // POST: Ads/Delete/5
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
