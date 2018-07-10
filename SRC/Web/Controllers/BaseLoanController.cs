using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web.Mvc;
using GBFinance.BLL;
using GBFinance.Entity;
using GBFinance.Web.Models;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.General;
using HiLand.General.BLL;
using HiLand.General.DAL;
using HiLand.General.Entity;
using HiLand.General.Enums;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;
using HiLand.Utility.Finance;
using HiLand.Utility.Web;
using HiLand.Utility4.Data;
using HiLand.Utility4.MVC.Controls;
using HiLand.Utility4.MVC.Data;

namespace GBFinance.Web.Controllers
{
    /// <summary>
    /// 本类不向View公开，为SecuredLoan和UnSecuredLoan的基类，仅仅为子类提供公用方法。
    /// </summary>
    public class BaseLoanController : Controller
    {
        #region step -10 Pre-qualification
        [ActionName("Pre-qualification")]
        public ActionResult PreQualification()
        {
            ResetCurrentLoanCookie();
            SetCurrentLoanStep(0);
            return View("PreQualification");
        }
        #endregion

        #region step0
        public ActionResult Index()
        {
            LoanCookie loanCookie = LoadCurrentLoanCookie();
            bool isNewBirthLoan = loanCookie.CheckIsNewBirthLoan();
            this.ViewData["isNewBirthLoan"] = isNewBirthLoan;
            return View();
        }

        [HttpPost]
        public ActionResult Index(bool resetLoan = false)
        {
            string result = RequestHelper.GetValue("resetLoan");
            if (result == "resetLoan")
            {
                ResetCurrentLoanCookie();
            }

            SetCurrentLoanStep(5);
            return View();
        }
        #endregion

        #region step1
        public ActionResult InitialLoanInformation()
        {
            SetCurrentLoanLayout();
            return View();
        }

        public ActionResult UInitialLoanInformation()
        {
            LoanSimpleInfo loanSimpleInfo = Miscs.CurrentLoanSimpleInfo;
            LoanBasicEntity entity = null;
            if (loanSimpleInfo.LoanGuid == Guid.Empty)
            {
                entity = new LoanBasicEntity();
            }
            else
            {
                entity = LoanBasicBLL.Instance.Get(loanSimpleInfo.LoanGuid);
            }

            int LoansAmountMax = 0;
            int LoansAmountMin = 0;
            if (loanSimpleInfo.loanType == LoanTypes.Secured)
            {
                LoansAmountMax = (int)LoanBasicSetting.SecuredLoansAmountMax;
                LoansAmountMin = (int)LoanBasicSetting.SecuredLoansAmountMin;
            }
            else
            {
                LoansAmountMax = (int)LoanBasicSetting.UnSecuredLoansAmountMax;
                LoansAmountMin = (int)LoanBasicSetting.UnSecuredLoansAmountMin;
            }

            this.PassParam("LoansAmountMax", LoansAmountMax);
            this.PassParam("LoansAmountMin", LoansAmountMin);

            this.BrokeLoanParam();
            return View(entity);
        }

        [HttpPost]
        public ActionResult UInitialLoanInformation(LoanBasicEntity entity)
        {
            var DataObject = new LogicStatusInfo();
            LoanSimpleInfo loanSimpleInfo = Miscs.CurrentLoanSimpleInfo;

            int LoansAmountMax = 0;
            int LoansAmountMin = 0;
            if (loanSimpleInfo.loanType == LoanTypes.Secured)
            {
                LoansAmountMax = (int)LoanBasicSetting.SecuredLoansAmountMax;
                LoansAmountMin = (int)LoanBasicSetting.SecuredLoansAmountMin;
            }
            else
            {
                LoansAmountMax = (int)LoanBasicSetting.UnSecuredLoansAmountMax;
                LoansAmountMin = (int)LoanBasicSetting.UnSecuredLoansAmountMin;
            }

            if (entity.LoanAmount > LoansAmountMax || entity.LoanAmount < LoansAmountMin)
            {
                DataObject.IsSuccessful = false;
                DataObject.Message = string.Format("The loan amount should be between ${0} to ${1}.", LoansAmountMin, LoansAmountMax);
            }
            else
            {

                bool isNeedSaveCookie = false;
                bool isNewLoan = false;

                Guid loanGuid = Guid.Empty;
                Guid ownerGuid = Guid.Empty;
                string ownerAddon = Guid.Empty.ToString();

                if (entity.LoanGuid == Guid.Empty)
                {
                    isNewLoan = true;

                    if (loanSimpleInfo.LoanGuid == Guid.Empty)
                    {
                        loanGuid = Guid.NewGuid();
                        isNeedSaveCookie = true;
                    }
                    else
                    {
                        loanGuid = loanSimpleInfo.LoanGuid;
                    }
                    entity.LoanGuid = loanGuid;
                }
                else
                {
                    isNewLoan = false;

                    LoanBasicEntity entityLoaded = LoanBasicBLL.Instance.Get(entity.LoanGuid);
                    entity.CheckDate = entityLoaded.CheckDate;
                    entity.CheckUserID = entityLoaded.CheckUserID;
                    entity.LoanDate = entityLoaded.LoanDate;
                    entity.LoanInterest = entityLoaded.LoanInterest;
                    entity.LoanStatus = entityLoaded.LoanStatus;
                    entity.LoanTermType = entityLoaded.LoanTermType;
                    entity.LoanType = entityLoaded.LoanType;
                    entity.PropertyNames = entityLoaded.PropertyNames;
                    entity.PropertyValues = entityLoaded.PropertyValues;
                }

                if (string.IsNullOrWhiteSpace(entity.LoanOwnerKey) || GuidHelper.IsInvalidOrEmpty(entity.LoanOwnerKey))
                {
                    if (loanSimpleInfo.OwnerGuid == Guid.Empty)
                    {
                        ownerGuid = Guid.NewGuid();
                        isNeedSaveCookie = true;
                    }
                    else
                    {
                        ownerGuid = loanSimpleInfo.OwnerGuid;
                    }

                    entity.LoanOwnerKey = ownerGuid.ToString();
                    entity.LoanOwnerType = LoanOwnerTypes.Person;
                }

                if (GuidHelper.IsInvalidOrEmpty(loanSimpleInfo.OwnerAddon))
                {
                    ownerAddon = GuidHelper.NewGuidString();
                }
                else
                {
                    ownerAddon = loanSimpleInfo.OwnerAddon.ToString();
                }

                if (isNeedSaveCookie == true && loanSimpleInfo.DataUsingMode == DataUsingModes.EndUserMode)
                {
                    LoanCookie loanCookie = LoadCurrentLoanCookie();
                    loanCookie.LoanGuid = loanGuid.ToString();
                    loanCookie.OwnerGuid = ownerGuid.ToString();
                    loanCookie.OwnerAddon = ownerAddon;
                    loanCookie.Save();
                }

                bool isSuccessful = false;
                if (isNewLoan == true)
                {
                    entity.LoanTermType = PaymentTermTypes.Monthly;
                    entity.LoanDate = DateTimeHelper.RunningLocalNow; //DateTime.Now;
                    entity.LoanInterest = GetCurrentLoanRate();
                    entity.LoanStatus = LoanStatuses.UserUnCompleted;
                    entity.LoanType = Miscs.GetCurrentLoanType();
                    isSuccessful = LoanBasicBLL.Instance.Create(entity);
                }
                else
                {
                    isSuccessful = LoanBasicBLL.Instance.Update(entity);
                }

                SetCurrentLoanStep(10);
                DataObject.IsSuccessful = isSuccessful;
            }

            return Json(DataObject);
        }
        #endregion

