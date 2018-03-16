using HLLibrarySystemAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HLLibrarySystemAPI.Controllers
{
    public class BaseMVCController : Controller
    {
        // GET: BaseMVC
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