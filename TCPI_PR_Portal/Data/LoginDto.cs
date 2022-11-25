using System.ComponentModel.DataAnnotations;

namespace TCPI_PR_Portal.Data
{
    public class LoginDto
    {
        [Required]
        public string U_UserName { get; set; } = string.Empty;
        [Required]
        public string U_Password { get; set; } = string.Empty;
    }

    public class SessionDto
    {
        public string Code { get; set; }
        public string U_Employee { get; set; }
        public string U_Role { get; set; }

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

    public class LoginResponse
    {
        public string odata { get; set; } = string.Empty;
        public List<SessionDto> value { get; set; } = new List<SessionDto>();
    }
}
