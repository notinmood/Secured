using HiLand.Utility.Web;

namespace GBFinance.Web.Models
{
    /// <summary>
    /// 此cookie在客户的贷款流程启动时生成，流程结束时销毁；在流程过程中保持连接状态。(请使用此类的派生类)
    /// </summary>
    public class LoanCookie : CookieInfo
    {
        private string ownerGuid = string.Empty;
        /// <summary>
        /// 贷款所有者的Guid
        /// </summary>
        public string OwnerGuid
        {
            get { return this.ownerGuid; }
            set { this.ownerGuid = value; }
        }

        private string ownerAddon = string.Empty;
        /// <summary>
        /// 贷款所有者的其他信息
        /// </summary>
        public string OwnerAddon
        {
            get { return this.ownerAddon; }
            set { this.ownerAddon = value; }
        }

        private string loanGuid = string.Empty;
        /// <summary>
        /// 贷款Guid
        /// </summary>
        public string LoanGuid
        {
            get { return this.loanGuid; }
            set { this.loanGuid = value; }
        }

        private int currentStep = 0;
        /// <summary>
        /// 贷款流程的当前步骤
        /// </summary>
        public int CurrentStep
        {
            get { return this.currentStep; }
            set { this.currentStep = value; }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            this.currentStep = 0;

            this.ownerGuid = string.Empty;
            this.ownerAddon = string.Empty;
            this.loanGuid = string.Empty;

            this.Save();
        }

        /// <summary>
        /// 检查此贷款信息是否刚刚创建
        /// </summary>
        /// <returns></returns>
        public bool CheckIsNewBirthLoan()
        {
            if (CurrentStep == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}