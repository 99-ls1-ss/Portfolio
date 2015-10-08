using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Portfolio.Models;
using MyPersonalPage.Models;
using System.IO;

namespace Portfolio.Controllers {
    public class BlogController : Controller {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Blog
        public ActionResult Index() {
            return View(db.Posts.ToList());
        }

        // GET: Blog/Details/
        public ActionResult Details(string Slug) {

            if (String.IsNullOrWhiteSpace(Slug)) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlogPost blogPost = db.Posts.FirstOrDefault(p => p.Slug == Slug);

            if (blogPost == null) {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // GET: Blog/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create() {
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Created,Updated,Title,Slug,Body,MediaURL,IsPublished,IsLoggedIn,AccessLevel")] BlogPost blogPost, HttpPostedFileBase image) {
            if (image != null && image.ContentLength > 0) {
                //Check the file name to make sure it's a image
                var ext = Path.GetExtension(image.FileName).ToLower();

                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                    ModelState.AddModelError("image", "Invalid Image Type.");
            }

            if (ModelState.IsValid) {

                var Slug = StringUtilities.URLFriendly(blogPost.Title);
                if (String.IsNullOrWhiteSpace(Slug)) {
                    ModelState.AddModelError("Title", "Invalid Title.");
                    return View(blogPost);
                }
                if (db.Posts.Any(p => p.Slug == Slug)) {
                    ModelState.AddModelError("Title", "Your Title must be unique.");
                    return View(blogPost);
                }                

                if (image != null) {
                    //relative server path
                    var filePath = "/Uploads/";

                    //Path on physical drive on server
                    var absPath = Server.MapPath("~" + filePath);

                    //Media URL for relative path
                    blogPost.MediaURL = filePath + image.FileName;

                    //Save image
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }               

                blogPost.Slug = Slug;
                db.Posts.Add(blogPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        // GET: Blog/Edit/
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null) {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: Blog/Edit/
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Created,Updated,Title,Slug,Body,MediaURL,IsPublished,IsLoggedIn,AccessLevel")] BlogPost blogPost, HttpPostedFileBase image) {
            if (image != null && image.ContentLength > 0) {
                //Check the file name to make sure it's a image
                var ext = Path.GetExtension(image.FileName).ToLower();

                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                    ModelState.AddModelError("image", "Invalid Image Type.");
            }

            if (ModelState.IsValid) {

                if (image != null) {
                    //relative server path
                    var filePath = "/Uploads/";

                    //Path on physical drive on server
                    var absPath = Server.MapPath("~" + filePath);

                    //Media URL for relative path
                    blogPost.MediaURL = filePath + image.FileName;

                    //Save image
                    image.SaveAs(Path.Combine(absPath, image.FileName));
                }

                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        // GET: Blog/Delete/
        public ActionResult Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.Posts.Find(id);
            if (blogPost == null) {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: Blog/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            BlogPost blogPost = db.Posts.Find(id);
            db.Posts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
