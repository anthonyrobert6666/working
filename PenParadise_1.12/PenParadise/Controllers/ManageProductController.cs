using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PenParadise.Models;
using System.IO;

namespace PenParadise.Controllers
{
    public class ManageProductController : Controller
    {
        private PenStoreEntities db = new PenStoreEntities();

        // GET: /ManageProduct/
        public ActionResult Index()
        {
            if (Convert.ToString(Session["UserName"]) == "admin")
            {
                var products = db.Products.Include(p => p.Branch).Include(p => p.Category);
                return View(products.ToList());
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: /ManageProduct/Details/5
        public ActionResult Details(string id)
        {
            if (Convert.ToString(Session["UserName"]) == "admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: /ManageProduct/Create
        public ActionResult Create()
        {
            if (Convert.ToString(Session["UserName"]) == "admin")
            {
                ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName");
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
                return View();
            }
            return RedirectToAction("Login", "Account");
        }


        // POST: /ManageProduct/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,CategoryID,ProductName,BranchID,Description,Price,Quantity")] Product product, HttpPostedFileBase File)
        {
            if (ModelState.IsValid)
            {
                product.images = UploadImage(File);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName", product.BranchID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        public string UploadImage(HttpPostedFileBase File)
        {
            string filename = "";
            if (File != null && File.ContentLength > 0)
            {
                filename = Path.GetFileName(File.FileName);
                string url = Path.Combine(Server.MapPath("~/Images"), filename);
                File.SaveAs(url);
            }
            return filename;
        }


        // GET: /ManageProduct/Edit/5
        public ActionResult Edit(string id)
        {
            if (Convert.ToString(Session["UserName"]) == "admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName", product.BranchID);
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
                return View(product);
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: /ManageProduct/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CategoryID,ProductName,BranchID,Description,Price,Quantity,images")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BranchID = new SelectList(db.Branches, "BranchID", "BranchName", product.BranchID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: /ManageProduct/Delete/5
        public ActionResult Delete(string id)
        {
            if (Convert.ToString(Session["UserName"]) == "admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Product product = db.Products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: /ManageProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
