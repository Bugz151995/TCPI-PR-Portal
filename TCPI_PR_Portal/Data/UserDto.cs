using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPI_PR_Portal.Data;

namespace TCPI_PR_Portal.Models
{
    public class UserDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, Display(Name="Username")]
        public string U_UserName { get; set; } = string.Empty;
        [Required, Display(Name = "Password")]
        public string U_Password { get; set; } = string.Empty;
        [Required, Display(Name = "Employee Name")]
        public string U_Employee { get; set; } = string.Empty;
        [AllowNull ,EmailAddress, Display(Name = "Email Address")]
        public string U_EmailAddress { get; set; } = string.Empty;
        [Required, Display(Name = "Role")]
        public string U_Role { get; set; } = string.Empty;
        public string? U_Approver1 { get; set; }
        public string? U_Approver2 { get; set; }
        public string? U_Approver3 { get; set; }
        public string? U_Approver4 { get; set; }
        public string? U_ApproverSpecial { get; set; }
        public string? U_ApproverCode1 { get; set; }
        public string? U_ApproverCode2 { get; set; }
        public string? U_ApproverCode3 { get; set; }
        public string? U_ApproverCode4 { get; set; }
        public string? U_ApproverSpecialCode { get; set; }
        public string? U_ApprovalLevel { get; set; }

        // Uncomment if needed this is included in the mapping not in the UI
        //public int U_UserId { get; set; }
        //public int U_Department { get; set; }
        //public int U_CostCenter { get; set; }
    }

    public class UserResponse
    {
        public string odatametadata { get; set; } = string.Empty;
        public List<SAPUserDto> value { get; set; } = new List<SAPUserDto>();
		public Error error { get; set; } = new Error();
		//public string odatanextLink { get; set; } = string.Empty;
    }

    public class SAPUserDto
    {
        public string UserCode { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string eMail { get; set; } = string.Empty;
        public int Branch { get; set; } = new int();
        public int Department { get; set; } = new int();
    }
}