        #region step2
        public ActionResult AboutTheApplicants()
        {
            SetCurrentLoanLayout();
            return View();
        }

        /// <summary>
        /// 企业负责人列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UApplicantList()
        {
            BrokeLoanParam();

            List<BusinessUser> userList = GetApplicantList();
            return View(userList);
        }

        [HttpPost]
        public ActionResult UApplicantList(Guid? entityGuidToDelete)
        {
            BrokeLoanParam();

            if (entityGuidToDelete.HasValue && entityGuidToDelete.Value != Guid.Empty)
            {
                BusinessUserBLL.DeleteUser(entityGuidToDelete.Value);
            }

            List<BusinessUser> userList = GetApplicantList();
            return View(userList);
        }

        public ActionResult UApplicantDetail(Guid? applicantGuid)
        {
            BusinessUser applicantUser = new BusinessUser();
            ResidentalEntity residentalEntity = new ResidentalEntity();
            ResidentalEntity residentalEntityPre = new ResidentalEntity();

            if (applicantGuid.HasValue == false)
            {
                //applicantUser = GetApplication();
                List<BusinessUser> userList = GetApplicantList();
                if (userList != null && userList.Count > 0)
                {
                    applicantUser = userList[0];
                }
            }
            else
            {
                applicantUser = BusinessUserBLL.Get(applicantGuid.Value);
            }

            if (applicantUser.UserGuid != Guid.Empty)
            {
                string whereClause = string.Format("ResidentalUserGuid='{0}'", applicantUser.UserGuid);
                List<ResidentalEntity> residentalList = ResidentalBLL.Instance.GetList(whereClause);
                if (residentalList != null)
                {
                    if (residentalList.Count > 0)
                    {
                        residentalEntity = residentalList[0];
                    }

                    if (residentalList.Count > 1)
                    {
                        residentalEntityPre = residentalList[1];
                    }
                }
            }

            this.ViewBag.ResidentalEntity = residentalEntity;
            this.ViewBag.ResidentalEntityPre = residentalEntityPre;
            this.BrokeLoanParam();
            return View(applicantUser);
        }

        [HttpPost]
        public ActionResult UApplicantDetail(BusinessUser applicantUser)
        {
            LogicStatusInfo statusInfo = new LogicStatusInfo();
            ResidentalEntity residentalEntity = CollectResidentalInfo();
            ResidentalEntity residentalEntityPre = CollectResidentalInfo("Pre");

            string birthDayFromRequest = RequestHelper.GetValue("UserBirthDay");
            applicantUser.UserBirthDay = DateTimeHelper.Parse(birthDayFromRequest, DateFormats.DMY);

            //1.首先做输入验证
            if (string.IsNullOrWhiteSpace(applicantUser.UserTitle))
            {
                return FailureValidate("Title");
            }

            if (applicantUser.UserSex == Sexes.UnSet)
            {
                return FailureValidate("Gender");
            }

            if (string.IsNullOrWhiteSpace(applicantUser.UserNameFirst))
            {
                return FailureValidate("First Name");
            }

            if (string.IsNullOrWhiteSpace(applicantUser.UserNameLast))
            {
                return FailureValidate("Last Name");
            }

            if (applicantUser.UserBirthDay <= DateTimeHelper.Min)
            {
                return FailureValidate("Date of Birth", "the date format must is 'DD/MM/YYYY'.");
            }

            if (applicantUser.MaritalStatus == MaritalStatuses.UnSet || applicantUser.MaritalStatus == MaritalStatuses.All)
            {
                return FailureValidate("Marital Status");
            }

            if (string.IsNullOrWhiteSpace(applicantUser.DriversLicenceNumber))
            {
                return FailureValidate("Drivers Licence");
            }

            if (string.IsNullOrWhiteSpace(applicantUser.HomeTelephone))
            {
                return FailureValidate("Home Tel");
            }

            if (string.IsNullOrWhiteSpace(applicantUser.WorkTelphone))
            {
                return FailureValidate("Work Tel");
            }

            if (string.IsNullOrWhiteSpace(applicantUser.UserEmail))
            {
                return FailureValidate("Email");
            }

            if (string.IsNullOrWhiteSpace(residentalEntity.Street))
            {
                return FailureValidate("Street No.& name");
            }

            if (string.IsNullOrWhiteSpace(residentalEntity.Suburb))
            {
                return FailureValidate("Suburb");
            }

            if (string.IsNullOrWhiteSpace(residentalEntity.PostCode))
            {
                return FailureValidate("Post code");
            }

            if (residentalEntity.ResidentalYears == 0 && residentalEntity.ResidentalMonths == 0)
            {
                return FailureValidate("Address Years");
            }

            //2.保存逻辑
            if (applicantUser.UserGuid == Guid.Empty)
            {
                LoanSimpleInfo loanSimpleInfo = Miscs.CurrentLoanSimpleInfo;
                applicantUser.UserGuid = loanSimpleInfo.OwnerGuid;
                applicantUser.DepartmentCode = "outer";
                //applicantUser.EnterpriseKey = Miscs.CurrentLoanSimpleInfo.OwnerGuid;
                applicantUser.UserRegisterDate = DateTimeHelper.RunningLocalNow; //DateTime.Now;
                applicantUser.UserStatus = UserStatuses.Normal;
                applicantUser.UserType = UserTypes.CommonUser;
                applicantUser.UserName = Guid.NewGuid().ToString();

                CreateUserRoleStatuses createStatus = CreateUserRoleStatuses.Successful;
                BusinessUserBLL.CreateUser(applicantUser, out createStatus);
                if (createStatus == CreateUserRoleStatuses.Successful)
                {
                    statusInfo.IsSuccessful = true;
                    statusInfo.Message = string.Empty;
                }
                else
                {
                    statusInfo.IsSuccessful = false;
                    statusInfo.Message = createStatus.ToString();
                }
            }
            else
            {
                BusinessUser originalUser = BusinessUserBLL.Get(applicantUser.UserGuid);
                applicantUser.DepartmentCode = originalUser.DepartmentCode;
                applicantUser.DepartmentGuid = originalUser.DepartmentGuid;
                applicantUser.UserRegisterDate = originalUser.UserRegisterDate;
                applicantUser.UserStatus = originalUser.UserStatus;
                applicantUser.UserType = originalUser.UserType;
                applicantUser.UserName = originalUser.UserName;

                bool isSuccessful = BusinessUserBLL.UpdateUser(applicantUser);
                if (isSuccessful == true)
                {
                    statusInfo.IsSuccessful = true;
                    statusInfo.Message = string.Empty;
                }
                else
                {
                    statusInfo.IsSuccessful = false;
                    statusInfo.Message = "error in updating";
                }
            }

            string residentalGuid = RequestHelper.GetValue("ResidentalGuid");
            if (string.IsNullOrWhiteSpace(residentalGuid) == false)
            {
                residentalEntity.ResidentialGuid = new Guid(residentalGuid);
            }

            string residentalGuidPre = RequestHelper.GetValue("ResidentalGuidPre");
            if (string.IsNullOrWhiteSpace(residentalGuidPre) == false)
            {
                residentalEntityPre.ResidentialGuid = new Guid(residentalGuidPre);
            }

            string residentalStatusString = RequestHelper.GetValue("ResidentialStatus");
            residentalEntity.ResidentialStatus = Converter.TryToEnum<ResidentalTypes>(residentalStatusString, ResidentalTypes.OwnHomeWithMortgage);

            residentalEntity.ResidentalUserGuid = applicantUser.UserGuid;
            residentalEntityPre.ResidentalUserGuid = applicantUser.UserGuid;

            if (string.IsNullOrWhiteSpace(residentalGuid) == true || residentalGuid == Guid.Empty.ToString())
            {
                ResidentalBLL.Instance.Create(residentalEntity);
            }
            else
            {
                ResidentalBLL.Instance.Update(residentalEntity);
            }

            if (string.IsNullOrWhiteSpace(residentalGuidPre) == true || residentalGuidPre == Guid.Empty.ToString())
            {
                ResidentalBLL.Instance.Create(residentalEntityPre);
            }
            else
            {
                ResidentalBLL.Instance.Update(residentalEntityPre);
            }

            SetCurrentLoanStep(20);

            return Json(statusInfo);
        }

