using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _23012018101517_LibraryManagementSystem.Utils
{
    public class AuthenticateAdmin
    {
        public static bool Authenticated
        {
            get
            {
                var user = HttpContext.Current.Session["Adminacc"];
                return user != null;
            }
        }
    }
}