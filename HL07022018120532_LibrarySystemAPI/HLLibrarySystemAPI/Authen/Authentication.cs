using _23012018101517_LibraryManagementSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _23012018101517_LibraryManagementSystem.Authen
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Authenticate.Authenticated)
            {
                var Uri = HttpContext.Current.Request.Url.AbsoluteUri; //Lấy url hiện tại
                HttpContext.Current.Session["RequestUrl"] = Uri; //gán vào session
                HttpContext.Current.Response.Redirect("/Students/Login");
            }
        }
    }
}