        #region 辅助方法
        //private BusinessUser GetApplication()
        //{
        //    BusinessUser applicantUser = BusinessUserBLL.Get(Miscs.CurrentLoanSimpleInfo.OwnerGuid);
        //    return applicantUser;
        //}

        private List<BusinessUser> GetApplicantList()
        {
            List<BusinessUser> userList = new List<BusinessUser>();  //BusinessUserBLL.GetUsersByDepartment(Miscs.CurrentLoanSimpleInfo.OwnerGuid);
            BusinessUser applicantUser = BusinessUserBLL.Get(Miscs.CurrentLoanSimpleInfo.OwnerGuid);
            if (applicantUser != null)
            {
                userList.Add(applicantUser);
            }
            return userList;
        }

        private ResidentalEntity CollectResidentalInfo(string prefix = "")
        {
            ResidentalEntity residentalEntity = new ResidentalEntity();
            residentalEntity.ApartmentNo = RequestHelper.GetValue(prefix + "UnitApartmentNo");
            residentalEntity.PostCode = RequestHelper.GetValue(prefix + "Postcode");

            int residentalMonthes = 0;
            int residentalYears = 0;
            int.TryParse(RequestHelper.GetValue(prefix + "AddressMonths"), out residentalMonthes);
            int.TryParse(RequestHelper.GetValue(prefix + "AddressYears"), out residentalYears);
            residentalEntity.ResidentalMonths = residentalMonthes;
            residentalEntity.ResidentalYears = residentalYears;

            residentalEntity.State = RequestHelper.GetValue(prefix + "State");
            residentalEntity.Street = RequestHelper.GetValue(prefix + "StreetNoAndName");
            residentalEntity.Suburb = RequestHelper.GetValue(prefix + "Suburb");

            return residentalEntity;
        }
        #endregion
        #endregion

        #region step3
        public ActionResult AboutYourBusiness()
        {
            SetCurrentLoanLayout();
            return View();
        }


        public ActionResult UAboutYourBusiness()
        {
            BrokeLoanParam();

            EnterpriseEntity enterpriseEntity = new EnterpriseEntity();
            EnterpriseEntity enterpriseRef1 = new EnterpriseEntity();
            EnterpriseEntity enterpriseRef2 = new EnterpriseEntity();

            Guid enterpriseGuid = Miscs.CurrentLoanSimpleInfo.OwnerGuid;

            if (enterpriseGuid != Guid.Empty)
            {
                enterpriseEntity = EnterpriseBLL.Instance.Get(enterpriseGuid);
                if (enterpriseEntity == null)
                {
                    enterpriseEntity = new EnterpriseEntity();
                }
                string whereClause = string.Format("AssociatedEnterpriseGuid='{0}'", enterpriseGuid.ToString());
                List<EnterpriseEntity> enterpriseRefList = EnterpriseBLL.Instance.GetList(whereClause);
                if (enterpriseRefList != null)
                {
                    if (enterpriseRefList.Count > 0)
                    {
                        enterpriseRef1 = enterpriseRefList[0];
                    }

                    if (enterpriseRefList.Count > 1)
                    {
                        enterpriseRef2 = enterpriseRefList[1];
                    }
                }
            }

            this.ViewBag.EnterpriseRef1 = enterpriseRef1;
            this.ViewBag.EnterpriseRef2 = enterpriseRef2;

            return View(enterpriseEntity);
        }

        [HttpPost]
        public ActionResult UAboutYourBusiness(EnterpriseEntity entity)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();

            Guid enterpriseGuid = Miscs.CurrentLoanSimpleInfo.OwnerGuid;

            EnterpriseEntity enterpriseRef1 = CollecteEnterpriseRef("Ref1");
            EnterpriseEntity enterpriseRef2 = CollecteEnterpriseRef("Ref2");

            enterpriseRef1.AssociatedEnterpriseGuid = enterpriseGuid;
            enterpriseRef2.AssociatedEnterpriseGuid = enterpriseGuid;


            if (entity.EnterpriseID == 0)
            {
                entity.EnterpriseGuid = enterpriseGuid;
                logicStatusInfo.IsSuccessful = EnterpriseBLL.Instance.Create(entity);
            }
            else
            {
                logicStatusInfo.IsSuccessful = EnterpriseBLL.Instance.Update(entity);
            }

            if (enterpriseRef1.EnterpriseGuid == Guid.Empty)
            {
                enterpriseRef1.EnterpriseGuid = Guid.NewGuid();
                EnterpriseBLL.Instance.Create(enterpriseRef1);
            }
            else
            {
                EnterpriseBLL.Instance.Update(enterpriseRef1);
            }

            if (enterpriseRef2.EnterpriseGuid == Guid.Empty)
            {
                enterpriseRef2.EnterpriseGuid = Guid.NewGuid();
                EnterpriseBLL.Instance.Create(enterpriseRef2);
            }
            else
            {
                EnterpriseBLL.Instance.Update(enterpriseRef2);
            }

            SetCurrentLoanStep(30);

            return Json(logicStatusInfo);
        }

        #region 辅助方法
        private EnterpriseEntity CollecteEnterpriseRef(string postFix)
        {
            EnterpriseEntity entity = new EnterpriseEntity();
            string enterpriseGuidRef = RequestHelper.GetValue("EnterpriseGuid" + postFix);
            if (string.IsNullOrWhiteSpace(enterpriseGuidRef))
            {
                entity.EnterpriseGuid = Guid.Empty;
            }
            else
            {
                entity.EnterpriseGuid = new Guid(enterpriseGuidRef);
            }

            entity.ContactPerson = RequestHelper.GetValue("ContactName" + postFix);
            entity.CompanyName = RequestHelper.GetValue("CompanyName" + postFix);
            entity.Telephone = RequestHelper.GetValue("Telephone" + postFix);

            return entity;
        }
        #endregion
        #endregion

