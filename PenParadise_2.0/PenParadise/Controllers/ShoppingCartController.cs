using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PenParadise.Models;
using PenParadise.ViewModels;

namespace PenParadise.Controllers
{
    public class ShoppingCartController : Controller
    {
        PenStoreEntities storeDB = new PenStoreEntities();

        //
        // GET: /ShoppingCart/

        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(storeDB, this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            // Return the view
            return View(viewModel);
        }

        //
        // GET: /ShoppingCart/AddToCart/5

        public ActionResult AddToCart(string id)
        {

            // Retrieve the album from the database
            var addedProduct = storeDB.Products
                .Single(pro => pro.ProductID == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(storeDB, this.HttpContext);

            cart.AddToCart(addedProduct);

            storeDB.SaveChanges();

            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Retrieve the current user's shopping cart
            var cart = ShoppingCart.GetCart(storeDB, this.HttpContext);

            // Get the name of the album to display confirmation
            string productname = storeDB.Carts
                .Single(item => item.RecordId == id).Product.ProductName;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            storeDB.SaveChanges();

            string removed = (itemCount > 0) ? " 1 copy of " : string.Empty;

            // Display the confirmation message

            var results = new ShoppingCartRemoveViewModel
            {
                Message = removed + productname +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                DeleteId = id
            };

            return Json(results);
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(storeDB, this.HttpContext);

            var cartItems = cart.GetCartItems()
                .Select(a => a.Product.ProductName)
                .OrderBy(x => x);

            ViewBag.CartCount = cartItems.Count();
            ViewBag.CartSummary = string.Join("\n", cartItems.Distinct());

            return PartialView("CartSummary");
        }
        [HttpPost]
        public ActionResult UpdateCartCount(int id, int cartCount)
        {
            // Get the cart 
            var cart = ShoppingCart.GetCart(storeDB, this.HttpContext);

            // Get the name of the album to display confirmation 
            string albumName = storeDB.Carts
                .Single(item => item.RecordId == id).Product.ProductName;
            int price = Convert.ToInt32(storeDB.Carts.Single(item=> item.RecordId == id).Product.Price);
            // Update the cart count 
            int itemCount = cart.UpdateCartCount(id, cartCount);

            //Prepare messages
            string msg = "The quantity of " + Server.HtmlEncode(albumName) +
                    " has been refreshed in your shopping cart.";
            if (itemCount == 0) msg = Server.HtmlEncode(albumName) +
                    " has been removed from your shopping cart.";
            //
            // Display the confirmation message 
            var results = new ShoppingCartRemoveViewModel
            {
                Message = msg,
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = cartCount * price,
                DeleteId = id
            };
            return Json(results);
        }
        [ChildActionOnly]
        public ActionResult CartTotal()
        {
            var cart = ShoppingCart.GetCart(storeDB, this.HttpContext);

            ViewData["CartTotal"] = cart.GetTotal();
            return PartialView("CartTotal");
        }
        [ChildActionOnly]
        public ActionResult CartNumber()
        {
            var cart = ShoppingCart.GetCart(storeDB, this.HttpContext);

            ViewData["CartNumber"] = cart.GetCount();
            return PartialView("CartNumber");
        }
        //
        [ChildActionOnly]
        public ActionResult CategoryList()
        {
            var Cat = storeDB.Categories.ToList();
            return View(Cat);
        }
        public ActionResult ListByCategory(string id)
        {
            var Pr = (from s in storeDB.Products
                      where s.CategoryID == id
                      select s);
            return View(Pr);
        }
    }
}