using System.Web.Mvc;

namespace GBFinance.Web.Controllers
{
    public class UserControlController : Controller
    {
        //
        // GET: /UserControl/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ContactExpress(string picName)
        {
            this.ViewData["picName"] = picName;
            return View();
        }
    }
}
