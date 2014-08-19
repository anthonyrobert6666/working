using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PenParadise.Models;

namespace PenParadise.Controllers
{
    public class ManageProductTypeController : Controller
    {
        private PenStoreEntities db = new PenStoreEntities();

        // GET: /ManageProductType/
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                return View(db.ProductTypes.ToList());
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: /ManageProductType/Details/5
        public ActionResult Details(string id)
        {
            if (Session["UserName"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ProductType producttype = db.ProductTypes.Find(id);
                if (producttype == null)
                {
                    return HttpNotFound();
                }
                return View(producttype);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: /ManageProductType/Create
        public ActionResult Create()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: /ManageProductType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProducTypeID,ProductTypeName")] ProductType producttype)
        {
            if (ModelState.IsValid)
            {
                db.ProductTypes.Add(producttype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producttype);
        }

        // GET: /ManageProductType/Edit/5
        public ActionResult Edit(string id)
        {
            if (Session["UserName"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ProductType producttype = db.ProductTypes.Find(id);
                if (producttype == null)
                {
                    return HttpNotFound();
                }
                return View(producttype);
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: /ManageProductType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProducTypeID,ProductTypeName")] ProductType producttype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producttype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producttype);
        }

        // GET: /ManageProductType/Delete/5
        public ActionResult Delete(string id)
        {
            if (Session["UserName"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ProductType producttype = db.ProductTypes.Find(id);
                if (producttype == null)
                {
                    return HttpNotFound();
                }
                return View(producttype);
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: /ManageProductType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ProductType producttype = db.ProductTypes.Find(id);
            db.ProductTypes.Remove(producttype);
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
