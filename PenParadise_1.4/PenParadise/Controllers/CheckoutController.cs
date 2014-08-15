using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Security;
using PenParadise.Models;
using PenParadise.ViewModels;
namespace PenParadise.Controllers
{
    public class CheckoutController : Controller
    {
        PenStoreEntities _db;

        //
        // GET: /Checkout/
        PenStoreEntities storeDB = new PenStoreEntities();
        const string PromoCode = "FREE";
        //
        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");

        }
        //[HttpPost]
        //public ActionResult AddressAndPayment(FormCollection values)
        //{
        //    if (Session["UserName"] != null)
        //    {
        //        return View();
        //    }
        //    return RedirectToAction("Login", "Account");

        //}
        //
        //POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var PhoneContact = values["PhoneContact"];
            var DeliveryAddress = values["DeliveryAddress"];
            var autoID = (from q in _db.Orders
                          select q).Count();
            var autoIDdetail = (from q in _db.OrderDetails
                          select q).Count();
            Order or = new Order();
            or.PhoneContact = PhoneContact;
            or.DeliveryAddress = DeliveryAddress;
            or.OrderID = Convert.ToString(autoID+1);
            or.UserNameID = Convert.ToString(Session["UserName"]);
            or.OrderDate = DateTime.Now;
            var cart = ShoppingCart.GetCart(_db, this.HttpContext);
            or.Total = Convert.ToDouble(cart.GetTotal());
            _db.Orders.Add(or);
            _db.SaveChanges();
            cart.CreateOrder(or);
            return RedirectToAction("Complete",
                       new { id = or.OrderID });


           

            






            //var order = new Order();
            //TryUpdateModel(order);
            //string a = values["PromoCode"];



            //try
            //{
            //    if (string.Equals(values["PromoCode"], PromoCode,
            //        StringComparison.OrdinalIgnoreCase) == false)
            //    {
            //        return View(order);
            //    }
            //    else
            //    {
            //        order.Username = User.Identity.Name;
            //        order.OrderDate = DateTime.Now;

            //        //Save Order
            //        storeDB.Orders.Add(order);
            //        storeDB.SaveChanges();
            //        //Process the order
            //        var cart = ShoppingCart.GetCart(this.HttpContext);
            //        cart.CreateOrder(order);

            //        return RedirectToAction("Complete",
            //            new { id = order.OrderId });
            //    }
            //}
            //catch
            //{
            //    //Invalid - redisplay with errors
            //    return View(order);
            //}
        }


        // GET: /Checkout/Complete

        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderID == Convert.ToString(id));

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}