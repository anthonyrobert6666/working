using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using PenParadise.Models;
using System.Web.Security;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.Data.Entity;
namespace PenParadise.Controllers
{
    public class AccountController : Controller
    {
        PenStoreEntities db = new PenStoreEntities();
        //public AccountController()
        //    : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        //{
        //}

        //public AccountController(UserManager<ApplicationUser> userManager)
        //{
        //    UserManager = userManager;
        //}

        //public UserManager<ApplicationUser> UserManager { get; private set; }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string returnUrl)
        {
            PenStoreEntities penstore = new PenStoreEntities();
            string haspas = GetMD5HashData(model.Password);
            bool user = penstore.Users.Any(u => u.UserName == model.UserName && u.Password == haspas);
            bool role = penstore.Users.Any(u => u.UserName == model.UserName && u.Role == "EndUser");
            if (user)
            {
                if (role)
                {
                    Session["UserName"] = model.UserName;
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    Session["UserName"] = "admin";
                    return RedirectToAction("Index", "ManageUser");
                }

            }

            else
            {
                ModelState.AddModelError("", "Invalid username or password.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register userInfo)
        {
            PenStoreEntities db = new PenStoreEntities();
            bool user = db.Users.Any(u => u.UserName == userInfo.UserName);
            if (!user)
            {
                User us = new User();
                int count = (from p in db.Users
                             select p).Count();
                count++;
                us.UserNameID = ("U0" + count).ToString();
                us.Role = "EndUser";
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
                Session["UserName"] = us.UserName;
                FormsAuthentication.SetAuthCookie(us.UserName, false);
                return RedirectToAction("Index", "Store");
            }
            else
            {
                ViewData["Error"] = "UserName is exist";
            }
            return View();
        }
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
        //
        // POST: /Account/Disassociate
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        //{
        //    ManageMessageId? message = null;
        //    IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
        //    if (result.Succeeded)
        //    {
        //        message = ManageMessageId.RemoveLoginSuccess;
        //    }
        //    else
        //    {
        //        message = ManageMessageId.Error;
        //    }
        //    return RedirectToAction("Manage", new { Message = message });
        //}

        //
        // GET: /Account/Manage
        public ActionResult Manage()
        {
            if (Session["UserName"] != null)
            {
                PenStoreEntities db = new PenStoreEntities();
                string sessionname = Session["UserName"].ToString();

                var userid = db.Users.SingleOrDefault(t => t.UserName == sessionname).UserNameID;
                User user = db.Users.Find(userid);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                return View(user);
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "UserNameID,UserName,Password,Role,FullName,Birthday,Email,Address,Phone")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Store");
            }
            return View(user);
        }
        // GET: /Account/ChangePasswordPartial
        public ActionResult ChangePasswordPartial()
        {
            if (Session["UserName"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePasswordPartial(Changepassword ManageInfo)
        {
            if (ManageInfo.OldPassword != null)
            {
                if (ManageInfo.Password != null && ManageInfo.Password.Length >= 6)
                {
                    if (ManageInfo.Password == ManageInfo.ConfirmPassword)
                    {
                        string sessionname = Session["UserName"].ToString();
                        var userid = db.Users.SingleOrDefault(t => t.UserName == sessionname).UserNameID;
                        User us = db.Users.Find(userid);
                        string haspas = GetMD5HashData(ManageInfo.OldPassword);
                        if (us.Password == haspas)
                        {
                            string newpassword = GetMD5HashData(ManageInfo.Password);
                            us.Password = newpassword;
                            db.Entry(us).State = EntityState.Modified;
                            db.SaveChanges();
                            return RedirectToAction("Complete", "Account");

                        }
                        else
                        {
                            ModelState.AddModelError("", "Current password wrong! try again!");
                        }
                    }
                }
            }
            return View();
        }


        public ActionResult Complete()
        {
            return View();
        }
        //public JsonResult OrderCustomer()
        //{
        //      string sessionname = Session["UserName"].ToString();

        //        var userid = db.Users.SingleOrDefault(t => t.UserName == sessionname).UserNameID;
        //    var result = from r in db.Orders
        //                 join p in db.OrderDetails on r.OrderID equals p.OrderID
        //                 join d in db.Products on p.ProductID equals d.ProductID
        //                 where r.UserNameID==userid
        //                 select new {d.ProductID,d.ProductName,p.Quantity,p.UnitPrice,r.OrderDate,r.DeliveryAddress,r.PhoneContact};
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult OrderCustomer()
        {
            string sessionname = Session["UserName"].ToString();

            var userid = db.Users.SingleOrDefault(t => t.UserName == sessionname).UserNameID;
            var result = from r in db.Orders
                         join p in db.OrderDetails on r.OrderID equals p.OrderID
                         join d in db.Products on p.ProductID equals d.ProductID
                         where r.UserNameID == userid
                         select new { d.ProductID, d.ProductName, p.Quantity, p.UnitPrice, r.OrderDate, r.DeliveryAddress, r.PhoneContact };
          //  return Json(result, JsonRequestBehavior.AllowGet);
            List<Order> list = db.Orders.Where(x => x.UserNameID == userid).ToList();
            return View(list);
        }
        public ActionResult Detail_Orders(int id)
        {
            List<OrderDetail> list = db.OrderDetails.Where(x => x.OrderID == id).ToList();
            return View(list);
                    
        }
        //
        // POST: /Account/Manage
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Manage(ManageUserViewModel model)
        //{
        //    bool hasPassword = HasPassword();
        //    ViewBag.HasLocalPassword = hasPassword;
        //    ViewBag.ReturnUrl = Url.Action("Manage");
        //    if (hasPassword)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
        //            }
        //            else
        //            {
        //                AddErrors(result);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        // User does not have a password so remove any validation errors caused by a missing OldPassword field
        //        ModelState state = ModelState["OldPassword"];
        //        if (state != null)
        //        {
        //            state.Errors.Clear();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
        //            if (result.Succeeded)
        //            {
        //                return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
        //            }
        //            else
        //            {
        //                AddErrors(result);
        //            }
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //
        // POST: /Account/ExternalLogin
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public ActionResult ExternalLogin(string provider, string returnUrl)
        //{
        //    // Request a redirect to the external login provider
        //    return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        //}

        //
        // GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var user = await UserManager.FindAsync(loginInfo.Login);
        //    if (user != null)
        //    {
        //        await SignInAsync(user, isPersistent: false);
        //        return RedirectToLocal(returnUrl);
        //    }
        //    else
        //    {
        //        // If the user does not have an account, then prompt the user to create an account
        //        ViewBag.ReturnUrl = returnUrl;
        //        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //        return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
        //    }
        //}

        //
        // POST: /Account/LinkLogin
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LinkLogin(string provider)
        //{
        //    // Request a redirect to the external login provider to link a login for the current user
        //    return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        //}

        //
        // GET: /Account/LinkLoginCallback
        //public async Task<ActionResult> LinkLoginCallback()
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        //    }
        //    var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("Manage");
        //    }
        //    return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        //}

        //
        // POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser() { UserName = model.UserName };
        //        var result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInAsync(user, isPersistent: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        //
        // POST: /Account/LogOff
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var cart = ShoppingCart.GetCart(db, this.HttpContext);
            decimal carttotal = cart.GetTotal();
            double total = Convert.ToDouble(carttotal);
            if (total != 0)
            {
                return RedirectToAction("Checkcart", "Account");
            }
            Session.RemoveAll();
            return RedirectToAction("Index", "Store");

        }

        // GET: /Account/Checkcart
        public ActionResult Checkcart()
        {
            return View();
        }

        //
        // Post: /Account/LogoffComplete
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult LogoffComplete()
        {
            var cart = ShoppingCart.GetCart(db, this.HttpContext);
            cart.EmptyCart();
            Session.RemoveAll();
            return RedirectToAction("Index", "Store");

        }
        //
        // GET: /Account/ExternalLoginFailure
        //[AllowAnonymous]
        //public ActionResult ExternalLoginFailure()
        //{
        //    return View();
        //}

        //[ChildActionOnly]
        //public ActionResult RemoveAccountList()
        //{
        //    var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
        //    ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
        //    return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && UserManager != null)
        //    {
        //        UserManager.Dispose();
        //        UserManager = null;
        //    }
        //    base.Dispose(disposing);
        //}

        //#region Helpers
        //// Used for XSRF protection when adding external logins
        //private const string XsrfKey = "XsrfId";

        //private IAuthenticationManager AuthenticationManager
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Authentication;
        //    }
        //}

        //private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        //{
        //    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //    var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
        //    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        //}

        //private void AddErrors(IdentityResult result)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        ModelState.AddModelError("", error);
        //    }
        //}

        //private bool HasPassword()
        //{
        //    var user = UserManager.FindById(User.Identity.GetUserId());
        //    if (user != null)
        //    {
        //        return user.PasswordHash != null;
        //    }
        //    return false;
        //}

        //public enum ManageMessageId
        //{
        //    ChangePasswordSuccess,
        //    SetPasswordSuccess,
        //    RemoveLoginSuccess,
        //    Error
        //}

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Store");
            }
        }

        //private class ChallengeResult : HttpUnauthorizedResult
        //{
        //    public ChallengeResult(string provider, string redirectUri)
        //        : this(provider, redirectUri, null)
        //    {
        //    }

        //    public ChallengeResult(string provider, string redirectUri, string userId)
        //    {
        //        LoginProvider = provider;
        //        RedirectUri = redirectUri;
        //        UserId = userId;
        //    }

        //    public string LoginProvider { get; set; }
        //    public string RedirectUri { get; set; }
        //    public string UserId { get; set; }

        //    public override void ExecuteResult(ControllerContext context)
        //    {
        //        var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
        //        if (UserId != null)
        //        {
        //            properties.Dictionary[XsrfKey] = UserId;
        //        }
        //        context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        //    }
        //}
        //#endregion
        [ChildActionOnly]
        public ActionResult CategoryList()
        {
            var Cat = db.Categories.ToList();
            return View(Cat);
        }
    }
}