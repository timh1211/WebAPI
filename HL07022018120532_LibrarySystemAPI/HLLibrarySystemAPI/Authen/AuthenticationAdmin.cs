using _23012018101517_LibraryManagementSystem.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _23012018101517_LibraryManagementSystem.Authen
{
    public class AuthenticationAdmin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!AuthenticateAdmin.Authenticated)
            {
                var Uri = HttpContext.Current.Request.Url.AbsoluteUri;
                HttpContext.Current.Session["AdminRequestUrl"] = Uri;
                HttpContext.Current.Response.Redirect("/Administrators/Login");
            }
        }
    }
}