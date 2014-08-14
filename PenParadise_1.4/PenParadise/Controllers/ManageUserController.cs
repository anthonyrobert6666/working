using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PenParadise.Models;
using System.Security.Cryptography;
using System.Text;
namespace PenParadise.Controllers
{
    public class ManageUserController : Controller
    {
        private PenStoreEntities db = new PenStoreEntities();

        // GET: /ManageUser/
        public ActionResult Index()
        {
            var User = from p in db.Users
                        select p; 
            return View(User);
        }

        // GET: /ManageUser/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: /ManageUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ManageUser/Create
   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User userInfo)
        {
            PenStoreEntities db = new PenStoreEntities();
            bool user = db.Users.Any(u => u.UserName == userInfo.UserName);
            if (!user)
            {

                User us = new User();
                us.Role = userInfo.Role;
                us.UserNameID = userInfo.UserNameID;
                us.UserName = userInfo.UserName;
                String haspas = GetMD5HashData(userInfo.Password);
                us.Password = haspas;
                us.FullName = userInfo.FullName;
                us.Birthday = userInfo.Birthday;
                us.Email = userInfo.Email;
                us.Address = userInfo.Address;
                us.Phone = userInfo.Phone;

                db.Users.Add(us);
                db.SaveChanges();
                return RedirectToAction("Index", "ManageUser");
            }
            else
            {
                ModelState.AddModelError("", "Error");
            }
            return View(user);
        }
        // Hash Password MD5
        private string GetMD5HashData(string data)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();

        }


        // GET: /ManageUser/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /ManageUser/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="UserNameID,UserName,Password,Role,FullName,Birthday,Email,Address,Phone")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: /ManageUser/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /ManageUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
