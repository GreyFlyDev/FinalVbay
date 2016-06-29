using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Vbay.Models;

namespace Vbay.Controllers
{
    [Authorize(Roles = RoleNames.ROLE_ADMIN)]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.HeadlineSortParam = sortOrder == "Headline" ? "headline_desc" : "Headline";
            ViewBag.DescriptionSortParam = sortOrder == "Description" ? "description_desc" : "Description";
            ViewBag.PriceSortParam = sortOrder == "Price" ? "price_desc" : "Price";
            ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.ApprovedSortParam = String.IsNullOrEmpty(sortOrder) ? "approved_desc" : "";
            ViewBag.SearchString = searchString;

            

            var ads = from a in db.Ads
                      select a;


            if (!String.IsNullOrEmpty(searchString))
            {
                switch (searchString)
                {
                    case "Pending":
                        ads = ads.Where(a => a.Approved == null);
                        break;
                    case "Approved":
                        ads = ads.Where(a => a.Approved == true);
                        break;
                    case "Denied":
                        ads = ads.Where(a => a.Approved == false);
                        break;
                    case "Active":
                        ads = ads.Where(a => a.Active == true);
                        break;
                    case "Expired":
                        ads = ads.Where(a => a.Active == false);
                        break;
                    default:
                        ads = ads.Where(a => a.Headline.Contains(searchString) || a.Description.Contains(searchString));
                        break;
                }

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
            return View(ads.ToList());
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

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(db));
            ApplicationUser adOwner = userManager.FindById(ad.UserId);

            ViewBag.UserFirstName = adOwner.FirstName;
            ViewBag.UserLastName = adOwner.LastName;
            ViewBag.UserPhone = adOwner.PhoneNumber;
            ViewBag.UserEmail = adOwner.Email;
            ViewBag.AdDescription = ad.Description;
            TempData["UserId"] = ad.UserId;
            TempData["AdDescription"] = ad.Description;
            TempData["AdPrice"] = ad.Price;
            TempData["AdHeadline"] = ad.Headline;
            TempData["AdStatus"] = ad.Approved;

            return View(ad);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Headline,Description,Price,Approved,DatePosted,AdminComments,UserId")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                ad.UserId = TempData["UserId"].ToString();
                ad.Headline = TempData["AdHeadline"].ToString();
                ad.Description = TempData["AdDescription"].ToString();
                ad.Price = (decimal)TempData["AdPrice"];
                ad.DatePosted = DateTime.Now;

                switch (ad.Approved)
                {
                    case true:
                        ad.Active = true;
                        break;

                    case null:
                        ad.Active = true;
                        break;

                    case false:
                        ad.Active = false;
                        break;
                }

                //Email Portion

                var userManager = new UserManager<ApplicationUser>(
                    new UserStore<ApplicationUser>(db));
                var adOwner = userManager.FindById(ad.UserId);

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("gregnnylf94@gmail.com");
                    mail.To.Add(adOwner.Email);
                    mail.Subject = "Ad Status";

                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("gregnnylf94@gmail.com", "Enjoilif3!");
                        smtp.EnableSsl = true;
                        if (TempData["AdStatus"] == null || ad.Approved != (bool)TempData["AdStatus"])
                        {
                            switch (ad.Approved)
                            {
                                case true:
                                    //Send email "Your ad has been Aproved"
                                    mail.Body = "<h1>Approved</h1> Your ad has been approved as of " + DateTime.Now + ". <br/>Additional Comments: <br/>" + HttpUtility.HtmlDecode(ad.AdminComments)+ "<br/> - Your friendly Neighborhood Administrator";
                                    smtp.Send(mail);
                                    break;

                                case false:
                                    //Send email "Your ad has been Denied"
                                    mail.Body = "<h1>Denied</h1> Your ad has been denied as of " + DateTime.Now + ". Your ad was denied because: " + HttpUtility.HtmlDecode(ad.AdminComments) + "<br/>" + "- Your Friendly Neighborhood Administrator";
                                    smtp.Send(mail);
                                    break;
                            }
                        }

                    }
                }


                

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
            ad.Active = false;
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
