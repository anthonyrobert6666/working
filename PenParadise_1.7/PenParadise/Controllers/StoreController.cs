using PenParadise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PenParadise.Controllers
{
    public class StoreController : Controller
    {
        private PenStoreEntities db = new PenStoreEntities();
        //
        // GET: /Store/
        public ActionResult Index()
        {
            var Pr = (from s in db.Products
                      orderby s.Price descending
                      select s).Take(3);
            return View(Pr);
        }
        //public ViewResult Index(string sortOrder, string searchString)
        //{
        //    var students = from s in db.Students
        //                   select s;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
        //                               || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
        //    }
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            students = students.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Date":
        //            students = students.OrderBy(s => s.EnrollmentDate);
        //            break;
        //        case "date_desc":
        //            students = students.OrderByDescending(s => s.EnrollmentDate);
        //            break;
        //        default:
        //            students = students.OrderBy(s => s.LastName);
        //            break;
        //    }

        //    return View(students.ToList());
        //}
        // GET: /Product/
        public ActionResult AllProduct()
        {
            var Pr = (from s in db.Products
                      select s);
            return View(Pr);
        }
        // GET: /Store/CategoryList
        [ChildActionOnly]
        public ActionResult CategoryList()
        {
            var Cat = db.Categories.ToList();
            return View(Cat);
        }
        public ActionResult ListByCategory(string id)
        {
            var Pr = (from s in db.Products
                      where s.CategoryID == id
                      select s);
            return View(Pr);
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Pr = (from s in db.Products
                      where s.ProductID==id
                      select s);
            
            return View(Pr);
        }

	}
}