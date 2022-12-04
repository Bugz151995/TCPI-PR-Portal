using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TCPI_PR_Portal.Data
{
    public class ContactDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}
