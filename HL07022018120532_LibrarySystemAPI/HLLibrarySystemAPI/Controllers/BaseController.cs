using HLLibrarySystemAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HLLibrarySystemAPI.Controllers
{
    public class BaseController : ApiController
    {
        protected LibraryDBEntities db = new LibraryDBEntities();
        //disposing connection to database
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
