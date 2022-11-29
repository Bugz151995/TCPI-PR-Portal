using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPI_PR_Portal.Data;

namespace TCPI_PR_Portal.Models
{
    public class BranchesDto
    {
        public int Code { get; set; }
        public string Name { get; set; }

        public override bool Equals(object o)
        {
            var other = o as BranchesDto;
            return other?.Code == Code;
        }
        public override int GetHashCode() => Code.GetHashCode();
        public override string ToString()
        {
            return Name;
        }
    }

    public class BranchesResponse
    {
        public string odata { get; set; } = string.Empty;
        public List<BranchesDto> value { get; set; } = new List<BranchesDto>();
		public Error error { get; set; } = new Error();
	}
}
