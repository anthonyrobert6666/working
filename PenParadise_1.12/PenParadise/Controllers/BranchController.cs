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
    public class BranchController : Controller
    {
        private PenStoreEntities db = new PenStoreEntities();

        // GET: /Branch/
        public ActionResult Index()
        {
            if (Convert.ToString(Session["UserName"]) == "admin")
            {
                return View(db.Branches.ToList());
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: /Branch/Details/5
        public ActionResult Details(string id)
        {
            if (Convert.ToString(Session["UserName"]) == "admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Branch branch = db.Branches.Find(id);
                if (branch == null)
                {
                    return HttpNotFound();
                }
                return View(branch);
            }
            return RedirectToAction("Login", "Account");
        }

        // GET: /Branch/Create
        public ActionResult Create()
        {
            if (Convert.ToString(Session["UserName"]) == "admin")
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: /Branch/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BranchID,BranchName")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                db.Branches.Add(branch);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(branch);
        }

        // GET: /Branch/Edit/5
        public ActionResult Edit(string id)
        {
            if (Convert.ToString(Session["UserName"]) == "admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Branch branch = db.Branches.Find(id);
                if (branch == null)
                {
                    return HttpNotFound();
                }
                return View(branch);
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: /Branch/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BranchID,BranchName")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                db.Entry(branch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(branch);
        }

        // GET: /Branch/Delete/5
        public ActionResult Delete(string id)
        {
            if (Convert.ToString(Session["UserName"]) == "admin")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Branch branch = db.Branches.Find(id);
                if (branch == null)
                {
                    return HttpNotFound();
                }
                return View(branch);
            }
            return RedirectToAction("Login", "Account");
        }

        // POST: /Branch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Branch branch = db.Branches.Find(id);
            db.Branches.Remove(branch);
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
