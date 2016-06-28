﻿using Microsoft.AspNet.Identity;
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
            //For sorting Ads
            ViewBag.HeadlineSortParam = sortOrder == "Headline" ? "headline_desc" : "Headline";
            ViewBag.DescriptionSortParam = sortOrder == "Description" ? "description_desc" : "Description";
            ViewBag.PriceSortParam = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.DateSortParam = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";

            //Get List of Ads from db
            var ads = from a in db.Ads
                      select a;
            
            
            //Determine if Ad is older than 20 days
            foreach(var a in ads)
            {
                if((DateTime.Now - a.DatePosted).Days >= 20 || a.Active == false)
                {
                    a.Active = false;
                }
                else
                {
                    a.Active = true;
                }
            }

            //For searching through Ads
            if (!String.IsNullOrEmpty(searchString))
            {
                ads = ads.Where(a => a.Headline.Contains(searchString) ||
                a.Description.Contains(searchString));
            }

            //For sorting
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
                    ads = ads.OrderBy(a => a.DatePosted);
                    break;
                default:
                    ads = ads.OrderByDescending(a => a.DatePosted);
                    break;
                
            }

            ViewBag.UserId = User.Identity.GetUserId();
            return View(ads.ToList());
        }

        public ActionResult Rules()
        {
            return View();
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
        [Authorize]
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
                ad.DatePosted = DateTime.Now;
                ad.Active = true;

                db.Ads.Add(ad);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ad);
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
