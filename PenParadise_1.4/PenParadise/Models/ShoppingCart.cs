﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PenParadise.Models
{
    public partial class ShoppingCart
    {
        PenStoreEntities _db = new PenStoreEntities();
        string ShoppingCartId { get; set; }

        public ShoppingCart(PenStoreEntities db)
        {
            _db = db;
        }

        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(PenStoreEntities db, HttpContextBase context)
        {
            var cart = new ShoppingCart(db);
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(PenStoreEntities db, Controller controller)
        {
            return GetCart(db, controller.HttpContext);
        }

        public void AddToCart(Product product)
        {
            // Get the matching cart and album instances
            var cartItem = _db.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductID == product.ProductID);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                var autoID = (from q in _db.Carts
                              select q).Count();
                cartItem = new Cart
                {
                    RecordId=autoID+1,
                    ProductID= product.ProductID,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                _db.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }
        }

        public int RemoveFromCart(int id)
        {
            var cartItem = _db.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                //--if (cartItem.Count > 1)
                //--{
                //--    cartItem.Count--;
                //--    itemCount = cartItem.Count;
                //--}
                //--else
                //--{
                _db.Carts.Remove(cartItem);
                //--}
                // Save changes 
                _db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = _db.Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _db.Carts.Remove(cartItem);
            }
            _db.SaveChanges();

        }

        public List<Cart> GetCartItems()
        {
            return _db.Carts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in _db.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();

            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            double? total = (from cartItems in _db.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Product.Price).Sum();
            return Convert.ToDecimal(total);
        }

        public int CreateOrder(Order order)
        {
            var cartItems = GetCartItems();

            // Iterate over the items in the cart, adding the order details for each
            foreach (var item in cartItems)
            {
                var product = _db.Products.Find(item.ProductID);

                var orderDetail = new OrderDetail
                {
                    ProductID = item.ProductID,
                    OrderID = order.OrderID,
                    UnitPrice = product.Price,
                    Quantity = item.Count,
                };

                // Set the order total of the shopping cart
                //orderTotal += Convert.ToDecimal((item.Count * (decimal?)item.Product.Price));

                _db.OrderDetails.Add(orderDetail);
                _db.SaveChanges();
            }

            // Set the order's total to the orderTotal count

            // Empty the shopping cart
            EmptyCart();

            // Return the OrderId as the confirmation number
            return Convert.ToInt32(order.OrderID);
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();

                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }

            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = _db.Carts.Where(c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }

        }
        public int UpdateCartCount(int id, int cartCount)
        {
            // Get the cart 
            var cartItem = _db.Carts.Single(
                cart => cart.CartId == ShoppingCartId
                && cart.RecordId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartCount > 0)
                {
                    cartItem.Count = cartCount;
                    itemCount = Convert.ToInt32(cartItem.Count);
                }
                else
                {
                    _db.Carts.Remove(cartItem);
                }
                // Save changes 
                _db.SaveChanges();
            }
            return itemCount;
        }
    }
}