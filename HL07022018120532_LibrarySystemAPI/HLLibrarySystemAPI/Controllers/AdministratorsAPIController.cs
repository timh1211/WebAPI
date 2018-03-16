using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace HLLibrarySystemAPI.Controllers
{
    public class AdministratorsAPIController : BaseController
    {
        [HttpGet]
        [Route("api/AdministratorsAPI/GetSession")]
        public IHttpActionResult GetSession()
        {
            List<string> sessionStorage = new List<string>();
            if (HttpContext.Current.Session["Adminacc"] == null)
            {
                sessionStorage.Add(null);
            }
            else if (HttpContext.Current.Session["Adminacc"] != null)
            {
                sessionStorage.Add(HttpContext.Current.Session["Adminacc"].ToString());
                sessionStorage.Add(HttpContext.Current.Session["Adminname"].ToString());
                sessionStorage.Add(HttpContext.Current.Session["Adminpos"].ToString());
            }
            return Ok(sessionStorage);
        }
    }
}
