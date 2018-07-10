using System.Collections.Generic;
using System.Linq;
using HiLand.General.BLL;
using HiLand.General.Entity;

namespace GBFinance.Web.Models
{
    public class LoanBasicSetting
    {
        private static List<BasicSettingEntity> basicSettingList = null;
        private static List<BasicSettingEntity> BasicSettingList
        {
            get
            {
                if (basicSettingList == null)
                {
                    basicSettingList = BasicSettingBLL.Instance.GetList(string.Empty);
                }

                return basicSettingList;
            }
        }

        public static decimal SecuredLoansAmountMin
        {
            get
            {
                return GetSettingValue("SecuredLoansAmountMin", 5000);
            }
        }

        public static decimal SecuredLoansAmountMax
        {
            get
            {
                return GetSettingValue("SecuredLoansAmountMax", 50000);
            }
        }

        public static decimal UnSecuredLoansAmountMin
        {
            get
            {
                return GetSettingValue("UnSecuredLoansAmountMin", 1000);
            }
        }

        public static decimal UnSecuredLoansAmountMax
        {
            get
            {
                return GetSettingValue("UnSecuredLoansAmountMax", 5000);
            }
        }

        public static decimal SecuredLoansAnnualInterest
        {
            get
            {
                return GetSettingValue("SecuredLoansAnnualInterest", 0.48M);
            }
        }

        public static decimal UnSecuredLoansAnnualInterest
        {
            get
            {
                return GetSettingValue("UnSecuredLoansAnnualInterest", 0.48M);
            }
        }

        private static decimal GetSettingValue(string settingKey, decimal defaultValue)
        {
            decimal securedLoansAmountMin = defaultValue;
            BasicSettingEntity entity = BasicSettingList.Single(currentEntity => currentEntity.SettingKey == settingKey);
            if (entity != null)
            {
                decimal.TryParse(entity.SettingValue, out  securedLoansAmountMin);
            }

            return securedLoansAmountMin;
        }
    }
}