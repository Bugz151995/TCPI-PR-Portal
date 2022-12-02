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
        public string U_ItemCode { get; set; }
        public string U_ItemName { get; set; }
    }

    public class ItemsResponse
    {
        public string odata { get; set; } = string.Empty;
        public List<ItemsDto> value { get; set; } = new List<ItemsDto>();
		public Error error { get; set; } = new Error();
	}
}