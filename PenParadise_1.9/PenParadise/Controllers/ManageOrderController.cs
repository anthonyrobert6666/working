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
    public class ManageOrderController : Controller
    {
        private PenStoreEntities db = new PenStoreEntities();

        // GET: /ManageOrder/
        public ActionResult Index()
        {
            if (Session["UserName"] != null)
            {
                var orders = db.Orders.Include(o => o.User);
                return View(orders.ToList());
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: /ManageOrder/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["UserName"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Order order = db.Orders.Find(id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                return View(order);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: /ManageOrder/Create
        public ActionResult Create()
        {
            if (Session["UserName"] != null)
            {
                ViewBag.UserNameID = new SelectList(db.Users, "UserNameID", "UserName");
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: /ManageOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,UserNameID,OrderDate,Total,DeliveryAddress,PhoneContact")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserNameID = new SelectList(db.Users, "UserNameID", "UserName", order.UserNameID);
            return View(order);
        }

        // GET: /ManageOrder/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserName"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Order order = db.Orders.Find(id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                ViewBag.UserNameID = new SelectList(db.Users, "UserNameID", "UserName", order.UserNameID);
                return View(order);
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: /ManageOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,UserNameID,OrderDate,Total,DeliveryAddress,PhoneContact")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserNameID = new SelectList(db.Users, "UserNameID", "UserName", order.UserNameID);
            return View(order);
        }

        // GET: /ManageOrder/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["UserName"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Order order = db.Orders.Find(id);
                if (order == null)
                {
                    return HttpNotFound();
                }
                return View(order);
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: /ManageOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
