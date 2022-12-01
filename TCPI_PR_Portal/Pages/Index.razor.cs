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
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Security.Claims;
using TCPI_PR_Portal.Data;
using TCPI_PR_Portal.Shared;
using System.Text;
using System.Security.Cryptography;
using Blazor.SubtleCrypto;
using TCPI_PR_Portal.Services;

namespace TCPI_PR_Portal.Pages
{
    public partial class Index
    {
        bool success;
        LoginDto User = new LoginDto();
        LoginSessionDto LoginSession = new LoginSessionDto();
        LoginResponse? LoginResponse = new LoginResponse();
        UserSessionDto UserSession = new UserSessionDto();
        SAPUserDto SapUser = new SAPUserDto();
        UserResponse? SapUserResponse = new UserResponse();
        private async Task OnValidSubmit(EditContext context)
        {
            try
            {
                var credential = new
                {
                CompanyDB = "TCPI_TESTDB", UserName = User.U_UserName, Password = User.U_Password
                }

                ;
                HttpContent content = new StringContent(JsonConvert.SerializeObject(credential), Encoding.UTF8, "application/json");
                using var sapLogin = await HttpClient.PostAsync("Login", content);
                if (!sapLogin.IsSuccessStatusCode)
                {
                    Snackbar.Add(sapLogin.ReasonPhrase, Severity.Error);
                    return;
                }

                LoginSession = await sapLogin.Content.ReadFromJsonAsync<LoginSessionDto>();
                using var userUdtResponse = await HttpClient.GetAsync($"U_FT_WPUS?$select=Code,U_Employee,U_ApproverLevel,U_Role,U_ApproverCode1,U_ApproverCode2,U_ApproverCode3,U_ApproverCode4,U_ApproverSpecialCode,U_Approver1,U_Approver2,U_Approver3,U_Approver4,U_ApproverSpecial&$filter=U_UserCode eq '{User.U_UserName}'");
                using var userResponse = await HttpClient.GetAsync($"Users?$select=UserCode,UserName,eMail,Branch,Department&$filter=UserCode eq '{User.U_UserName}'");

                if (!userUdtResponse.IsSuccessStatusCode && !userResponse.IsSuccessStatusCode)
                {
                    Snackbar.Add("Oops! It seems like we have hit a snag!", Severity.Error);
                    return;
                }

                LoginResponse = await userUdtResponse.Content.ReadFromJsonAsync<LoginResponse>();
                UserSession = LoginResponse.value[0];
                SapUserResponse = await userResponse.Content.ReadFromJsonAsync<UserResponse>();
                SapUser = SapUserResponse.value[0];
                Console.WriteLine(JsonConvert.SerializeObject(SapUser));
                CreateLocalStorageSession();
                AuthService.Login(UserSession.U_Employee, UserSession.U_Role);
                Navigation.NavigateTo("user-setup");
                success = true;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Snackbar.Add("Oops! It seems like we have hit a snag!", Severity.Error);
                throw;
            }
        }

        private void CreateLocalStorageSession()
        {
            LocalStorage.SetItem("SessionId", LoginSession.SessionId);
            LocalStorage.SetItem("Version", LoginSession.Version);
            LocalStorage.SetItem("SessionTimeout", LoginSession.SessionTimeout);
            LocalStorage.SetItem("Code", UserSession.Code);
            LocalStorage.SetItem("UserName", SapUser.UserName);
            LocalStorage.SetItem("UserCode", SapUser.UserCode);
            LocalStorage.SetItem("EmployeeName", UserSession.U_Employee);
            LocalStorage.SetItem("Role", UserSession.U_Role);
            LocalStorage.SetItem("ApproverLevel", UserSession.U_ApproverLevel);
            LocalStorage.SetItem("Branch", SapUser.Branch.ToString());
            LocalStorage.SetItem("Department", SapUser.Department.ToString());
            LocalStorage.SetItem("U_ApproverCode1", UserSession.U_ApproverCode1);
            LocalStorage.SetItem("U_ApproverCode2", UserSession.U_ApproverCode2);
            LocalStorage.SetItem("U_ApproverCode3", UserSession.U_ApproverCode3);
            LocalStorage.SetItem("U_ApproverCode4", UserSession.U_ApproverCode4);
            LocalStorage.SetItem("U_ApproverSpecialCode", UserSession.U_ApproverSpecialCode);
            LocalStorage.SetItem("U_Approver1", UserSession.U_Approver1);
            LocalStorage.SetItem("U_Approver2", UserSession.U_Approver2);
            LocalStorage.SetItem("U_Approver3", UserSession.U_Approver3);
            LocalStorage.SetItem("U_Approver4", UserSession.U_Approver4);
            LocalStorage.SetItem("U_ApproverSpecial", UserSession.U_ApproverSpecial);
        }
    }
}