        #region step3B (此方法应用于个人抵押部分)
        public ActionResult AboutYourWorks()
        {
            SetCurrentLoanLayout();
            return View();
        }


        public ActionResult UAboutYourWorks()
        {
            BrokeLoanParam();

            LoanWorkEntity loanWorksEntity = new LoanWorkEntity();
            List<LoanWorkEntity> loanWorksList = LoanWorkBLL.Instance.GetList(string.Format("UserGuid='{0}'", Miscs.CurrentLoanSimpleInfo.OwnerGuid.ToString()));
            if (loanWorksList != null && loanWorksList.Count > 0)
            {
                loanWorksEntity = loanWorksList[0];
            }

            return View("UAboutYourBusiness", loanWorksEntity);
        }

        [HttpPost]
        public ActionResult UAboutYourWorks(LoanWorkEntity entity)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            Guid enterpriseGuid = Miscs.CurrentLoanSimpleInfo.OwnerGuid;

            //0.输入验证
            if (string.IsNullOrWhiteSpace(entity.EmployerName))
            {
                return FailureValidate("Employer Name");
            }

            if (string.IsNullOrWhiteSpace(entity.EmployerAddress))
            {
                return FailureValidate("Employer Address");
            }

            if (string.IsNullOrWhiteSpace(entity.EmployerName))
            {
                return FailureValidate("Employer Name");
            }

            if (string.IsNullOrWhiteSpace(entity.EmployerTelephone))
            {
                return FailureValidate("Employer Telephone");
            }

            if (string.IsNullOrWhiteSpace(entity.JobTitle))
            {
                return FailureValidate("Job Title");
            }

            if (entity.TimeEmployedYears == 0 && entity.TimeEmployedMonths == 0)
            {
                return FailureValidate("Time Employed");
            }

            if (entity.IncomeafterTaxes == 0)
            {
                return FailureValidate("Take Home Pay After Taxes");
            }

            string nextIncomeDayFromRequest = RequestHelper.GetValue("NextIncomeDay");
            entity.NextIncomeDay = DateTimeHelper.Parse(nextIncomeDayFromRequest, DateFormats.DMY);
            if (entity.NextIncomeDay <= DateTimeHelper.Min)
            {
                return FailureValidate("Next Pay Day");
            }

            //1.保存企业信息
            if (enterpriseGuid != Guid.Empty)
            {
                EnterpriseEntity enterpriseEntity = EnterpriseBLL.Instance.Get(enterpriseGuid);
                if (enterpriseEntity == null)
                {
                    enterpriseEntity = new EnterpriseEntity();
                    enterpriseEntity.EnterpriseGuid = Miscs.CurrentLoanSimpleInfo.OwnerGuid;
                    EnterpriseBLL.Instance.Create(enterpriseEntity);
                }
            }

            //2.保存工作信息
            if (entity.WorkID == 0)
            {
                entity.WorkGuid = Guid.NewGuid();
                entity.UserGuid = Miscs.CurrentLoanSimpleInfo.OwnerGuid;
                logicStatusInfo.IsSuccessful = LoanWorkBLL.Instance.Create(entity);
            }
            else
            {
                entity.UserGuid = Miscs.CurrentLoanSimpleInfo.OwnerGuid;
                logicStatusInfo.IsSuccessful = LoanWorkBLL.Instance.Update(entity);
            }

