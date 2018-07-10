using System;
using HiLand.General.Entity;

namespace Web.Models
{
    public class LoanBasicExtEntity : LoanBasicEntity
    {
        public int UserIndexID { get; set; }
        public string UserFirstName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserLastName { get; set; }
        public DateTime UserBirthday { get; set; }
        public string UserEmail { get; set; }
        public DateTime UserRegisterDate { get; set; }
        public string UserNote { get; set; }
        public string UserFlowUpHistory { get; set; }
    }
}
