using System;
using System.Globalization;
using HiLand.General;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using HiLand.Utility.Finance;
using HiLand.Utility.Setting;
using HiLand.Utility.Web;
using HiLand.Utility4.MVC;

namespace GBFinance.Web.Models
{
    public class Miscs
    {
        private static IFormatProvider currentCultureInfo = null;
        /// <summary>
        /// 系统使用的文化信息
        /// </summary>
        public static IFormatProvider CurrentCultureInfo
        {
            get
            {
                if (currentCultureInfo == null)
                {
                    string usingCultureInfoString = Config.GetAppSetting("usingCultureInfo");
                    if (string.IsNullOrWhiteSpace(usingCultureInfoString))
                    {
                        usingCultureInfoString = "en-AU";
                    }

                    currentCultureInfo = new CultureInfo(usingCultureInfoString);
                }

                return currentCultureInfo;
            }
        }

        private static string dateTimeFormating = string.Empty;
        /// <summary>
        /// 系统使用的日期格式
        /// </summary>
        public static string DateTimeFormating
        {
            get
            {
                if (dateTimeFormating == string.Empty)
                {
                    dateTimeFormating = Config.GetAppSetting("dateTimeFormating");
                }

                if (string.IsNullOrWhiteSpace(dateTimeFormating))
                {
                    dateTimeFormating = "dd/MM/yyyy";
                }

                return dateTimeFormating;
            }
        }

        /// <summary>
        /// 通过澳洲格式的日期字符串转换成日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateFromAustrliaFormatString(string value)
        {
            DateTime paymentDate = DateTimeHelper.Min;
            if (string.IsNullOrWhiteSpace(value) == false)
            {
                paymentDate = DateTimeHelper.Parse(value, DateFormats.DMY);
            }
            return paymentDate;
        }

        /// <summary>
        ///  将日期转换成澳洲格式的日期字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertToAustrliaFormatStringFromDate(DateTime value)
        {
            return DateTimeHelper.UnParse(value,DateFormats.DMY,"/");
        }

        /// <summary>
        /// 根据月数和贷款周期类型计算还款期数
        /// </summary>
        /// <param name="monthCount"></param>
        /// <param name="paymentTermType"></param>
        /// <returns></returns>
        public static int CalculateTotalTermCount(int monthCount,PaymentTermTypes paymentTermType)
        {
            int result = 0;
            switch (paymentTermType)
            {
                case PaymentTermTypes.DoubleMonthly:
                    result = monthCount / 2;
                    break;
                case PaymentTermTypes.DoubleWeekly:
                    result = monthCount * 2;
                    break;

                case PaymentTermTypes.Weekly:
                    result = monthCount * 4;
                    break;
                case PaymentTermTypes.Monthly:
                default:
                    result = monthCount;
                    break;
            }

            return result;
        }

        #region 常用配置
        private static int pageSizeManage = 0;
        /// <summary>
        /// 后台管理中信息列表时，每页显示的信息数
        /// </summary>
        public static int PageSizeManage
        {
            get
            {
                if (pageSizeManage == 0)
                {
                    pageSizeManage = Config.GetAppSettingInt("PageSizeManage", 10);
                }
                return pageSizeManage;
            }
        }

        private static int pageSizeFront = 0;
        /// <summary>
        /// 后台管理中信息列表时，每页显示的信息数
        /// </summary>
        public static int PageSizeFront
        {
            get
            {
                if (pageSizeFront == 0)
                {
                    pageSizeFront = Config.GetAppSettingInt("PageSizeFront", 10);
                }
                return pageSizeFront;
            }
        }
        #endregion

