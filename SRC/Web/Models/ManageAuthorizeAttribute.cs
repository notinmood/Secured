//using System.Web.Mvc;
//using System.Web.Security;
//using HiLand.Framework.BusinessCore.BLL;
//using HiLand.Framework.BusinessCore;
//using HiLand.Utility.Enums;
//using HiLand.Framework.Membership;

//namespace GBFinance.Web.Models
//{
//    public class ManageAuthorizeAttribute : AuthorizeAttribute
//    {
//        public override void OnAuthorization(AuthorizationContext filterContext)
//        {
//            bool isAllowed = false;

//            UserCookie userCookie = new UserCookie();
//            userCookie.Load();

//            if (userCookie != null && string.IsNullOrWhiteSpace(userCookie.UserName)==false)
//            {
//                BusinessUser user = BusinessUserBLL.Get(userCookie.UserName);
//                if (user != null && (user.UserType == UserTypes.SuperAdmin || user.UserType == UserTypes.Manager))
//                {
//                    isAllowed = true;
//                }
//            }

//            if (isAllowed == false)
//            {
//                FormsAuthentication.RedirectToLoginPage();
//            }
//        }
//    }
//}