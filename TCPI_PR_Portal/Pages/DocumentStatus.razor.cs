using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
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
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using TCPI_PR_Portal.Data;
using TCPI_PR_Portal.Services;
using TCPI_PR_Portal.Shared;
using Newtonsoft.Json;

namespace TCPI_PR_Portal.Pages
{
    public partial class DocumentStatus
    {
        // table settings
        private bool dense = false;
        private bool hover = true;
        private bool striped = false;
        private bool bordered = false;
        private List<BreadcrumbItem> _items = new List<BreadcrumbItem>{new BreadcrumbItem("Document Status", href: "document-status", disabled: true)};
        private string searchString = "";
        private PRHeaderDto selectedItem1 = null;
        private List<PRLinesDto> _lines = new List<PRLinesDto>();
        //test data
        private List<PRHeaderDto> PRRequests = new List<PRHeaderDto>();
        PRHeaderResponse? PRHeaderResponse = new PRHeaderResponse();
        protected override async Task OnInitializedAsync()
        {
            string role = LocalStorage.GetItem<string>("Role");
            if (role == "Approver")
            {
                await GetMyRequestors();
            }

            if (role == "Requestor")
            {
                await GetMyRequests();
            }
        }

        private bool FilterFunc1(PRHeaderDto element) => FilterFunc(element, searchString);
        private bool FilterFunc(PRHeaderDto element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.U_ProjectID.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.U_ProjName.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        void ViewDocument(int? docEntry)
        {
            Navigation.NavigateTo($"document-status/{docEntry}");
        }

        async Task GetMyRequests()
        {
            var userCode = LocalStorage.GetItem<string>("UserCode");
            using var response = await HttpClient.GetAsync($"U_FT_OPRQ?$filter=U_CardCode eq '{userCode}'");
            if (!response.IsSuccessStatusCode)
            {
                Snackbar.Add(response.ReasonPhrase, Severity.Error);
            }

            PRHeaderResponse = await response.Content.ReadFromJsonAsync<PRHeaderResponse>();
            PRRequests = PRHeaderResponse.value;
        }

        async Task GetMyRequestors()
        {
            List<UserCodeDto> userCodes = new List<UserCodeDto>();
            string approvalLevel = LocalStorage.GetItem<string>("ApproverLevel");
            string userCode = LocalStorage.GetItem<string>("UserCode");
            Console.WriteLine(approvalLevel);
            string filterField = string.Empty;
            if (approvalLevel == "Level 1")
                filterField = "U_ApproverCode1";
            if (approvalLevel == "Level 2")
                filterField = "U_ApproverCode2";
            if (approvalLevel == "Level 3")
                filterField = "U_ApproverCode3";
            if (approvalLevel == "Level 4")
                filterField = "U_ApproverCode4";
            if (approvalLevel == "Special")
                filterField = "U_ApproverSpecialCode";
            string query = $"U_FT_WPUS?$select=U_UserCode&$filter={filterField} eq '{userCode}'";
            JObject user;
            do
            {
                user = await GetData(query);
                userCodes.AddRange(user["value"].ToObject<List<UserCodeDto>>());
                if (user.ContainsKey("odata.nextLink"))
                    query = user["odata.nextLink"].ToString();
            }
            while (user.ContainsKey("odata.nextLink"));
            Console.WriteLine(JsonConvert.SerializeObject(userCodes));
            var filteredRequest = FilterRequests();
            foreach (var code in userCodes)
            {
                string requestsQuery = $"U_FT_OPRQ?$filter=U_CardCode eq '{code.U_UserCode}' and U_DocStatus eq '{filteredRequest}'";
                JObject requests;
                do
                {
                    requests = await GetData(requestsQuery);
                    PRRequests.AddRange(requests["value"].ToObject<List<PRHeaderDto>>());
                    if (requests.ContainsKey("odata.nextLink"))
                        requestsQuery = requests["odata.nextLink"].ToString();
                }
                while (requests.ContainsKey("odata.nextLink"));
            }
        }

        async Task<JObject> GetData(string query)
        {
            using var response = await HttpClient.GetAsync(query);
            if (!response.IsSuccessStatusCode)
            {
                Snackbar.Add(response.ReasonPhrase, Severity.Error);
            }

            string content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }

        string FilterRequests()
        {
            string approvalLevel = LocalStorage.GetItem<string>("ApproverLevel");
            string status = string.Empty;
            if (approvalLevel == "Level 1")
                status = "Waiting For Approval";
            if (approvalLevel == "Special")
                status = "A1-NotCompleted";
            if (approvalLevel == "Level 2")
                status = "A1";
            if (approvalLevel == "Level 3")
                status = "A2";
            if (approvalLevel == "Level 4")
                status = "A3";
            return status;
        }
    }
}