        /// <summary>
        /// 当前贷款的简单信息
        /// </summary>
        public static LoanSimpleInfo CurrentLoanSimpleInfo
        {
            get
            {
                DataUsingModes dataUsingMode = DataUsingModes.EndUserMode;
                string currentAreaName = MVCHelper.GetCurrentAreaName().ToLower();
                //如果area是"",那么就是终端用户在前台操作；否则就是管理员在后台操作。
                if (string.IsNullOrWhiteSpace(currentAreaName))
                {
                    dataUsingMode = DataUsingModes.EndUserMode;
                }
                else
                {
                    dataUsingMode = DataUsingModes.AdminManagerMode;
                }

                Guid loanGuid = Guid.Empty;
                Guid ownerGuid = Guid.Empty;

                LoanTypes loanType = GetCurrentLoanType();
                string ownerGuidString = string.Empty;
                string loandGuidString = string.Empty;
                LoanCookie adminLoanCookie = null;

                if (dataUsingMode == DataUsingModes.AdminManagerMode)
                {
                    //2.1通过Cookie获取信息（用于后台管理）
                    adminLoanCookie = CookieInfo.Load<AdminLoanCookie>();

                    ownerGuidString = adminLoanCookie.OwnerGuid;
                    loandGuidString = adminLoanCookie.LoanGuid;

                    if (ownerGuid == Guid.Empty && string.IsNullOrWhiteSpace(ownerGuidString) == false && ownerGuidString != Guid.Empty.ToString())
                    {
                        ownerGuid = new Guid(ownerGuidString);
                    }

                    if (loanGuid == Guid.Empty && string.IsNullOrWhiteSpace(loandGuidString) == false && loandGuidString != Guid.Empty.ToString())
                    {
                        loanGuid = new Guid(loandGuidString);
                    }
                }
                else
                {
                    //2.2通过Cookie获取信息（主要用于客户的贷款流程）
                    LoanCookie loanCookie = null;

                    if (loanType == LoanTypes.Secured)
                    {
                        loanCookie = CookieInfo.Load<SecuredLoanCookie>();
                    }
                    else
                    {
                        loanCookie = CookieInfo.Load<UnSecuredLoanCookie>();
                    }

                    ownerGuidString = loanCookie.OwnerGuid;
                    loandGuidString = loanCookie.LoanGuid;

                    if (ownerGuid == Guid.Empty && string.IsNullOrWhiteSpace(ownerGuidString) == false && ownerGuidString != Guid.Empty.ToString())
                    {
                        ownerGuid = new Guid(ownerGuidString);
                    }

                    if (loanGuid == Guid.Empty && string.IsNullOrWhiteSpace(loandGuidString) == false && loandGuidString != Guid.Empty.ToString())
                    {
                        loanGuid = new Guid(loandGuidString);
                    }
                }

                return new LoanSimpleInfo(loanGuid, ownerGuid, dataUsingMode);
            }
        }

        /// <summary>
        /// 在cookie中设置贷款的步骤数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="step"></param>
        public static void SetLoanStep<T>(int step) where T : LoanCookie, new()
        {
            LoanCookie loanCookie = CookieInfo.Load<T>();
            bool isNeedSaveCookie = false;
            if (loanCookie.CurrentStep < step)
            {
                loanCookie.CurrentStep = step;
                isNeedSaveCookie = true;
            }

            if (isNeedSaveCookie == true)
            {
                loanCookie.Save();
            }
        }

        /// <summary>
        /// 重置LoanCookie
        /// </summary>
        public static void ResetLoanCookie<T>() where T : LoanCookie, new()
        {
            LoanCookie loanCookie = CookieInfo.Load<T>();
            loanCookie.CurrentStep = 0;
            loanCookie.OwnerGuid = string.Empty;
            loanCookie.LoanGuid = string.Empty;
            loanCookie.Save();
        }

        /// <summary>
        /// 创建新的贷款Cookie
        /// </summary>
        /// <param name="isAlwaysCreateNew">是否总是创建新的Cookie（即原来存在的情况下会覆盖）</param>
        public static void CreateNewLoanCookie<T>(bool isAlwaysCreateNew = false) where T : LoanCookie, new()
        {
            LoanCookie loanCookie = CookieInfo.Load<T>();
            if (loanCookie.CheckIsNewBirthLoan() == true || isAlwaysCreateNew == true)
            {
                loanCookie.OwnerGuid = Guid.NewGuid().ToString();
                loanCookie.LoanGuid = Guid.NewGuid().ToString();
                loanCookie.Save();
            }
        }

        /// <summary>
        /// 获取当前执行的贷款的类型
        /// </summary>
        /// <returns></returns>
        public static LoanTypes GetCurrentLoanType()
        {
            LoanTypes loanType = LoanTypes.UnSecured;
            string currentControllerName = MVCHelper.GetCurrentControllerName().ToLower();
            if (currentControllerName == "secured" || currentControllerName == "securedloan")
            {
                loanType = LoanTypes.Secured;
            }
            else
            {
                loanType = LoanTypes.UnSecured;
            }

            return loanType;
        }
    }
}