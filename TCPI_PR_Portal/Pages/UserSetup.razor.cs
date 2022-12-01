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
using Blazor.SubtleCrypto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using TCPI_PR_Portal.Data;
using TCPI_PR_Portal.Services;
using TCPI_PR_Portal.Shared;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace TCPI_PR_Portal.Pages
{
    public partial class UserSetup
    {
        bool success = false;
        string displayApprover = "none";
        string displayLevel = "none";
        UserDto User = new UserDto();
        //DepartmentsDto SelectedDepartment = new DepartmentsDto();
        //DepartmentsResponse? DeptResponse = new DepartmentsResponse();
        //List<DepartmentsDto>? Departments = new List<DepartmentsDto>();
        List<SAPUserDto>? SapUser = new List<SAPUserDto>();
        List<PortalUserDto>? PortalUser = new List<PortalUserDto>();
        List<string> userCodes = new List<string>();
        List<string> approver1Codes = new List<string>();
        List<string> approver2Codes = new List<string>();
        List<string> approver3Codes = new List<string>();
        List<string> approver4Codes = new List<string>();
        List<string> approverSpecCodes = new List<string>();
        private List<BreadcrumbItem> _items = new List<BreadcrumbItem>{new BreadcrumbItem("User Setup", href: "user-setup", disabled: true)};
        private async Task OnValidSubmit(EditContext context)
        {
            try
            {
                //string password = User.U_Password;
                //CryptoResult encryptedPassword = await Crypto.EncryptAsync(password);
                //User.U_Password = encryptedPassword.Value;
                string lastCode = "PR0000000000";
                JObject codeJson = await GetData("U_FT_WPUS?$select=Code&$orderby=Code desc&$top=1");
                CodeDto code = codeJson["value"][0].ToObject<CodeDto>();
                string nextCode = IncrementCode(code.Code);
                User.Code = nextCode;
                User.Name = nextCode;
                HttpContent content = new StringContent(JsonConvert.SerializeObject(User), Encoding.UTF8, "application/json");
                using var response = await HttpClient.PostAsync("U_FT_WPUS", content);
                if (!response.IsSuccessStatusCode)
                {
                    Snackbar.Add(response.ReasonPhrase, Severity.Error);
                    Console.WriteLine(response.ReasonPhrase);
                    return;
                }

                Snackbar.Add("A new User was successfully created!", Severity.Success);
                User = new UserDto();
                success = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
                throw;
            }
        }

        string IncrementCode(string code)
        {
            return Regex.Replace(code, "\\d+", m => (int.Parse(m.Value) + 1).ToString(new string ('0', m.Value.Length)));
        }

        protected override async void OnInitialized()
        {
            //Uncomment this to get the list of departments
            //var response = await HttpClient.GetAsync("Departments?$select=Code,Name");
            //DeptResponse = await response.Content.ReadFromJsonAsync<DepartmentsResponse>();
            //Departments = DeptResponse.value;
            await GetUsers();
            await GetApprovers();
            foreach (var item in PortalUser)
            {
                if (item.U_Role == "Approver" && item.U_ApproverLevel == "Level 1")
                    approver1Codes.Add(item.U_UserCode);
                if (item.U_Role == "Approver" && item.U_ApproverLevel == "Level 2")
                    approver2Codes.Add(item.U_UserCode);
                if (item.U_Role == "Approver" && item.U_ApproverLevel == "Level 3")
                    approver3Codes.Add(item.U_UserCode);
                if (item.U_Role == "Approver" && item.U_ApproverLevel == "Level 4")
                    approver4Codes.Add(item.U_UserCode);
                if (item.U_Role == "Approver" && item.U_ApproverLevel == "Special")
                    approverSpecCodes.Add(item.U_UserCode);
            }
        }

        void SelectRole(UserDto context, string selectedString)
        {
            context.U_Role = selectedString;
            displayApprover = selectedString == "Requestor" ? "block" : "none";
            displayLevel = selectedString == "Approver" ? "block" : "none";
        }

        // REFACTOR THIS REPETITIVE CODE LATER!!!!!
        // TEMP CODE TO MAKE THE FUNCTIONALITY WORKING.
        private void OnValueChanged(string value)
        {
            User.U_UserCode = value;
            SAPUserDto result = SapUser.Find(e => e.UserCode == User.U_UserCode);
            User.U_UserName = result.UserName;
            User.U_EmailAddress = result.eMail;
        }

        private void OnValueChanged1(string value)
        {
            User.U_ApproverCode1 = value;
            User.U_Approver1 = PortalUser.Where(e => e.U_UserCode == value).Select(e => e.U_UserName).First();
        }

        private void OnValueChanged2(string value)
        {
            User.U_ApproverCode2 = value;
            User.U_Approver2 = PortalUser.Where(e => e.U_UserCode == value).Select(e => e.U_UserName).First();
        }

        private void OnValueChanged3(string value)
        {
            User.U_ApproverCode3 = value;
            User.U_Approver3 = PortalUser.Where(e => e.U_UserCode == value).Select(e => e.U_UserName).First();
        }

        private void OnValueChanged4(string value)
        {
            User.U_ApproverCode4 = value;
            User.U_Approver4 = PortalUser.Where(e => e.U_UserCode == value).Select(e => e.U_UserName).First();
        }

        private void OnValueChangedSpec(string value)
        {
            User.U_ApproverSpecialCode = value;
            User.U_ApproverSpecial = PortalUser.Where(e => e.U_UserCode == value).Select(e => e.U_UserName).First();
        }

        private async Task<IEnumerable<string>> SearchUserCode(string value)
        {
            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return userCodes;
            return userCodes.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchApprover1(string value)
        {
            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return approver1Codes;
            return approver1Codes.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchApprover2(string value)
        {
            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return approver2Codes;
            return approver2Codes.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchApprover3(string value)
        {
            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return approver3Codes;
            return approver3Codes.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchApprover4(string value)
        {
            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return approver4Codes;
            return approver4Codes.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchApproverSpecial(string value)
        {
            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return approverSpecCodes;
            return approverSpecCodes.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        async Task<JObject> GetData(string query)
        {
            using var response = await HttpClient.GetAsync(query);
            if (!response.IsSuccessStatusCode)
            {
                Snackbar.Add("Session has expired", Severity.Error);
                AuthService.Logout();
                Navigation.NavigateTo("/");
            }

            string content = await response.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }

        async Task GetUsers()
        {
            string query = "Users?$select=UserCode,UserName,eMail,Branch,Department&$orderby=InternalKey desc";
            JObject json;
            do
            {
                json = await GetData(query);
                SapUser.AddRange(json["value"].ToObject<List<SAPUserDto>>());
                if (json.ContainsKey("odata.nextLink"))
                    query = json["odata.nextLink"].ToString();
            }
            while (json.ContainsKey("odata.nextLink"));
            foreach (var item in SapUser)
            {
                userCodes.Add(item.UserCode);
            }
        }

        async Task GetApprovers()
        {
            string query = "U_FT_WPUS?$select=U_UserCode,U_UserName,U_Role,U_ApproverLevel,U_Employee,U_EmailAddress&$orderby=Code desc";
            JObject json;
            do
            {
                json = await GetData(query);
                PortalUser.AddRange(json["value"].ToObject<List<PortalUserDto>>());
                if (json.ContainsKey("odata.nextLink"))
                    query = json["odata.nextLink"].ToString();
            }
            while (json.ContainsKey("odata.nextLink"));
        }
    }
}