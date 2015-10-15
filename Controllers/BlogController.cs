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
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using PagedList;
using PagedList.Mvc;

namespace Portfolio.Controllers {
    [RequireHttps]
    public class BlogController : Controller {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Blog
        public ActionResult Index(int? page, string searchString) {

            int pageSize = 3; // Displays three blog posts per page
            int pageNumber = (page ?? 1);

            var queryPost = db.Posts.AsQueryable();

            if (searchString != null && searchString != "") {                

                string[] searchArray = searchString.Split(' ');

                queryPost= searchArray.SelectMany(r => db.Posts.Where(q => (q.Body.Contains(r)) ||
                            (q.Title.Contains(r)) ||
                            (q.Slug.Contains(r)) ||
                            (q.Comments).Any(c => (c.Body).Contains(r)))).AsQueryable();

                ViewBag.Query = searchString;            
            }

            return View(queryPost.OrderByDescending(m => m.Created).ToPagedList(pageNumber, pageSize));
        }

        // GET: Blog/Details/
        public ActionResult Details(string Slug) {

            if (String.IsNullOrWhiteSpace(Slug)) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            BlogPost blogPost = db.Posts.Include("Comments").FirstOrDefault(p => p.Slug == Slug);            
            ViewBag.CurrentUser = db.Users.Find(User.Identity.GetUserId());
            
            if (blogPost == null) {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // GET: Blog/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create() {
            BlogPost BlogPost = new BlogPost();
            BlogPost.IsPublished = true;
            return View(BlogPost);
        }

        // POST: Blog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Created,Updated,Title,Slug,Body,MediaURL,IsPublished,IsLoggedIn")] BlogPost blogPost, HttpPostedFileBase image) {
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
                blogPost.Created = DateTimeOffset.Now;
                blogPost.Updated = null;
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
        public ActionResult Edit(BlogPost blogPost, HttpPostedFileBase image) {
            if (image != null && image.ContentLength > 0) {
                //Check the file name to make sure it's a image
                var ext = Path.GetExtension(image.FileName).ToLower();

                if (ext != ".png" && ext != ".jpg" && ext != ".jpeg" && ext != ".gif" && ext != ".bmp")
                    ModelState.AddModelError("image", "Invalid Image Type.");
            }

            if (ModelState.IsValid) {

                //The Posts refers to the Model IdentityModels - public DbSet<BlogPost> Posts { get; set; }
                if(!db.Posts.Local.Any(p=>p.Id == blogPost.Id))
                    db.Posts.Attach(blogPost);

                if (image != null) {
                    //relative server path
                    var filePath = "/Uploads/";

                    //Path on physical drive on server
                    var absPath = Server.MapPath("~" + filePath);

                    //Media URL for relative path
                    blogPost.MediaURL = filePath + image.FileName;

                    //Save image
                    image.SaveAs(Path.Combine(absPath, image.FileName));

                    db.Entry(blogPost).Property(p => p.MediaURL).IsModified = true;
                }

                blogPost.Updated = DateTimeOffset.Now;
                db.Entry(blogPost).Property(p => p.Updated).IsModified = true;
                db.Entry(blogPost).Property(p => p.Title).IsModified = true;
                db.Entry(blogPost).Property(p => p.Body).IsModified = true;

                if (db.Entry(blogPost).Property(p => p.Title).IsModified == true) {

                    var Slug = StringUtilities.URLFriendly(blogPost.Title);

                    if (String.IsNullOrWhiteSpace(Slug)) {
                        ModelState.AddModelError("Title", "Invalid Title.");
                        return View(blogPost);
                    }                
                    
                }
                db.Entry(blogPost).Property(p => p.Slug).IsModified = true;
                db.Entry(blogPost).Property(p => p.IsPublished).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        // GET: Blog/Delete/
        //[Authorize(Roles = "Admin", "Moderator")]
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

        [HttpPost]
        public ActionResult AddComment(Comment newComment, string Slug) {
            if (ModelState.IsValid) {
                newComment.CreatedDate = System.DateTimeOffset.Now;
                newComment.IsRemoved = false;
                newComment.RemovedDate = null;
                newComment.RemovedBy = "";
                newComment.AuthorId = User.Identity.GetUserId();
                db.Comments.Add(newComment);
                db.SaveChanges();
            }
            return RedirectToAction(Slug, "Post");
        }


        [HttpPost]
        public ActionResult EditComment(Comment editComment, string Slug) {

            if (ModelState.IsValid) {
                // the Comments refers to the Model IdentityModels - public DbSet<Comment> Comments { get; set; }.
                if (!db.Comments.Local.Any(c => c.Id == editComment.Id))
                    db.Comments.Attach(editComment);

                db.Entry(editComment).Property(p => p.Body).IsModified = true;
                db.Entry(editComment).Property(p => p.UpdateReason).IsModified = true;
                db.Entry(editComment).Property(p => p.UpdatedBy).IsModified = true;
                editComment.UpdatedDate = System.DateTimeOffset.Now;
                db.SaveChanges();
            }
            return RedirectToAction(Slug, "Post");
        }

        [HttpPost]
        public ActionResult DeleteComment(Comment deleteComment, string Slug) {

            if (ModelState.IsValid) {
                // the Comments refers to the Model IdentityModels - public DbSet<Comment> Comments { get; set; }.
                if (!db.Comments.Local.Any(c => c.Id == deleteComment.Id))
                    db.Comments.Attach(deleteComment);
                
                db.Entry(deleteComment).Property(d => d.Body).IsModified = true;                
                db.Entry(deleteComment).Property(d => d.RemoveReason).IsModified = true;
                db.Entry(deleteComment).Property(d => d.RemovedBy).IsModified = true;
                deleteComment.RemovedDate = System.DateTimeOffset.Now;
                //deleteComment.IsRemoved = true;
                db.SaveChanges();
            }
            return RedirectToAction(Slug, "Post");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
