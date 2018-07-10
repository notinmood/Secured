using System.Collections.Generic;
using System.Web.Mvc;
using GBFinance.Web.Models;
using HiLand.Utility.Data;
using HiLand.Utility.Finance;

namespace GBFinance.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            PassLoanSetting();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Faqs()
        {
            return View();
        }

        [ActionName("Contact-Us")]
        public ActionResult ContactUs()
        {
            return View("ContactUs");
        }

        [ActionName("How-It-Works")]
        public ActionResult HowItWorks()
        {
            return View("HowItWorks");
        }

        [ActionName("Costs-And-Fees")]
        public ActionResult CostsAndFees()
        {
            return View("CostsAndFees");
        }

        [ActionName("Privacy-Statement")]
        public ActionResult PrivacyStatement()
        {
            return View("PrivacyStatement");
        }

        [ActionName("Unsecured-Loans-Calculator")]
        public ActionResult UnsecuredLoansCalculator()
        {
            return View("UnsecuredLoansCalculator");
        }

        [ActionName("Unsecured-Loans-Calculator")]
        [HttpPost]
        public ActionResult UnsecuredLoansCalculator(double? loanAmount, int? loanTerms)
        {
            GetSchedule(loanAmount, loanTerms);
            return View("UnsecuredLoansCalculator");
        }

        [ActionName("Secured-Loans-Calculator")]
        public ActionResult SecuredLoansCalculator()
        {
            return View("SecuredLoansCalculator");
        }

        [ActionName("Secured-Loans-Calculator")]
        [HttpPost]
        public ActionResult SecuredLoansCalculator(double? loanAmount, int? loanTerms, PaymentTermTypes loanTermType = PaymentTermTypes.Monthly)
        {
            GetSchedule(loanAmount, loanTerms, loanTermType);
            return View("SecuredLoansCalculator");
        }

        private void GetSchedule(double? loanAmount, int? loanTerms, PaymentTermTypes loanTermType = PaymentTermTypes.Monthly)
        {
            if (loanTerms.HasValue && loanAmount.HasValue)
            {
                double securedLoansAnnualInterest = (double)LoanBasicSetting.SecuredLoansAnnualInterest;
                RateConverter rateConverter = new SimpleRateConverter(securedLoansAnnualInterest, PaymentTermTypes.Annual);
                double rate = rateConverter.GetRate(loanTermType);
                int loanTermCount = Miscs.CalculateTotalTermCount(loanTerms.Value, loanTermType);
                List<Payment> paymentList = CPMLoan.GetPaymentSchedule(rate, loanAmount.Value, loanTermCount, loanTermType, DateTimeHelper.RunningLocalNow);
                this.ViewData["paymentList"] = paymentList;
            }
        }


        [ActionName("Unsecured-Loans")]
        public ActionResult UnsecuredLoans()
        {
            PassLoanSetting();
            return View("UnsecuredLoans");
        }

        [ActionName("Secured-Loans")]
        public ActionResult SecuredLoans()
        {
            PassLoanSetting();
            return View("SecuredLoans");
        }

        [ActionName("Payroll-Finance")]
        public ActionResult PayrollFinance()
        {
            return View("PayrollFinance");
        }

        #region 辅助方法
        /// <summary>
        /// 向页面传递贷款设置信息
        /// </summary>
        private void PassLoanSetting()
        {
            //LoanBasicSetting loanBasicSetting = new LoanBasicSetting();
            decimal SecuredLoansAmountMin = LoanBasicSetting.SecuredLoansAmountMin;
            this.ViewData["SecuredLoansAmountMin"] = SecuredLoansAmountMin;

            decimal SecuredLoansAmountMax = LoanBasicSetting.SecuredLoansAmountMax;
            this.ViewData["SecuredLoansAmountMax"] = SecuredLoansAmountMax;

            decimal UnSecuredLoansAmountMin = LoanBasicSetting.UnSecuredLoansAmountMin;
            this.ViewData["UnSecuredLoansAmountMin"] = UnSecuredLoansAmountMin;

            decimal UnSecuredLoansAmountMax = LoanBasicSetting.UnSecuredLoansAmountMax;
            this.ViewData["UnSecuredLoansAmountMax"] = UnSecuredLoansAmountMax;
        }
        #endregion
    }
}
