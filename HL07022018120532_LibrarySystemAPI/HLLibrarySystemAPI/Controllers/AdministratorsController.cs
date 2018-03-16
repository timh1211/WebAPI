using HLLibrarySystemAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HLLibrarySystemAPI.Controllers
{
    public class AdministratorsController : BaseMVCController
    {
        //action login, get
        public ActionResult Login()
        {
            var cookie = Request.Cookies["AdminUser"];
            if (cookie != null)
            {
                ViewBag.Id = cookie.Values["AdminId"];
                ViewBag.Password = cookie.Values["AdminPassword"];
                ViewBag.Remember = true;
            }
            return View();
        }
        //action login, post
        [HttpPost]
        public ActionResult Login(v_AccountAdmins admin, bool Remember)
        {
            try
            {
                var acc = db.v_AccountAdmins.Where(a => a.userName.Equals(admin.userName) && a.statusAdminAcc != "Stopped").SingleOrDefault();
                //nếu acc rỗng, tức tên tài khoản sai => lỗi
                if (acc == null)
                {
                    ModelState.AddModelError("", "Account is invalid!");
                }
                else if (acc.statusAdminAcc.Equals("stopped"))
                {
                    ModelState.AddModelError("", "Account stopped!");
                }
                else if (acc != null) //ngược lại
                {
                    if (acc.password.Equals(admin.password))
                    {
                        //nếu password đúng
                        Session["Adminacc"] = acc.userName.ToString();
                        Session["Adminname"] = acc.name.ToString();
                        Session["Adminpos"] = acc.posName.ToString();
                        // Save Password in cookies
                        var cookie = new HttpCookie("AdminUser");
                        if (Remember == true)
                        {
                            cookie.Values["AdminId"] = acc.userName.ToString(); ;
                            cookie.Values["AdminPassword"] = acc.password.ToString();
                            cookie.Expires = DateTime.Now.AddMonths(1);
                        }
                        else
                        {
                            cookie.Expires = DateTime.Now;
                        }
                        Response.Cookies.Add(cookie);

                        var RequestUrl = Session["AdminRequestUrl"] as String;
                        if (RequestUrl != null)
                        {
                            return Redirect(RequestUrl);
                        }
                        return Redirect("~/LibraryRemote.html");
                    }
                    else if (!acc.password.Equals(admin.password))
                    {
                        ModelState.AddModelError("", "Password is invalid, please input again!");
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["Adminacc"] = null;
            Session["Adminname"] = null;
            Session["Adminpos"] = null;
            return RedirectToAction("Login", "Administrators");
        }
    }
}