using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPI_PR_Portal.Data;

namespace TCPI_PR_Portal.Models
{
    public class DepartmentsDto
    {
        public int Code { get; set; }
        public string Name { get; set; }

        public override bool Equals(object o)
        {
            var other = o as DepartmentsDto;
            return other?.Code == Code;
        }
        public override int GetHashCode() => Code.GetHashCode();
        public override string ToString()
        {
            return Name;
        }
    }

    public class DepartmentsResponse
    {
        public string odata { get; set; } = string.Empty;
        public List<DepartmentsDto> value { get; set; } = new List<DepartmentsDto>();
		public Error error { get; set; } = new Error();
	}
}
