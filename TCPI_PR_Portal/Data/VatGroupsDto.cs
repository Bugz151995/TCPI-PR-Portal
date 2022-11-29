using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPI_PR_Portal.Data;

namespace TCPI_PR_Portal.Models
{
    public class VatGroupsDto
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public override bool Equals(object o)
        {
            var other = o as VatGroupsDto;
            return other?.Code == Code;
        }
        public override int GetHashCode() => Code.GetHashCode();
        public override string ToString()
        {
            return Name;
        }
    }

    public class VatGroupsResponse
    {
        public string odata { get; set; } = string.Empty;
        public List<VatGroupsDto> value { get; set; } = new List<VatGroupsDto>();
		public Error error { get; set; } = new Error();
		public string odatanextLink { get; set; } = string.Empty;
	}
}