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

    public class LoginResponse
    {
        public string odata { get; set; } = string.Empty;
        public List<LoginDto> value { get; set; } = new List<LoginDto>();
    }
}
