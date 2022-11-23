using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPI_PR_Portal.Models
{
    public class PRHeaderDto
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
        public int? DocEntry { get; set; }
        public string? U_ProjectID { get; set; } // revert back to int after testing
        public string? U_ProjName { get; set; }
        public string? U_Location { get; set; }
        public string? U_PRType { get; set; } // no binded field
        public string? U_DocNum { get; set; }
        public string? U_Department { get; set; } // Preferred Supplier to confirm if this is the field for it
        public string? U_Branch { get; set; } // no binded field // list
        public string? U_DocStatus { get; set; }
        public DateTime? U_TaxDate { get; set; }
        public DateTime? U_ReqDate { get; set; }
        public string? U_Urgency { get; set; }
        public string? U_PreparedBy { get; set; }
        public string? U_ReviewedBy { get; set; }
        public string? U_ApprovedBy1 { get; set; }
        public string? U_ApprovedBy2 { get; set; }
        public string? U_ApprovedBy3 { get; set; }
        public string? U_ApprovedBy4 { get; set; }
        public string? U_ApprovedDate { get; set; }
        public string? U_Remarks { get; set; }
        public string? U_ApproverRemarks { get; set; }

        public virtual PRLinesDto? PRLines { get; set; }
    }
}
