using HLLibrarySystemAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HLLibrarySystemAPI.Controllers
{
    public class StudentsMVCController : BaseMVCController
    {
        public ActionResult Login()
        {
            var cookie = Request.Cookies["User"];
            if (cookie != null)
            {
                ViewBag.Id = cookie.Values["Id"];
                ViewBag.Password = cookie.Values["Password"];
                ViewBag.Remember = true;
            }
            return View();
        }
        //action login, post
        [HttpPost]
        public ActionResult Login(v_AccountStudents loginStudent, bool Remember)
        {
            try
            {
                var acc = db.v_AccountStudents.Where(a => a.stu_userName.Equals(loginStudent.stu_userName)).SingleOrDefault();
                //if acc is empty, account is wrong => error
                if (acc == null)
                {
                    ModelState.AddModelError("", "Account is invalid, please input again.");
                }
                else if (acc.statusStuAcc.Equals("Stopped"))
                {
                    ModelState.AddModelError("", "Account is stopped, please contact the librarians.");
                }
                else if (acc != null && acc.stu_password.Equals(loginStudent.stu_password))
                {
                    Session["acc"] = acc.stu_userName.ToString();
                    Session["name"] = acc.stuFirstName.ToString() + " " + acc.stuLastName.ToString();
                    // Save Password in cookies
                    var cookie = new HttpCookie("User");
                    if (Remember == true)
                    {
                        cookie.Values["Id"] = acc.stu_userName.ToString(); ;
                        cookie.Values["Password"] = acc.stu_password.ToString();
                        cookie.Expires = DateTime.Now.AddMonths(1);
                    }
                    else
                    {
                        cookie.Expires = DateTime.Now;
                    }
                    Response.Cookies.Add(cookie);

                    var RequestUrl = Session["RequestUrl"] as String;
                    if (RequestUrl != null)
                    {
                        return Redirect(RequestUrl);
                    }
                    return Redirect("~/VisitorPage.html");
                }
                else if (!acc.stu_password.Equals(loginStudent.stu_password))
                {
                    ModelState.AddModelError("", "Password is invalid, please input again or contact the librarians");
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View();
        }

        public ActionResult SignOut()
        {
            Session["acc"] = null;
            Session["name"] = null;
            Session["callNumber"] = null;
            //còn thiếu session
            return RedirectToAction("Login", "StudentsMVC");
        }

        //action Forgot password
        public ActionResult Forgot()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Forgot(String Id, String Email)
        {
            var user = db.AccountStudents.Find(Id);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid UserName");
            }
            else if (Email != user.email)
            {
                ModelState.AddModelError("", "Invalid email");
            }
            else
            {
                ModelState.AddModelError("", "Your password was sent to your inbox!");
                SendMail.Send(Email, "Hello Student!", "Your Password:" + user.stu_password);
            }
            return View();
        }
    }
}