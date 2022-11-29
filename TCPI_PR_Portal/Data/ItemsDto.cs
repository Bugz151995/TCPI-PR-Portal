using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPI_PR_Portal.Data;

namespace TCPI_PR_Portal.Models
{
    public class ItemsDto
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        public override bool Equals(object o)
        {
            var other = o as ItemsDto;
            return other?.ItemCode == ItemCode;
        }
        public override int GetHashCode() => ItemCode.GetHashCode();
        public override string ToString()
        {
            return ItemName;
        }
    }

    public class ItemsResponse
    {
        public string odata { get; set; } = string.Empty;
        public List<ItemsDto> value { get; set; } = new List<ItemsDto>();
		public Error error { get; set; } = new Error();
	}
}