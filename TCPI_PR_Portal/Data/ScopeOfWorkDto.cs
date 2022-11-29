using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPI_PR_Portal.Data;

namespace TCPI_PR_Portal.Models
{
    public class ScopeOfWorkDto
    {
        public string FactorCode { get; set; }
        public string FactorDescription { get; set; }

        public override bool Equals(object o)
        {
            var other = o as ScopeOfWorkDto;
            return other?.FactorCode == FactorCode;
        }
        public override int GetHashCode() => FactorCode.GetHashCode();
        public override string ToString()
        {
            return FactorCode;
        }
    }

    public class ScopeOfWorkResponse
    {
        public string odata { get; set; } = string.Empty;
        public List<ScopeOfWorkDto> value { get; set; } = new List<ScopeOfWorkDto>();
		public Error error { get; set; } = new Error();
	}
}
