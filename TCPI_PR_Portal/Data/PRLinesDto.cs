using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TCPI_PR_Portal.Models
{
    public class PRLinesDto
    {
        [Key, Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
        public int? U_DocEntry { get; set; }
        public string? U_Dscription { get; set; } = string.Empty;
        public string? U_WhsCode { get; set; } = string.Empty;
        public string? U_ItemCode { get; set; } = string.Empty;
        public string? U_BinLoc { get; set; } = string.Empty;
        public string? U_Scope { get; set; } = string.Empty;
        public string? U_ScopeDesc { get; set; } = string.Empty;
        public string? U_MaterialCode { get; set; } = string.Empty;
        public string? U_MaterialDesc { get; set; } = string.Empty;
        public string? U_Quantity { get; set; } = string.Empty;
        public string? U_InfoPrice { get; set; } = string.Empty;
        public string? U_UomCode { get; set; } = string.Empty;
        public string? U_ItemSpecification { get; set; } = string.Empty;
        public string? U_TaxCode { get; set; } = string.Empty;
        public string? U_OnHandQuantity { get; set; } = string.Empty;
        public string? U_InventoryType { get; set; } = string.Empty;
        public string? U_InventoryPurpose { get; set; } = string.Empty;
    }
}
