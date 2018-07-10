using System;
using System.Web.Mvc;
using System.Web.Security;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework.Membership;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;

namespace GBFinance.Web.Areas.Manage.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userName, string password)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            LoginStatuses loginStatus = LoginStatuses.Successful;
            BusinessUser businessUser= null;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                logicStatusInfo.IsSuccessful = false;
                logicStatusInfo.Message = "Please must enter your account and password first!";
            }
            else
            {
                businessUser= BusinessUserBLL.Login(userName, password, out loginStatus);

                if (loginStatus == LoginStatuses.Successful)
                {
                    logicStatusInfo.IsSuccessful = true;
                }
                else
                {
                    logicStatusInfo.IsSuccessful = false;
                    logicStatusInfo.Message = loginStatus.ToString();
                }
            }

            if (logicStatusInfo.IsSuccessful == true)
            {
                FormsAuthentication.RedirectFromLoginPage(userName, false);
            }

            return View(logicStatusInfo);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            UserCookie userCookie = UserCookie.Load<UserCookie>();
            userCookie.UserGuid = Guid.Empty;
            userCookie.UserID = 0;
            userCookie.UserName = string.Empty;
            userCookie.Save(DateTime.Now.AddDays(-1));
            return RedirectToAction("Index","Main");
        }

        public ActionResult ChangePassword()
        {
            PassCurrentUser();
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(Guid userGuid, string passwordOld, string passwordNew, string passwordNewConfirm)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            logicStatusInfo.IsSuccessful = BusinessUserBLL.ChangePassword(userGuid, passwordNew, passwordOld);

            PassCurrentUser();
            return View(logicStatusInfo);
        }

        #region 辅助方法
        private void PassCurrentUser()
        {
            string currentUserName = string.Empty;
            bool isAuthenticated = this.Request.RequestContext.HttpContext.User.Identity.IsAuthenticated;
            if (isAuthenticated == true)
            {
                currentUserName = this.Request.RequestContext.HttpContext.User.Identity.Name;
            }
            BusinessUser currentUser = BusinessUserBLL.Get(currentUserName);
            this.ViewBag.CurrentUser = currentUser;
        }
        #endregion
    }
}
