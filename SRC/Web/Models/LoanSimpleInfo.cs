using System;
using HiLand.General;
using HiLand.Utility.Enums;

namespace GBFinance.Web.Models
{
    /// <summary>
    /// 关于贷款的最概要的信息
    /// </summary>
    public class LoanSimpleInfo
    {
        public LoanSimpleInfo(Guid loanGuid, Guid ownerGuid, DataUsingModes dataUsingMode= DataUsingModes.EndUserMode)
        {
            this.LoanGuid = loanGuid;
            this.OwnerGuid = ownerGuid;
            this.DataUsingMode = dataUsingMode;
        }
        
        /// <summary>
        /// 贷款GUID
        /// </summary>
        public Guid LoanGuid { get; set; }

        /// <summary>
        /// 贷款所有者GUID
        /// </summary>
        public Guid OwnerGuid { get; set; }

        /// <summary>
        /// 贷款所有者的其他信息
        /// </summary>
        public string OwnerAddon { get; set; }

        /// <summary>
        /// 当前贷款的类型
        /// </summary>
        public LoanTypes loanType
        {
            get
            {
                return Miscs.GetCurrentLoanType();
            }
        }

        /// <summary>
        /// 数据来源方式
        /// </summary>
        /// <remarks></remarks>
        public DataUsingModes DataUsingMode { get; set; }
    }
}