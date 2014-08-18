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
    public class ManageOrderDetailController : Controller
    {
        private PenStoreEntities db = new PenStoreEntities();

        // GET: /ManageOrderDetail/
        //public ActionResult Index()
        //{
        //    var orderdetails = db.OrderDetails.Include(o => o.Order).Include(o => o.Product);
        //    return View(orderdetails.ToList());
        //}

        // GET: /ManageOrderDetail/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            if (orderdetail == null)
            {
                return HttpNotFound();
            }
            return View(orderdetail);
        }

        // GET: /ManageOrderDetail/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "UserNameID");
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "CategoryID");
            return View();
        }

        // POST: /ManageOrderDetail/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="OrderDetailID,OrderID,ProductID,Quantity,UnitPrice")] OrderDetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                db.OrderDetails.Add(orderdetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "UserNameID", orderdetail.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "CategoryID", orderdetail.ProductID);
            return View(orderdetail);
        }

        // GET: /ManageOrderDetail/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            if (orderdetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "UserNameID", orderdetail.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "CategoryID", orderdetail.ProductID);
            return View(orderdetail);
        }

        // POST: /ManageOrderDetail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="OrderDetailID,OrderID,ProductID,Quantity,UnitPrice")] OrderDetail orderdetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderdetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "UserNameID", orderdetail.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "CategoryID", orderdetail.ProductID);
            return View(orderdetail);
        }

        // GET: /ManageOrderDetail/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            if (orderdetail == null)
            {
                return HttpNotFound();
            }
            return View(orderdetail);
        }

        // POST: /ManageOrderDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            OrderDetail orderdetail = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderdetail);
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
