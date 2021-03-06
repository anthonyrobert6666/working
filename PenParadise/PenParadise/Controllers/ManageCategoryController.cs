﻿using System;
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
    public class ManageCategoryController : Controller
    {
        private PenStoreEntities db = new PenStoreEntities();

        // GET: /ManageCategory/
        public ActionResult Index()
        {
            var categories = db.Categories.Include(c => c.ProductType);
            return View(categories.ToList());
        }

        // GET: /ManageCategory/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: /ManageCategory/Create
        public ActionResult Create()
        {
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProducTypeID", "ProductTypeName");
            return View();
        }

        // POST: /ManageCategory/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CategoryID,CategoryName,ProductTypeID")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProducTypeID", "ProductTypeName", category.ProductTypeID);
            return View(category);
        }

        // GET: /ManageCategory/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProducTypeID", "ProductTypeName", category.ProductTypeID);
            return View(category);
        }

        // POST: /ManageCategory/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CategoryID,CategoryName,ProductTypeID")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductTypeID = new SelectList(db.ProductTypes, "ProducTypeID", "ProductTypeName", category.ProductTypeID);
            return View(category);
        }

        // GET: /ManageCategory/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: /ManageCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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