            SetCurrentLoanStep(35);
            return Json(logicStatusInfo);
        }
        #endregion

        #region step4
        public ActionResult LiabilityStatement()
        {
            SetCurrentLoanLayout();
            return View();
        }

        public ActionResult ULiabilityStatementWrapper()
        {
            BrokeLoanParam();
            return View();
        }

        public ActionResult ULiabilityStatementList()
        {
            BrokeLoanParam();

            List<LiabilityEntity> entityList = GetLiabilityList();
            return View(entityList);
        }

        [HttpPost]
        public ActionResult ULiabilityStatementList(Guid? entityGuidToDelete)
        {
            BrokeLoanParam();

            if (entityGuidToDelete.HasValue && entityGuidToDelete.Value != Guid.Empty)
            {
                LiabilityBLL.Instance.Delete(entityGuidToDelete.Value);
            }

            List<LiabilityEntity> entityList = GetLiabilityList();
            return View(entityList);
        }

        /// <summary>
        /// 删除某条记录
        /// </summary>
        /// <param name="entityGuidToDelete"></param>
        /// <returns></returns>
        public JsonResult ULiabilityStatementDelete(Guid? entityGuidToDelete)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            if (entityGuidToDelete.HasValue && entityGuidToDelete.Value != Guid.Empty)
            {
                logicStatusInfo.IsSuccessful = LiabilityBLL.Instance.Delete(entityGuidToDelete.Value);
            }

            return Json(logicStatusInfo);
        }

        private List<LiabilityEntity> GetLiabilityList()
        {
            List<LiabilityEntity> entityList = new List<LiabilityEntity>();
            string whereCluase = string.Format("UserGuid='{0}'", Miscs.CurrentLoanSimpleInfo.OwnerGuid.ToString());
            entityList = LiabilityBLL.Instance.GetList(whereCluase);
            return entityList;
        }

        [HttpPost]
        public ActionResult ULiabilityStatementDeleteRow(string itemIDToDelete)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            if (string.IsNullOrWhiteSpace(itemIDToDelete) == false && itemIDToDelete != Guid.Empty.ToString())
            {
                logicStatusInfo.IsSuccessful = LiabilityBLL.Instance.Delete(new Guid(itemIDToDelete));
            }

            return Json(logicStatusInfo);
        }

        /// <summary>
        /// 给列表添加行
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="entity"></param>
        /// <param name="usingModeInput"></param>
        /// <returns></returns>
        public ActionResult ULiabilityStatementAddRow(int rowNumber, LiabilityEntity entity, MvcControlUsingModes? usingModeInput)
        {
            MvcControlUsingModes usingMode = MvcControlUsingModes.Editable;
            if (usingModeInput.HasValue)
            {
                usingMode = usingModeInput.Value;
            }
            if (entity == null)
            {
                entity = new LiabilityEntity();
            }

            string LiabilityGuidString = ControlsHtmlHelper.HiHidden().Name("LiabilityGuid_" + rowNumber).Value(entity.LiabilityGuid).UsingMode(usingMode).Render().ToString();
            string LiabilityLenderInfoString = ControlsHtmlHelper.HiTextBox().Name("LiabilityLenderInfo_" + rowNumber).Value(entity.LiabilityLenderInfo).UsingMode(usingMode).Render().ToString();
            string LiabilityAmountOwingString = ControlsHtmlHelper.HiTextBox().Name("LiabilityAmountOwing_" + rowNumber).Value(entity.LiabilityAmountOwing.ToString()).UsingMode(usingMode).Render().ToString();
            string LiabilityPaymentMonthlyString = ControlsHtmlHelper.HiTextBox().Name("LiabilityPaymentMonthly_" + rowNumber).Value(entity.LiabilityPaymentMonthly.ToString()).UsingMode(usingMode).Render().ToString();
            string OpereationString = string.Empty;
            if (entity.LiabilityGuid != Guid.Empty)
            {
                OpereationString = string.Format("<a itemGuid=\"{0}\" class=\"operateEntry deleteEntry\">DELETE</a>", entity.LiabilityGuid);
            }

            string result = string.Format("<tr><td>{0} {1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td>", rowNumber + 1, LiabilityGuidString, LiabilityLenderInfoString, LiabilityAmountOwingString, LiabilityPaymentMonthlyString, OpereationString);
            return Content(result);
        }

        public ActionResult ULiabilityStatementDetails()
        {
            string whereClause = string.Format(" UserGuid='{0}' ", Miscs.CurrentLoanSimpleInfo.OwnerGuid);
            List<LiabilityEntity> entityList = LiabilityBLL.Instance.GetList(whereClause);

            BrokeLoanParam();
            return View(entityList);
        }

        [HttpPost]
        public ActionResult ULiabilityStatementDetails(bool? onlyPlaceHolder)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();

            int rowCountToFeedback = 0;
            string rowCountToFeedbackString = RequestHelper.GetValue("rowCountToFeedback");
            if (string.IsNullOrWhiteSpace(rowCountToFeedbackString) == false)
            {
                rowCountToFeedback = Converter.TryToInt32(rowCountToFeedbackString);
            }

            if (rowCountToFeedback > 0)
            {
                for (int i = 0; i < rowCountToFeedback; i++)
                {
                    bool isNewEntity = false;
                    bool isNeedSave = false;
                    LiabilityEntity entity = new LiabilityEntity();
                    string LiabilityGuidString = RequestHelper.GetValue("LiabilityGuid_" + i);
                    if (string.IsNullOrWhiteSpace(LiabilityGuidString) || LiabilityGuidString == Guid.Empty.ToString())
                    {
                        isNewEntity = true;
                        entity.LiabilityGuid = Guid.NewGuid();
                        entity.UserGuid = Miscs.CurrentLoanSimpleInfo.OwnerGuid;
                    }
                    else
                    {
                        isNewEntity = false;
                        isNeedSave = true;
                        Guid LiabilityGuid = new Guid(LiabilityGuidString);
                        entity = LiabilityBLL.Instance.Get(LiabilityGuid);
                    }

                    string LiabilityLenderInfoString = RequestHelper.GetValue("LiabilityLenderInfo_" + i);
                    if (string.IsNullOrWhiteSpace(LiabilityLenderInfoString) == false)
                    {
                        isNeedSave = true;
                        entity.LiabilityLenderInfo = LiabilityLenderInfoString;
                    }

                    string LiabilityAmountOwingString = RequestHelper.GetValue("LiabilityAmountOwing_" + i);
                    if (string.IsNullOrWhiteSpace(LiabilityAmountOwingString) == false)
                    {
                        decimal LiabilityAmountOwing = Converter.TryToDecimal(LiabilityAmountOwingString);
                        if (LiabilityAmountOwing != 0)
                        {
                            isNeedSave = true;
                            entity.LiabilityAmountOwing = LiabilityAmountOwing;
                        }
                    }

                    string LiabilityPaymentMonthlyString = RequestHelper.GetValue("LiabilityPaymentMonthly_" + i);
                    if (string.IsNullOrWhiteSpace(LiabilityPaymentMonthlyString) == false)
                    {
                        decimal LiabilityPaymentMonthly = Converter.TryToDecimal(LiabilityPaymentMonthlyString);
                        if (LiabilityPaymentMonthly != 0)
                        {
                            isNeedSave = true;
                            entity.LiabilityPaymentMonthly = LiabilityPaymentMonthly;
                        }
                    }

                    if (isNeedSave == true)
                    {
                        if (isNewEntity == true)
                        {
                            LiabilityBLL.Instance.Create(entity);
                        }
                        else
                        {
                            LiabilityBLL.Instance.Update(entity);
                        }
                    }
                }
            }

            logicStatusInfo.IsSuccessful = true;
            SetCurrentLoanStep(40);

            return Json(logicStatusInfo);
        }
        #endregion

        #region step5
        public ActionResult AssetStatement()
        {
            SetCurrentLoanLayout();
            return View();
        }

        public ActionResult UAssetStatementWrapper()
        {
            BrokeLoanParam();
            return View();
        }

        public ActionResult UAssetStatementList()
        {
            BrokeLoanParam();

            List<IncomeAssetEntity> entityList = GetAssetStatementList();
            return View(entityList);
        }

        [HttpPost]
        public ActionResult UAssetStatementList(Guid? entityGuidToDelete)
        {
            BrokeLoanParam();

            if (entityGuidToDelete.HasValue && entityGuidToDelete.Value != Guid.Empty)
            {
                IncomeAssetBLL.Instance.Delete(entityGuidToDelete.Value);
            }

            List<IncomeAssetEntity> entityList = GetAssetStatementList();
            return View(entityList);
        }

        /// <summary>
        /// 删除某条记录
        /// </summary>
        /// <param name="entityGuidToDelete"></param>
        /// <returns></returns>
        public JsonResult UAssetStatementDelete(Guid? entityGuidToDelete)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            if (entityGuidToDelete.HasValue && entityGuidToDelete.Value != Guid.Empty)
            {
                logicStatusInfo.IsSuccessful = IncomeAssetBLL.Instance.Delete(entityGuidToDelete.Value);
            }

            return Json(logicStatusInfo);
        }


        private List<IncomeAssetEntity> GetAssetStatementList()
        {
            List<IncomeAssetEntity> entityList = new List<IncomeAssetEntity>();

            string whereCluase = string.Format("UserGuid='{0}'", Miscs.CurrentLoanSimpleInfo.OwnerGuid.ToString());
            entityList = IncomeAssetBLL.Instance.GetList(whereCluase);
            return entityList;
        }

        [HttpPost]
        public ActionResult UAssetStatementDeleteRow(string itemIDToDelete)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            if (string.IsNullOrWhiteSpace(itemIDToDelete) == false && itemIDToDelete != Guid.Empty.ToString())
            {
                logicStatusInfo.IsSuccessful = IncomeAssetBLL.Instance.Delete(new Guid(itemIDToDelete));
            }

            return Json(logicStatusInfo);
        }

        /// <summary>
        /// 给列表添加行
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="entity"></param>
        /// <param name="usingModeInput"></param>
        /// <returns></returns>
        public ActionResult UAssetStatementAddRow(int rowNumber, IncomeAssetEntity entity, MvcControlUsingModes? usingModeInput)
        {
            MvcControlUsingModes usingMode = MvcControlUsingModes.Editable;
            if (usingModeInput.HasValue)
            {
                usingMode = usingModeInput.Value;
            }
            if (entity == null)
            {
                entity = new IncomeAssetEntity();
            }

            string itemGuidString = ControlsHtmlHelper.HiHidden().Name("ItemGuid_" + rowNumber).Value(entity.ItemGuid.ToString()).UsingMode(usingMode).Render().ToString();
            string ItemNameString = ControlsHtmlHelper.HiTextBox().Name("ItemName_" + rowNumber).Value(entity.ItemName).UsingMode(usingMode).Render().ToString();
            string ItemValueString = ControlsHtmlHelper.HiTextBox().Name("ItemValue_" + rowNumber).Value(entity.ItemValue.ToString()).UsingMode(usingMode).Render().ToString();
            string OpereationString = string.Empty;
            if (entity.ItemGuid != Guid.Empty)
            {
                OpereationString = string.Format("<a itemGuid=\"{0}\" class=\"operateEntry deleteEntry\">DELETE</a>", entity.ItemGuid);
            }

            string result = string.Format("<tr><td>{0} {1}</td><td>{2}</td><td>{3}</td><td>{4}</td>", rowNumber + 1, itemGuidString, ItemNameString, ItemValueString, OpereationString);
            return Content(result);
        }

        public ActionResult UAssetStatementDetails()
        {
            string whereClause = string.Format(" UserGuid='{0}' ", Miscs.CurrentLoanSimpleInfo.OwnerGuid);
            List<IncomeAssetEntity> entityList = IncomeAssetBLL.Instance.GetList(whereClause);

            BrokeLoanParam();

            return View(entityList);
        }

        [HttpPost]
        public ActionResult UAssetStatementDetails(bool? onlyPlaceHolder)
        {
            BrokeLoanParam();

            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();

            int rowCountToFeedback = 0;
            string rowCountToFeedbackString = RequestHelper.GetValue("rowCountToFeedback");
            if (string.IsNullOrWhiteSpace(rowCountToFeedbackString) == false)
            {
                rowCountToFeedback = Converter.TryToInt32(rowCountToFeedbackString);
            }

            if (rowCountToFeedback > 0)
            {
                for (int i = 0; i < rowCountToFeedback; i++)
                {
                    bool isNewEntity = false;
                    bool isNeedSave = false;
                    IncomeAssetEntity entity = new IncomeAssetEntity();
                    string ItemGuidString = RequestHelper.GetValue("ItemGuid_" + i);
                    if (string.IsNullOrWhiteSpace(ItemGuidString) || ItemGuidString == Guid.Empty.ToString())
                    {
                        isNewEntity = true;
                        entity.ItemGuid = Guid.NewGuid();
                        entity.UserGuid = Miscs.CurrentLoanSimpleInfo.OwnerGuid;
                    }
                    else
                    {
                        isNewEntity = false;
                        isNeedSave = true;
                        Guid ItemGuid = new Guid(ItemGuidString);
                        entity = IncomeAssetBLL.Instance.Get(ItemGuid);
                    }

                    string ItemNameString = RequestHelper.GetValue("ItemName_" + i);
                    if (string.IsNullOrWhiteSpace(ItemNameString) == false)
                    {
                        isNeedSave = true;
                        entity.ItemName = ItemNameString;
                    }

                    string ItemValueString = RequestHelper.GetValue("ItemValue_" + i);
                    if (string.IsNullOrWhiteSpace(ItemValueString) == false)
                    {
                        decimal ItemValue = Converter.TryToDecimal(ItemValueString);
                        if (ItemValue != 0)
                        {
                            isNeedSave = true;
                            entity.ItemValue = ItemValue;
                        }
                    }

                    if (isNeedSave == true)
                    {
                        if (isNewEntity == true)
                        {
                            IncomeAssetBLL.Instance.Create(entity);
                        }
                        else
                        {
                            IncomeAssetBLL.Instance.Update(entity);
                        }
                    }
                }
            }

            logicStatusInfo.IsSuccessful = true;
            SetCurrentLoanStep(50);
            return Json(logicStatusInfo);
        }
        #endregion

        #region step6
        public ActionResult AboutTheSecurities()
        {
            SetCurrentLoanLayout();
            return View();
        }

        [HttpPost]
        public ActionResult UAboutTheSecuritiesDeleteRow(string itemIDToDelete)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            if (string.IsNullOrWhiteSpace(itemIDToDelete) == false && itemIDToDelete != Guid.Empty.ToString())
            {
                logicStatusInfo.IsSuccessful = SecuredPropertyBLL.Instance.Delete(new Guid(itemIDToDelete));
            }

            return Json(logicStatusInfo);
        }

        /// <summary>
        /// 给列表添加行
        /// </summary>
        /// <param name="rowNumber"></param>
        /// <param name="entity"></param>
        /// <param name="usingModeInput"></param>
        /// <returns></returns>
        public ActionResult UAboutTheSecuritiesAddRow(int rowNumber, SecuredPropertyEntity entity, MvcControlUsingModes? usingModeInput)
        {
            MvcControlUsingModes usingMode = MvcControlUsingModes.Editable;
            if (usingModeInput.HasValue)
            {
                usingMode = usingModeInput.Value;
            }
            if (entity == null)
            {
                entity = new SecuredPropertyEntity();
            }

            string ItemGuidString = ControlsHtmlHelper.HiHidden().Name("ItemGuid_" + rowNumber).Value(entity.ItemGuid).UsingMode(usingMode).Render().ToString();
            string ItemDetailString = ControlsHtmlHelper.HiTextBox().Name("ItemDetail_" + rowNumber).Value(entity.ItemDetail).UsingMode(usingMode).Render().ToString();
            string ItemOwerNameString = ControlsHtmlHelper.HiTextBox().Name("ItemOwerName_" + rowNumber).Value(entity.ItemOwerName).UsingMode(usingMode).Render().ToString();
            string ItemValueString = ControlsHtmlHelper.HiTextBox().Name("ItemValue_" + rowNumber).Value(entity.ItemValue.ToString()).UsingMode(usingMode).Render().ToString();
            string OpereationString = string.Empty;
            if (entity.ItemGuid != Guid.Empty)
            {
                OpereationString = string.Format("<a itemGuid=\"{0}\" class=\"operateEntry deleteEntry\">DELETE</a>", entity.ItemGuid);
            }

            string result = string.Format("<tr><td>{0} {1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td>", rowNumber + 1, ItemGuidString, ItemDetailString, ItemOwerNameString, ItemValueString, OpereationString);
            return Content(result);
        }

        public ActionResult UAboutTheSecuritiesDetails()
        {
            string whereClause = string.Format(" UserGuid='{0}' ", Miscs.CurrentLoanSimpleInfo.OwnerGuid);
            List<SecuredPropertyEntity> entityList = SecuredPropertyBLL.Instance.GetList(whereClause);

            BrokeLoanParam();
            return View(entityList);
        }

        [HttpPost]
        public ActionResult UAboutTheSecuritiesDetails(bool? onlyPlaceHolder)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            int rowCountToFeedback = 0;
            string rowCountToFeedbackString = RequestHelper.GetValue("rowCountToFeedback");
            if (string.IsNullOrWhiteSpace(rowCountToFeedbackString) == false)
            {
                rowCountToFeedback = Converter.TryToInt32(rowCountToFeedbackString);
            }

            if (rowCountToFeedback > 0)
            {
                for (int i = 0; i < rowCountToFeedback; i++)
                {
                    bool isNewEntity = false;
                    bool isNeedSave = false;
                    SecuredPropertyEntity entity = new SecuredPropertyEntity();
                    string ItemGuidString = RequestHelper.GetValue("ItemGuid_" + i);
                    if (string.IsNullOrWhiteSpace(ItemGuidString) || ItemGuidString == Guid.Empty.ToString())
                    {
                        isNewEntity = true;
                        entity.ItemGuid = Guid.NewGuid();
                        entity.UserGuid = Miscs.CurrentLoanSimpleInfo.OwnerGuid;
                    }
                    else
                    {
                        isNewEntity = false;
                        isNeedSave = true;
                        Guid LiabilityGuid = new Guid(ItemGuidString);
                        entity = SecuredPropertyBLL.Instance.Get(LiabilityGuid);
                    }

                    string ItemDetailString = RequestHelper.GetValue("ItemDetail_" + i);
                    if (string.IsNullOrWhiteSpace(ItemDetailString) == false)
                    {
                        isNeedSave = true;
                        entity.ItemDetail = ItemDetailString;
                    }

                    string ItemOwerNameString = RequestHelper.GetValue("ItemOwerName_" + i);
                    if (string.IsNullOrWhiteSpace(ItemOwerNameString) == false)
                    {
                        isNeedSave = true;
                        entity.ItemOwerName = ItemOwerNameString;
                    }

                    string ItemValueString = RequestHelper.GetValue("ItemValue_" + i);
                    if (string.IsNullOrWhiteSpace(ItemValueString) == false)
                    {
                        decimal ItemValue = Converter.TryToDecimal(ItemValueString);
                        if (ItemValue != 0)
                        {
                            isNeedSave = true;
                            entity.ItemValue = ItemValue;
                        }
                    }

                    if (isNeedSave == true)
                    {
                        if (isNewEntity == true)
                        {
                            SecuredPropertyBLL.Instance.Create(entity);
                        }
                        else
                        {
                            SecuredPropertyBLL.Instance.Update(entity);
                        }
                    }
                }
            }

            logicStatusInfo.IsSuccessful = true;

            SetCurrentLoanStep(60);
            return Json(logicStatusInfo);
        }

        public ActionResult UAboutTheSecuritiesList()
        {
            BrokeLoanParam();

            List<SecuredPropertyEntity> securedPropertyList = GetSecuredPropertyList();
            return View(securedPropertyList);
        }

        [HttpPost]
        public ActionResult UAboutTheSecuritiesList(Guid? entityGuidToDelete)
        {
            BrokeLoanParam();

            if (entityGuidToDelete.HasValue && entityGuidToDelete.Value != Guid.Empty)
            {
                SecuredPropertyBLL.Instance.Delete(entityGuidToDelete.Value);
            }

            List<SecuredPropertyEntity> securedPropertyList = GetSecuredPropertyList();
            return View(securedPropertyList);
        }


        /// <summary>
        /// 删除某条记录
        /// </summary>
        /// <param name="entityGuidToDelete"></param>
        /// <returns></returns>
        public JsonResult UAboutTheSecuritiesDelete(Guid? entityGuidToDelete)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            if (entityGuidToDelete.HasValue && entityGuidToDelete.Value != Guid.Empty)
            {
                logicStatusInfo.IsSuccessful = SecuredPropertyBLL.Instance.Delete(entityGuidToDelete.Value);
            }

            return Json(logicStatusInfo);
        }

        private List<SecuredPropertyEntity> GetSecuredPropertyList()
        {
            List<SecuredPropertyEntity> securedPropertyList = new List<SecuredPropertyEntity>();
            string whereCluase = string.Format("UserGuid='{0}'", Miscs.CurrentLoanSimpleInfo.OwnerGuid.ToString());
            securedPropertyList = SecuredPropertyBLL.Instance.GetList(whereCluase);
            return securedPropertyList;
        }
        #endregion

        #region step7
        public ActionResult BusinessBankDetails()
        {
            SetCurrentLoanLayout();
            return View();
        }

        public ActionResult UBusinessBankDetails()
        {
            BrokeLoanParam();

            BankEntity bankEntity = new BankEntity();

            Guid enterpriseGuid = Miscs.CurrentLoanSimpleInfo.OwnerGuid;
            if (enterpriseGuid != Guid.Empty)
            {
                string whereClause = string.Format("UserGuid='{0}'", enterpriseGuid.ToString());
                List<BankEntity> bankList = BankBLL.Instance.GetList(whereClause);
                if (bankList != null && bankList.Count > 0)
                {
                    bankEntity = bankList[0];
                }
            }


            return View(bankEntity);
        }

        [HttpPost]
        public ActionResult UBusinessBankDetails(BankEntity bankEntity)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            bankEntity.UserGuid = Miscs.CurrentLoanSimpleInfo.OwnerGuid;

            //0.验证数据
            if (string.IsNullOrWhiteSpace(bankEntity.BankName))
            {
                return FailureValidate("Bank Name");
            }

            if (string.IsNullOrWhiteSpace(bankEntity.Branch))
            {
                return FailureValidate("Branch");
            }

            if (string.IsNullOrWhiteSpace(bankEntity.AccountName))
            {
                return FailureValidate("Account Name");
            }

            if (string.IsNullOrWhiteSpace(bankEntity.BankCode))
            {
                return FailureValidate("BSB");
            }

            if (string.IsNullOrWhiteSpace(bankEntity.AccountNumber))
            {
                return FailureValidate("Account Number");
            }

            if (bankEntity.AccountNumber != RequestHelper.GetValue("AccountNumberConfirm"))
            {
                return FailureValidate("Confirm Account Number");
            }

            //1.保存数据
            if (bankEntity.BankGuid == Guid.Empty)
            {
                bankEntity.BankGuid = Guid.NewGuid();

                logicStatusInfo.IsSuccessful = BankBLL.Instance.Create(bankEntity);
            }
            else
            {
                logicStatusInfo.IsSuccessful = BankBLL.Instance.Update(bankEntity);
            }

            SetCurrentLoanStep(70);

            return Json(logicStatusInfo);
        }
        #endregion

        #region step8
        public ActionResult ApplicationSummary()
        {
            SetCurrentLoanStep(80);
            SetCurrentLoanLayout();
            return View();
        }

        [HttpPost]
        public ActionResult ApplicationSummary(string onlyPlaceHolder)
        {
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo(true);
            return View(logicStatusInfo);
        }

        public ActionResult UApplicationSummary()
        {
            return View();
        }
        #endregion

        #region step9
        public ActionResult SubmitApplication()
        {
            SetCurrentLoanLayout();
            return View();
        }

        public ActionResult USubmitApplication()
        {
            return View();
        }

        [HttpPost]
        public ActionResult USubmitApplication(string signedFullName)
        {
            //SetCurrentLoanLayout();
            LogicStatusInfo logicStatusInfo = new LogicStatusInfo();
            if (string.IsNullOrWhiteSpace(signedFullName) == false)
            {
                //EnterpriseBLL enterpriseBLL = EnterpriseBLL.Instance;
                //Guid enterpriseGuid = Miscs.CurrentLoanSimpleInfo.OwnerGuid;
                //EnterpriseEntity enterpriseEntity = enterpriseBLL.Get(enterpriseGuid);
                //if (enterpriseEntity != null)
                //{
                //    enterpriseEntity.CompanyNameShort = signedFullName;
                //    logicStatusInfo.IsSuccessful = EnterpriseBLL.Instance.Update(enterpriseEntity);
                //}

                //LoanBasicEntity loanEntity = LoanBasicBLL.Instance.Get(Miscs.CurrentLoanSimpleInfo.LoanGuid);
                //if (loanEntity != null)
                //{
                //    loanEntity.LoanStatus = LoanStatuses.New;
                //    logicStatusInfo.IsSuccessful = LoanBasicBLL.Instance.Update(loanEntity);
                //}

                string loanString = string.Format("UPDATE [GeneralLoanBasic]   SET       [LoanStatus] = {0}  WHERE [LoanGuid] = '{1}'", (int)LoanStatuses.New, Miscs.CurrentLoanSimpleInfo.LoanGuid);
                LoanBasicDAL.HelperExInstance.ExecuteNonQuery(loanString);


                //SetCurrentLoanStep(90);
                //清空Loan的cookie
                LoanCookie loanCookie = LoadCurrentLoanCookie();
                loanCookie.Reset();

                //向管理员发送通知邮件
                try
                {
                    using (SmtpClient smtpClient = new SmtpClient())
                    {
                        MailMessage mailMessage = GetMail();
                        smtpClient.Send(mailMessage);
                    }
                }
                catch (Exception)
                {
                    //do nothing;
                }
                
            }
            else
            {
                logicStatusInfo.IsSuccessful = false;
                logicStatusInfo.Message = "please sign your name first,thanks.";
            }

            return Json(logicStatusInfo);
        }
        #endregion

        #region step10
        public ActionResult FinalStep()
        {
            SetCurrentLoanLayout();
            return View();
        }
        #endregion

        #region 辅助方法
        private static MailMessage GetMail()
        {
            MailMessage mailMessage = new MailMessage();
            MailAddress fromAddress = new MailAddress("develope@163.com", "gbcash");
            MailAddress toAddress = new MailAddress("notinmood@gmail.com", "gbcash apply");
            //MailAddress toAddress = new MailAddress("apply@gbcash.com.au", "gbcash apply");

            mailMessage.From = fromAddress;
            mailMessage.To.Add(toAddress);
            mailMessage.Body = "please to check...";
            mailMessage.Subject = DateTime.Now.ToString()+ " secured new apply";

            return mailMessage;
        }


        /// <summary>
        /// 设置当前执行贷款的步骤数
        /// </summary>
        /// <param name="step"></param>
        private void SetCurrentLoanStep(int step)
        {
            LoanTypes loanType = Miscs.GetCurrentLoanType();

            if (loanType == LoanTypes.Secured)
            {
                Miscs.SetLoanStep<SecuredLoanCookie>(step);
            }
            else
            {
                Miscs.SetLoanStep<UnSecuredLoanCookie>(step);
            }
        }

        /// <summary>
        /// 载入当前执行贷款的Cookie
        /// </summary>
        /// <returns></returns>
        private LoanCookie LoadCurrentLoanCookie()
        {
            LoanTypes loanType = Miscs.GetCurrentLoanType();
            LoanCookie loanCookie;

            if (loanType == LoanTypes.Secured)
            {
                loanCookie = CookieInfo.Load<SecuredLoanCookie>();
            }
            else
            {
                loanCookie = CookieInfo.Load<UnSecuredLoanCookie>();
            }

            return loanCookie;
        }

        /// <summary>
        /// 重置当前执行贷款的Cookie
        /// </summary>
        private void ResetCurrentLoanCookie()
        {
            LoanTypes loanType = Miscs.GetCurrentLoanType();

            if (loanType == LoanTypes.Secured)
            {
                Miscs.ResetLoanCookie<SecuredLoanCookie>();
            }
            else
            {
                Miscs.ResetLoanCookie<UnSecuredLoanCookie>();
            }
        }

        /// <summary>
        /// 设置当前执行贷款的模板页
        /// </summary>
        private void SetCurrentLoanLayout()
        {
            LoanTypes loanType = Miscs.GetCurrentLoanType();

            if (loanType == LoanTypes.Secured)
            {
                this.ViewData["_Layout"] = CodeUtils.LayoutPathForSecured;
            }
            else
            {
                this.ViewData["_Layout"] = CodeUtils.LayoutPathForUnSecured;
            }
        }

        private decimal GetCurrentLoanRate()
        {
            decimal rate = 0.48M;
            LoanTypes loanType = Miscs.GetCurrentLoanType();
            if (loanType == LoanTypes.Secured)
            {
                rate = LoanBasicSetting.SecuredLoansAnnualInterest;
            }
            else
            {
                rate = LoanBasicSetting.UnSecuredLoansAnnualInterest;
            }

            return rate;
        }


        /// <summary>
        /// 判断当前页面是现实状态还是编辑状态（供View使用）
        /// </summary>
        /// <returns></returns>
        public MvcControlUsingModes GetUsingMode()
        {
            MvcControlUsingModes usingMode = MvcControlUsingModes.Editable;
            if (this.ViewData["usingMode"] != null)
            {
                string usingModeString = this.ViewData["usingMode"].ToString();
                int usingModeInt = (int)usingMode;
                int.TryParse(usingModeString, out usingModeInt);
                usingMode = (MvcControlUsingModes)usingModeInt;
            }

            return usingMode;
        }

        /// <summary>
        /// 判断当前页面是内嵌状态还是弹出状态（供View使用）
        /// </summary>
        /// <returns></returns>
        public bool CheckIsPopLayer()
        {
            bool isPopLayer = false;
            if (this.ViewData["IsPopLayer"] != null)
            {
                string isPopLayerString = this.ViewData["IsPopLayer"].ToString();
                bool.TryParse(isPopLayerString, out isPopLayer);
            }
            return isPopLayer;
        }

        /// <summary>
        /// 在Controller中接受参数并传递到View中
        /// </summary>
        public void BrokeLoanParam()
        {
            this.BrokeParam("isPopLayer");
            this.BrokeParam("usingMode");
        }

        /// <summary>
        /// 必填字段的验证
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private JsonResult FailureValidate(string fieldName, string additionalMessage = "")
        {
            LogicStatusInfo statusInfo = new LogicStatusInfo();
            statusInfo.IsSuccessful = false;
            statusInfo.Message = string.Format("Field '{0}' is required.", fieldName);
            if (string.IsNullOrWhiteSpace(additionalMessage) == false)
            {
                statusInfo.Message += "And " + additionalMessage;
            }

            return Json(statusInfo);
        }
        #endregion

        #region 测试方法
        public ActionResult Test()
        {
            return View();
        }

        #endregion
    }
}
