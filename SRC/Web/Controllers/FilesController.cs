using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class FilesController : Controller
    {
        //
        // GET: /Files/

        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Form3A()
        {
            string fileFullName= Server.MapPath("~/Information/Form3A.doc");
            return File(fileFullName, "application/msword ");
        }

    }
}
