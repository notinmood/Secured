using System;
using System.Web.Mvc;
using GBFinance.Web.Models;
using HiLand.General;
using HiLand.General.BLL;
using HiLand.General.DAL;
using HiLand.General.Entity;
using HiLand.Utility.Cache;
using HiLand.Utility.Data;
using HiLand.Utility.DataBase;

namespace GBFinance.Web.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ControlTest()
        {
            return View();
        }

        public ActionResult CookieTest()
        {
            LoanCookie cookie = new LoanCookie();
            cookie.Load();
            this.ViewData["CurrentStep"] = cookie.CurrentStep;
            return View();
        }

        [HttpPost]
        public ActionResult CookieTest(bool? onlyPlaceHolder)
        {
            LoanCookie cookie = new LoanCookie();

            cookie.CurrentStep = 1;
            cookie.Save();
            this.ViewData["CurrentStep"] = cookie.CurrentStep;
            return View();
        }

        public ActionResult DisplayControllerName()
        {
            return View();
        }

        public ActionResult BLLTest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BLLTest(bool isplaceholder= true) 
        {
            LoanBasicBLL lbBLL = LoanBasicBLL.Instance;
            Guid guid = new Guid("d4b383c6-1c71-477d-948c-d30f9142a839");
            LoanBasicEntity entity = lbBLL.Get(guid);

            string loanString = string.Format("UPDATE [GeneralLoanBasic]   SET       [LoanStatus] =  [LoanStatus]+1  WHERE [LoanGuid] = '{0}'", "42d21e17-3140-482f-b0e6-56a4577e053a");
            LoanBasicDAL.HelperExInstance.ExecuteNonQuery(loanString);

            int i = 9;
            return View();
        }

        public ActionResult ClearCache()
        {
            CacheFactory.Create().Clear();
            return View();
        }

        public ActionResult DataBaseTest()
        {
            this.ViewData["result"] = SqlHelperEx.Instance.ConnectionString;
            return View();
        }

        [HttpPost]
        public ActionResult DataBaseTest(bool? onlyPlaceHolder)
        {
            this.ViewData["result"] = SqlHelperEx.Instance.ExecuteScalar("SELECT count(1)  FROM [CoreUser]").ToString();
            return View();
        }


        public ActionResult ValidateTest()
        {
            this.ViewData["Status"] = "Get";
            return View();
        }

        [HttpPost]
        public ActionResult ValidateTest(string myInput)
        {
            return Json(myInput);
        }

        public ActionResult ValidateTestResult(string myInput)
        {
            this.ViewData["Status"] = "Post";
            this.ViewData["MyInput"] = myInput;
            return View();
        }

        public ActionResult ValidateTestWrapper()
        {
            return View();
        }

        public ActionResult LoanScheduleTest()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoanScheduleTest(bool? isOnlyPlaceHolder)
        { 
            //http://goldenbridgefinance.com.au/Manage/Secured/ScheduleList?loanGuid=017f7f83-5668-4f6b-99d9-913ce159878b#
            LoanBasicEntity loanBasicInfo =  LoanBasicBLL.Instance.Get("017f7f83-5668-4f6b-99d9-913ce159878b");
            LoanBasicBLL.Instance.GeneralSchedules(loanBasicInfo, DateTimeHelper.RunningLocalNow);
            return View();
        }

        public ActionResult TimeZoneTest()
        {
            return View();
        }
    }
}
