using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPI_PR_Portal.Data;

namespace TCPI_PR_Portal.Models
{
    public class WarehouseDto
    {
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }

        public override bool Equals(object o)
        {
            var other = o as WarehouseDto;
            return other?.WarehouseCode == WarehouseCode;
        }
        public override int GetHashCode() => WarehouseCode.GetHashCode();
        public override string ToString()
        {
            return WarehouseName;
        }
    }

    public class WarehouseResponse
    {
        public string odata { get; set; } = string.Empty;
        public List<WarehouseDto> value { get; set; } = new List<WarehouseDto>();
		public Error error { get; set; } = new Error();
	}
}