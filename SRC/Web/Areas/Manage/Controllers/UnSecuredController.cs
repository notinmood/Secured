using System.Web.Mvc;
using HiLand.Framework4.Permission.Attributes;
using HiLand.General;

namespace GBFinance.Web.Areas.Manage.Controllers
{
    [UserAuthorize]
    public class UnSecuredController : BaseLoanController
    {
        //
        // GET: /Manage/UnSecuredLoan/

        public ActionResult Index(int id = 1)
        {
            return RedirectToAction("UnSecuredLoanList");
        }


        public ActionResult UnSecuredLoanList(int id = 1)
        {
            return base.LoanList(LoanTypes.UnSecured, id);
        }

    }
}
