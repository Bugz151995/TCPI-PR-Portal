using TCPI_PR_Portal.Models;

namespace TCPI_PR_Portal.Data
{
    public class DocEntryDto
    {
        public int U_DocEntry { get; set; }
    }

    public class DocEntryResponse
    {
        public string odata { get; set; }
        public List<DocEntryDto> value { get; set; } = new List<DocEntryDto>();
    }

    public class CodeDto
    {
        public string Code { get; set; }
    }

    public class CodeResponse
    {
        public string odata { get; set; }
        public List<CodeDto> value { get; set; } = new List<CodeDto>();
    }
}
