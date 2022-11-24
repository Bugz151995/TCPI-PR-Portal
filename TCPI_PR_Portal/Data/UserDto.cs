using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPI_PR_Portal.Models
{
    public class UserDto
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string U_UserName { get; set; }
        [Required]
        public string U_Password { get; set; }
        public string U_Employee { get; set; }
        [EmailAddress, Required]
        public string U_EmailAddress { get; set; }
        [Required]
        public string U_Role { get; set; }

        public string? U_Approver1 { get; set; }
        public string? U_Approver2 { get; set; }
        public string? U_Approver3 { get; set; }
        public string? U_Approver4 { get; set; }
        public string? U_ApproverSpecial { get; set; }
        public int? U_ApproverCode1 { get; set; }
        public int? U_ApproverCode2 { get; set; }
        public int? U_ApproverCode3 { get; set; }
        public int? U_ApproverCode4 { get; set; }
        public int? U_ApproverCodeSpecial { get; set; }

        // Uncomment if needed this is included in the mapping not in the UI
        
        //public int U_UserId { get; set; }
        public string U_ApprovalLevel { get; set; }
        public int U_Department { get; set; }
        public int U_CostCenter { get; set; }
    }
}
