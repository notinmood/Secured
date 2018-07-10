using System.Web.Mvc;
using HiLand.Framework4.Permission.Attributes;
using HiLand.General;

namespace GBFinance.Web.Areas.Manage.Controllers
{
    [UserAuthorize]
    public class SecuredController : BaseLoanController
    {
        //
        // GET: /Manage/SecuredLoan/

        public ActionResult Index(int id = 1)
        {
            return RedirectToAction("SecuredLoanList");
        }


        public ActionResult SecuredLoanList(int id = 1)
        {
            return base.LoanList(LoanTypes.Secured, id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="onlyPlaceholder"></param>
        /// <returns></returns>
        /// <remarks>
        /// 如果是通过输入条件进行查询，那么一定是从第一页开始显示
        /// </remarks>
        [HttpPost]
        public ActionResult SecuredLoanList(int id = 1, bool onlyPlaceholder = true)
        {
            return base.LoanList(LoanTypes.Secured, 1);
        }



        public ActionResult AcrossAreaTest()
        {
            return View();
        }
    }
}
