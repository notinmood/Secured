using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GBFinance.BLL;
using GBFinance.Entity;
using GBFinance.Web.Models;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.General;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;
using HiLand.Utility.Finance;
using HiLand.Utility.Paging;
using HiLand.Utility.Web;
using HiLand.Utility4.MVC.Data;
using Web.Models;
using Webdiyer.WebControls.Mvc;

namespace GBFinance.Web.Areas.Manage.Controllers
{
    /// <summary>
    /// 本类不向View公开，为SecuredLoan和UnSecuredLoan的基类，仅仅为子类提供公用方法。
    /// </summary>
    public class BaseLoanController : GBFinance.Web.Controllers.BaseLoanController
    {
        public ActionResult UserNote(Guid? userID)
        {
            if (userID.HasValue)
            {
                BusinessUserExt loanUser = Converter.InheritedEntityConvert<BusinessUser, BusinessUserExt>(BusinessUserBLL.Get(userID.Value));
                return View(loanUser);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult UserNote(Guid userID, string note)
        {
            LogicStatusInfo statusInfo = new LogicStatusInfo();
            BusinessUserExt loanUser = Converter.InheritedEntityConvert<BusinessUser, BusinessUserExt>(BusinessUserBLL.Get(userID));
            if (loanUser != null)
            {
                loanUser.Note = note;
                statusInfo.IsSuccessful = BusinessUserBLL.UpdateUser(loanUser);

                if (statusInfo.IsSuccessful == true)
                {
                    LoanBasicBLL.Instance.CleanUpListCache();
                }
            }

            return Json(statusInfo);
        }

        public ActionResult UserFollowUpHistory(Guid? userID)
        {
            if (userID.HasValue)
            {
                BusinessUserExt loanUser = Converter.InheritedEntityConvert<BusinessUser, BusinessUserExt>(BusinessUserBLL.Get(userID.Value));
                return View(loanUser);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult UserFollowUpHistory(Guid userID, string flowUpHistory)
        {
            LogicStatusInfo statusInfo = new LogicStatusInfo();
            BusinessUserExt loanUser = Converter.InheritedEntityConvert<BusinessUser, BusinessUserExt>(BusinessUserBLL.Get(userID));
            if (loanUser != null)
            {
                loanUser.FlowUpHistory = flowUpHistory;
                statusInfo.IsSuccessful = BusinessUserBLL.UpdateUser(loanUser);

                if (statusInfo.IsSuccessful == true)
                {
                    LoanBasicBLL.Instance.CleanUpListCache();
                }
            }

            return Json(statusInfo);
        }

        public ActionResult LoanStatus(Guid? loanID)
        {
            if (loanID.HasValue)
            {
                LoanBasicEntity loanEntity = LoanBasicBLL.Instance.Get(loanID.Value);
                return View(loanEntity);
            }
            else
            {
                return new EmptyResult();
            }
        }

        [HttpPost]
        public ActionResult LoanStatus(Guid loanID, LoanStatuses loanStatuses)
        {
            LogicStatusInfo statusInfo = new LogicStatusInfo();
            LoanBasicEntity loanEntity = LoanBasicBLL.Instance.Get(loanID);
            if (loanEntity != null)
            {
                loanEntity.LoanStatus = loanStatuses;
                statusInfo.IsSuccessful = LoanBasicBLL.Instance.Update(loanEntity);
            }

            return Json(statusInfo);
        }

        /// <summary>
        /// 设置贷款信息的状态为Deleted
        /// </summary>
        /// <param name="loanGuid"></param>
        /// <returns></returns>
        public ActionResult SetLoanStatusToDeleted(Guid loanGuid)
        {
            return LoanStatus(loanGuid, LoanStatuses.Deleted);
        }

        /// <summary>
        /// 设置贷款信息的状态为New
        /// </summary>
        /// <param name="loanGuid"></param>
        /// <returns></returns>
        public ActionResult SetLoanStatusToNew(Guid loanGuid)
        {
            return LoanStatus(loanGuid, LoanStatuses.New);
        }

        /// <summary>
        /// 真实地删除贷款信息
        /// </summary>
        /// <param name="loanGuid"></param>
        /// <returns></returns>
        public ActionResult DeleteLoan(Guid loanGuid)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            logicStatusInfo.IsSuccessful = LoanBasicBLL.Instance.Delete(loanGuid);

            return Json(logicStatusInfo);
        }

        protected ActionResult LoanList(LoanTypes loanType, int id = 1, string viewName = "")
        {
            int pageIndex = id;
            int pageSize = Miscs.PageSizeManage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = string.Format(" LoanType= {0} AND LoanStatus!={1}  ",
                (int)loanType, (int)LoanStatuses.UserUnCompleted);
            string orderClause = "LoanID DESC";

            string queryCondition = GetQueryCondition();
            if (string.IsNullOrWhiteSpace(queryCondition) == false)
            {
                whereClause += " AND " + queryCondition;
            }

            string queryName = RequestHelper.GetValue("queryConditionName");
            string queryValue = RequestHelper.GetValue("queryConditionValue");
            if (queryName == "Deleted")
            {
                whereClause += string.Format("  AND LoanStatus={0}", (int)LoanStatuses.Deleted);
            }
            else
            {
                whereClause += string.Format(" AND LoanStatus!={0}", (int)LoanStatuses.Deleted);
            }

            PagedEntityCollection<LoanBasicEntity> entityList = LoanBasicBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            List<LoanBasicExtEntity> tempList = new List<LoanBasicExtEntity>();
            foreach (var currentItem in entityList.Records)
            {
                LoanBasicExtEntity extEntity = Converter.InheritedEntityConvert<LoanBasicEntity, LoanBasicExtEntity>(currentItem);
                if (string.IsNullOrWhiteSpace(currentItem.LoanOwnerKey) == false && GuidHelper.IsInvalidOrEmpty(currentItem.LoanOwnerKey) == false)
                {
                    BusinessUserExt currentLoanUser = Converter.InheritedEntityConvert<BusinessUser, BusinessUserExt>(BusinessUserBLL.Get(GuidHelper.TryConvert(currentItem.LoanOwnerKey)));
                    if (currentLoanUser != null)
                    {
                        extEntity.UserIndexID = currentLoanUser.UserID;
                        extEntity.UserFirstName = currentLoanUser.UserNameFirst;
                        extEntity.UserMiddleName = currentLoanUser.UserNameMiddle;
                        extEntity.UserLastName = currentLoanUser.UserNameLast;
                        extEntity.UserBirthday = currentLoanUser.UserBirthDay;
                        extEntity.UserEmail = currentLoanUser.UserEmail;
                        extEntity.UserRegisterDate = currentLoanUser.UserRegisterDate;
                        extEntity.UserNote = currentLoanUser.Note;
                        extEntity.UserFlowUpHistory = currentLoanUser.FlowUpHistory;
                    }
                }
                tempList.Add(extEntity);
            }

            PagedList<LoanBasicExtEntity> pagedExList = new PagedList<LoanBasicExtEntity>(tempList, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            if (viewName == "")
            {
                viewName = "LoanList";
            }

            this.PassParam("queryName", queryName);
            this.PassParam("queryValue", queryValue);
            return View(viewName, pagedExList);
        }

        public ActionResult LoanDetails(Guid? loanGuid)
        {
            if (loanGuid.HasValue)
            {
                string loanOwnerKey = string.Empty;
                LoanBasicEntity loanBasicEntity = LoanBasicBLL.Instance.Get(loanGuid.Value);
                if (loanBasicEntity != null)
                {
                    loanOwnerKey = loanBasicEntity.LoanOwnerKey;
                }

                AdminLoanCookie cookieInfo = CookieInfo.Load<AdminLoanCookie>();
                cookieInfo.OwnerGuid = loanOwnerKey;
                cookieInfo.LoanGuid = loanGuid.Value.ToString();
                cookieInfo.Save();
            }

            return View();
        }

        public ActionResult ScheduleList(Guid? loanGuid)
        {
            Guid loanGuidLocal = Guid.Empty;
            List<LoanScheduleEntity> scheduleList = new List<LoanScheduleEntity>();

            if (loanGuid.HasValue)
            {
                loanGuidLocal = loanGuid.Value;
                string whereClause = string.Format(" LoanGuid='{0}' ", loanGuidLocal.ToString());
                scheduleList = LoanScheduleBLL.Instance.GetList(Logics.False, whereClause, 0, " PaymentDate ");
            }

            this.ViewData["loanGuid"] = loanGuidLocal;
            return View(scheduleList);
        }

        public ActionResult LoanInfoForSchedule(Guid? loanGuid, bool isDisplayGenerateButton)
        {
            LoanBasicEntity loanBasicInfo = new LoanBasicEntity();
            if (loanGuid.HasValue)
            {
                loanBasicInfo = LoanBasicBLL.Instance.Get(loanGuid.Value);
            }

            //this.ViewData["isDisplayGenerateButton"] = isDisplayGenerateButton;
            return View(loanBasicInfo);
        }

        /// <summary>
        /// 针对某一贷款生成分期明细
        /// </summary>
        /// <param name="loanBasicInfo"></param>
        public ActionResult GenerateSchedules2(LoanBasicEntity loanBasicInfo, DateTime? paymentStartCalculateDate)
        {
            LogicStatusInfo logicInfo = new LogicStatusInfo();
            if (loanBasicInfo != null)
            {
                // 获取用户还款的周期
                List<LoanWorkEntity> workList = LoanWorkBLL.Instance.GetList(string.Format("UserGuid='{0}'", loanBasicInfo.LoanOwnerKey));
                if (workList != null && workList.Count > 0)
                {
                    LoanWorkEntity firstWork = workList[0];
                    int loanTermCount = Miscs.CalculateTotalTermCount(loanBasicInfo.LoanTermCount, firstWork.IncomeCircle);

                    logicInfo.IsSuccessful = LoanBasicBLL.Instance.GeneralSchedules(loanBasicInfo, paymentStartCalculateDate, loanTermCount, firstWork.IncomeCircle);
                }
                else
                {
                    logicInfo.IsSuccessful = false;
                    logicInfo.Message = "No Work Info";
                }
            }
            else
            {
                logicInfo.IsSuccessful = false;
                logicInfo.Message = "No Loan Info";
            }

            return Json(logicInfo);
        }


        /// <summary>
        /// 针对某一贷款生成分期明细
        /// </summary>
        /// <param name="loanBasicInfo"></param>
        public ActionResult GenerateSchedules(Guid loanGuid, string paymentStartCalculateDate)
        {
            LoanBasicEntity loanBasicInfo = null;
            if (loanGuid != Guid.Empty)
            {
                loanBasicInfo = LoanBasicBLL.Instance.Get(loanGuid);
            }

            DateTime paymentDate = Miscs.ConvertToDateFromAustrliaFormatString(paymentStartCalculateDate);

            return GenerateSchedules2(loanBasicInfo, paymentDate);
        }

        /// <summary>
        /// 对某一分期的记录，重新分期
        /// </summary>
        /// <param name="scheduleGuid"></param>
        /// <param name="paymentTermCount"></param>
        /// <param name="paymentTermType"></param>
        /// <param name="balanceToReinstall">需要分期的费用，如果未传入该值，将会对Schedule中的费用进行分期</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReinstallSchedule(Guid scheduleGuid, int paymentTermCount, string paymentTermType, decimal balanceToReinstall = 0, DateTime? loanStartDate = null)
        {
            PaymentTermTypes termType = Converter.TryToEnum<PaymentTermTypes>(paymentTermType);
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            LoanScheduleEntity entity = LoanScheduleBLL.Instance.Get(scheduleGuid);
            LoanScheduleBLL.Instance.ReInstall(entity, paymentTermCount, termType, balanceToReinstall, loanStartDate);

            return Json(logicStatusInfo);
        }

        /// <summary>
        /// 对某一分期的记录，重新分期
        /// </summary>
        /// <param name="scheduleGuid"></param>
        /// <param name="paymentTermCount"></param>
        /// <param name="paymentTermType"></param>
        /// <param name="balanceToReinstall"></param>
        /// <param name="loanStartDate"></param>
        /// <returns></returns>
        public ActionResult ReinstallSchedule2(Guid scheduleGuid, int paymentTermCount, string paymentTermType, decimal balanceToReinstall = 0, string loanStartDate = "")
        {
            DateTime? targetDate = DateTimeHelper.Parse(loanStartDate, DateFormats.DMY);
            if (targetDate == DateTimeHelper.Min)
            {
                targetDate = null;
            }
            return ReinstallSchedule(scheduleGuid, paymentTermCount, paymentTermType, balanceToReinstall, targetDate);
        }

        /// <summary>
        /// 在页面上直接创建一个分期记录
        /// </summary>
        /// <param name="loanGuid"></param>
        /// <param name="payDate"></param>
        /// <param name="payPrincipal"></param>
        /// <param name="payInterest"></param>
        /// <param name="payPenalty"></param>
        /// <param name="payLateCharge"></param>
        /// <param name="payOtherFee"></param>
        /// <param name="paidAmount"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateSchedule(Guid loanGuid, Guid? scheduleParentGuid, DateTime? payDate, decimal? payPrincipal, decimal? payInterest, decimal? payPenalty, decimal? payLateCharge, decimal? payOtherFee, decimal? paidAmount)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            LoanBasicEntity loanBasicInfo = LoanBasicBLL.Instance.Get(loanGuid);
            if (loanBasicInfo != null)
            {
                LoanScheduleEntity entity = new LoanScheduleEntity();

                entity.LoanGuid = loanGuid;
                entity.ScheduleTimes = 2;//TODO:这个应该根据当前的分期+1

                if (scheduleParentGuid.HasValue)
                {
                    entity.ScheduleParentGuid = scheduleParentGuid.Value;
                }
                else
                {
                    entity.ScheduleParentGuid = Guid.Empty;
                }

                if (payInterest.HasValue)
                {
                    entity.Interest = payInterest.Value;
                }
                else
                {
                    entity.Interest = 0;
                }

                if (payDate.HasValue)
                {
                    entity.PaymentDate = payDate.Value;
                }
                else
                {
                    entity.PaymentDate = DateTimeHelper.Min;
                }

                if (payPrincipal.HasValue)
                {
                    entity.Principal = payPrincipal.Value;
                }
                else
                {
                    entity.Principal = 0;
                }

                if (payPenalty.HasValue)
                {
                    entity.Penalty = payPenalty.Value;
                }
                else
                {
                    entity.Penalty = 0;
                }

                if (payLateCharge.HasValue)
                {
                    entity.LateCharge = payLateCharge.Value;
                }
                else
                {
                    entity.LateCharge = 0;
                }

                if (payOtherFee.HasValue)
                {
                    entity.OtherFee = payOtherFee.Value;
                }
                else
                {
                    entity.OtherFee = 0;
                }

                if (paidAmount.HasValue)
                {
                    entity.PrincipalPaid = paidAmount.Value;
                }
                else
                {
                    entity.PrincipalPaid = 0;
                }

                logicStatusInfo.IsSuccessful = LoanScheduleBLL.Instance.Create(entity);
            }

            return Json(logicStatusInfo);
        }

        /// <summary>
        /// 在页面上直接创建一个分期记录
        /// </summary>
        /// <param name="loanGuid"></param>
        /// <param name="payDate"></param>
        /// <param name="payPrincipal"></param>
        /// <param name="payInterest"></param>
        /// <param name="payPenalty"></param>
        /// <param name="payLateCharge"></param>
        /// <param name="payOtherFee"></param>
        /// <param name="paidAmount"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateSchedule2(Guid loanGuid, Guid? scheduleParentGuid, string payDate, decimal? payPrincipal, decimal? payInterest, decimal? payPenalty, decimal? payLateCharge, decimal? payOtherFee, decimal? paidAmount)
        {
            DateTime paymentDate = Miscs.ConvertToDateFromAustrliaFormatString(payDate);
            return CreateSchedule(loanGuid, scheduleParentGuid, paymentDate, payPrincipal, payInterest, payPenalty, payLateCharge, payOtherFee, paidAmount);
        }

        [HttpPost]
        public ActionResult EditSchedule(Guid scheduleGuid, DateTime? payDate, decimal? payPrincipal, decimal? payInterest, decimal? payPenalty, decimal? payLateCharge, decimal? payOtherFee, decimal? paidAmount, DateTime? paidDate)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            LoanScheduleEntity entity = LoanScheduleBLL.Instance.Get(scheduleGuid);
            if (payPrincipal.HasValue)
            {
                entity.Principal = payPrincipal.Value;
            }
            if (payInterest.HasValue)
            {
                entity.Interest = payInterest.Value;
            }
            if (payPenalty.HasValue)
            {
                entity.Penalty = payPenalty.Value;
            }
            if (payLateCharge.HasValue)
            {
                entity.LateCharge = payLateCharge.Value;
            }
            if (payOtherFee.HasValue)
            {
                entity.OtherFee = payOtherFee.Value;
            }
            if (payDate.HasValue && payDate.Value != DateTimeHelper.Min && payDate.Value != DateTime.MinValue)
            {
                entity.PaymentDate = payDate.Value;
            }

            if (paidAmount.HasValue)
            {
                entity.PrincipalPaid = paidAmount.Value;
            }

            if (paidDate.HasValue && paidDate.Value != DateTimeHelper.Min && paidDate.Value != DateTime.MinValue)
            {
                entity.PaidDate = paidDate.Value;
            }
            else
            {
                entity.PaidDate = DateTimeHelper.RunningLocalNow;
            }

            logicStatusInfo.IsSuccessful = LoanScheduleBLL.Instance.Update(entity);

            return Json(logicStatusInfo);
        }

        [HttpPost]
        public ActionResult EditSchedule2(Guid scheduleGuid, string payDate, decimal? payPrincipal, decimal? payInterest, decimal? payPenalty, decimal? payLateCharge, decimal? payOtherFee, decimal? paidAmount, string paidDate)
        {
            DateTime paymentDate = Miscs.ConvertToDateFromAustrliaFormatString(payDate);
            DateTime paidDateTime = Miscs.ConvertToDateFromAustrliaFormatString(paidDate);
            return EditSchedule(scheduleGuid, paymentDate, payPrincipal, payInterest, payPenalty, payLateCharge, payOtherFee, paidAmount, paidDateTime);
        }

        [HttpPost]
        public ActionResult DeleteSchedule(Guid scheduleGuid)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            LoanScheduleEntity entity = LoanScheduleBLL.Instance.Get(scheduleGuid);
            logicStatusInfo.IsSuccessful = LoanScheduleBLL.Instance.Delete(entity);

            return Json(logicStatusInfo);
        }



        #region 辅助方法
        /// <summary>
        /// 获取用户输入的查询条件
        /// </summary>
        /// <returns></returns>
        private string GetQueryCondition()
        {
            string result = string.Empty;
            string queryName = RequestHelper.GetValue("queryConditionName");
            string queryValue = RequestHelper.GetValue("queryConditionValue");

            switch (queryName)
            {
                case "CustomerNo":
                    if (string.IsNullOrWhiteSpace(queryValue) == false)
                    {
                        int customNo = Converter.TryToInt32(queryValue, 0);
                        if (customNo != 0)
                        {
                            result = string.Format(" UserID={0} ", customNo);
                        }
                    }
                    break;
                case "FirstName":
                    if (string.IsNullOrWhiteSpace(queryValue) == false)
                    {
                        result = string.Format(" UserNameFirst like '%{0}%' ", SQLInjectionHelper.GetSafeSqlBeforeSave(queryValue));
                    }
                    break;
                case "LastName":
                    if (string.IsNullOrWhiteSpace(queryValue) == false)
                    {
                        result = string.Format(" UserNameLast like '%{0}%'  ", SQLInjectionHelper.GetSafeSqlBeforeSave(queryValue));
                    }
                    break;
                case "DateOfBirth":
                    if (string.IsNullOrWhiteSpace(queryValue) == false)
                    {
                        DateTime birthday = DateTimeHelper.Parse(queryValue, DateFormats.DMY);
                        if (birthday != DateTimeHelper.Min)
                        {
                            result = string.Format(" UserBirthDay='{0}' ", birthday);
                        }
                    }
                    break;
                case "Approved":
                    result = string.Format(" LoanStatus={0} ", (int)LoanStatuses.Approved);
                    break;
                case "Rejected":
                    result = string.Format(" LoanStatus={0} ", (int)LoanStatuses.Rejected);
                    break;
                case "Deleted":
                    result = string.Format(" LoanStatus={0} ", (int)LoanStatuses.Deleted);
                    break;
                case "":
                default:
                    result = string.Empty;
                    break;
            }

            return result;
        }
        #endregion
    }
}
