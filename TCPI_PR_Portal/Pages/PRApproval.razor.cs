using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using MudBlazor;
using TCPI_PR_Portal;
using TCPI_PR_Portal.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TCPI_PR_Portal.Data;
using TCPI_PR_Portal.Services;
using TCPI_PR_Portal.Shared;
using System.Text;

namespace TCPI_PR_Portal.Pages
{
    public partial class PRApproval
    {
        [Parameter]
        public string DocEntry { get; set; }

        public int HeaderDocEntry;
        public int index = 0;
        public bool isApprover = false;
        string approvalLevel = string.Empty;
        // table settings
        private bool dense = false;
        private bool hover = true;
        private bool striped = true;
        private bool bordered = false;
        private PRHeaderDto PRHeader = new PRHeaderDto();
        private List<PRLinesDto> PRLines = new List<PRLinesDto>();
        private List<BreadcrumbItem> _items = new List<BreadcrumbItem>{new BreadcrumbItem("Document Status", href: "document-status", disabled: true), new BreadcrumbItem("Requisition Slip Approval", href: "approval")};
        // form validation
        bool success = false;
        private async Task Approve(EditContext context)
        {
            var approverDetails = FilterApprover();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(approverDetails), Encoding.UTF8, "application/json");
            using var response = await HttpClient.PatchAsync($"U_FT_OPRQ('{PRHeader.Code}')", content);
            if (!response.IsSuccessStatusCode)
            {
                Snackbar.Add("Sorry, Approval was not successful!", Severity.Error);
                return;
            }

            Snackbar.Add("The Document was successfully approved!", Severity.Success);
            success = true;
            StateHasChanged();
            Navigation.NavigateTo("document-status");
        }

        private async Task Reject()
        {
            var rejectContent = new
            {
            U_DocStatus = "Rejected"
            }

            ;
            HttpContent content = new StringContent(JsonConvert.SerializeObject(rejectContent), Encoding.UTF8, "application/json");
            using var response = await HttpClient.PatchAsync($"U_FT_OPRQ('{PRHeader.Code}')", content);
            if (!response.IsSuccessStatusCode)
            {
                Snackbar.Add("Sorry, rejection was not successful!", Severity.Error);
                return;
            }

            Snackbar.Add("The Document was rejected!", Severity.Success);
            Navigation.NavigateTo("document-status");
        }

        // TODO: Populate the lines and header
        protected override async Task OnInitializedAsync()
        {
            approvalLevel = LocalStorage.GetItem<string>("ApproverLevel");
            JObject headerResponse = await GetData($"U_FT_OPRQ?$filter=U_DocEntry eq {DocEntry}");
            List<PRHeaderDto> header = headerResponse["value"].ToObject<List<PRHeaderDto>>();
            PRHeader = header[0];
            string query = $"U_FT_PRQ1?$filter=U_DocEntry eq '{DocEntry}'";
            JObject lines;
            do
            {
                lines = await GetData(query);
                PRLines.AddRange(lines["value"].ToObject<List<PRLinesDto>>());
                if (lines.ContainsKey("odata.nextLink"))
                    query = lines["odata.nextLink"].ToString();
            }
            while (lines.ContainsKey("odata.nextLink"));
        }

        async Task<JObject> GetData(string query)
        {
            using var response = await HttpClient.GetAsync(query);
            if (!response.IsSuccessStatusCode)
            {
                Snackbar.Add(response.ReasonPhrase, Severity.Error);
                AuthService.Logout();
                Navigation.NavigateTo("/");
            }

            string content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }

        object FilterApprover()
        {
            string filterField = string.Empty;
            DateTime approveDate = DateTime.Now;
            var remarks = PRHeader.U_ApproverRemarks;
            var approver = LocalStorage.GetItem<string>("UserCode");
            var approverDetails = new object ();
            if (PRHeader.U_PRType != "Regular" && approvalLevel == "Level 1")
            {
                approverDetails = new
                {
                U_ApprovedBy1 = approver, U_ApprovedDate = approveDate, U_ApproverRemarks = remarks, U_DocStatus = "A1-NotCompleted"
                }

                ;
            }

            if (PRHeader.U_PRType != "Regular" && approvalLevel == "Special")
            {
                approverDetails = new
                {
                U_ApprovedBy1 = approver, U_ApprovedDate = approveDate, U_ApproverRemarks = remarks, U_DocStatus = "A1"
                }

                ;
            }

            if (PRHeader.U_PRType == "Regular" && approvalLevel == "Level 1")
            {
                approverDetails = new
                {
                U_ApprovedBy1 = approver, U_ApprovedDate = approveDate, U_ApproverRemarks = remarks, U_DocStatus = "A1"
                }

                ;
            }

            if (approvalLevel == "Level 2")
            {
                approverDetails = new
                {
                U_ApprovedBy2 = approver, U_ApprovedDate = approveDate, U_ApproverRemarks = remarks, U_DocStatus = "A2"
                }

                ;
            }

            if (approvalLevel == "Level 3")
            {
                approverDetails = new
                {
                U_ApprovedBy3 = approver, U_ApprovedDate = approveDate, U_ApproverRemarks = remarks, U_DocStatus = "A3"
                }

                ;
            }

            if (approvalLevel == "Level 4")
            {
                approverDetails = new
                {
                U_ApprovedBy4 = approver, U_ApprovedDate = approveDate, U_ApproverRemarks = remarks, U_DocStatus = "A4"
                }

                ;
            }

            return approverDetails;
        }
    }
}