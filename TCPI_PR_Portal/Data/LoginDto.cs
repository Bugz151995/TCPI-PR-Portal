using System.ComponentModel.DataAnnotations;
using TCPI_PR_Portal.Models;

namespace TCPI_PR_Portal.Data
{
    public class LoginDto
    {
        [Required, Display(Name = "Username")]
        public string U_UserName { get; set; } = string.Empty;
        [Required, Display(Name = "Password")]
        public string U_Password { get; set; } = string.Empty;
    }

    public class UserSessionDto
    {
        public string Code { get; set; }
        public string U_Employee { get; set; }
        public string U_Role { get; set; }
        public string U_Password { get; set; }
        public string? U_ApproverLevel { get; set; }
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
    }

    public class LoginSessionDto
    {
        public string odata { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public int SessionTimeout { get; set; }
        public Error error { get; set; } = new Error();
    }

    public class LoginResponse
    {
        public string odata { get; set; } = string.Empty;
        public List<UserSessionDto> value { get; set; } = new List<UserSessionDto>();
    }
}
