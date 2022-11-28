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
        public string WhsCode { get; set; }
        public string WhsName { get; set; }

        public override bool Equals(object o)
        {
            var other = o as WarehouseDto;
            return other?.WhsCode == WhsCode;
        }
        public override int GetHashCode() => WhsCode.GetHashCode();
        public override string ToString()
        {
            return WhsName;
        }
    }

    public class WarehouseResponse
    {
        public string odata { get; set; } = string.Empty;
        public List<WarehouseDto> value { get; set; } = new List<WarehouseDto>();
    }
}