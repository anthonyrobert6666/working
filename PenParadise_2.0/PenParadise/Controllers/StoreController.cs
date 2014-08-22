using PenParadise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

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

        // GET: /About/
        public ActionResult About()
        {
            return View();
        }

        // GET: /Product/
        public ActionResult AllProduct(string sortOrder, string currentFilter, string searchString, int? page, string ProType)
        {
            ViewBag.ProType = (from p in db.ProductTypes select p.ProductTypeName ).Distinct();

            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            
            var Pr = from s in db.Products
                     join v in db.Categories on s.CategoryID equals v.CategoryID
                     join k in db.ProductTypes on v.ProductTypeID equals k.ProducTypeID
                     select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                Pr = Pr.Where(s => s.ProductName.ToUpper().Contains(searchString.ToUpper()));
            }
            if (!String.IsNullOrEmpty(ProType))
            {
                Pr = Pr.Where(s => s.Category.ProductType.ProductTypeName == ProType);
            }
            switch (sortOrder)
            {
                default:
                    Pr = Pr.OrderBy(s => s.ProductID);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(Pr.ToPagedList(pageNumber, pageSize));
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