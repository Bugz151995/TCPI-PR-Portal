using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPI_PR_Portal.Models
{
    public class User
    {
        public string Code { get; set; }
        public string Name { get; set; }
        
        [Required]
        public string U_UserName { get; set; }
        [Required]
        public string U_Password { get; set; }
        public int U_UserId { get; set; }
        public string U_Employee { get; set; }
        public string U_EmailAddress { get; set; }
        public string U_Role { get; set; }
        public string U_ApprovalLevel { get; set; }
        public int U_Department { get; set; }
        public int U_CostCenter { get; set; }
        public int U_Approver1 { get; set; }
        public int U_Approver2 { get; set; }
        public int U_Approver3 { get; set; }
        public int U_Approver4 { get; set; }
        public int U_ApproverCode1 { get; set; }
        public int U_ApproverCode2 { get; set; }
        public int U_ApproverCode3 { get; set; }
        public int U_ApproverCode4 { get; set; }
    }
}
