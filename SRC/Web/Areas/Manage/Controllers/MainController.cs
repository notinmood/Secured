using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HiLand.Framework4.Permission.Attributes;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;

namespace GBFinance.Web.Areas.Manage.Controllers
{
    [UserAuthorize]
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 最新贷款提醒
        /// </summary>
        /// <returns></returns>
        public ActionResult LastLoanTip()
        {
            string orderClause = "LoanID DESC";
            string whereClause = string.Format(" ReadDate is null OR ReadDate= '{0}' ", DateTimeHelper.Min); //SqlWhereClauseBuilder.Create().AppendCondition<DateTime>("ReadDate", DateTimeHelper.Min,CompareModes.NotEquals).GetClause().CluaseString;
            List<LoanBasicEntity> entityList = LoanBasicBLL.Instance.GetList(whereClause, orderClause);
            return View(entityList);
        }

        /// <summary>
        /// 更新贷款信息的读取信息
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdateLoanReadInfo(Guid loanGuid)
        {
            LoanBasicBLL.Instance.UpdataReadInfo(loanGuid);
            return Json("");
        }
    }
}
