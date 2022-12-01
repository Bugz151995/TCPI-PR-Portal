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
using System.Text.RegularExpressions;
using TCPI_PR_Portal.Data;
using TCPI_PR_Portal.Services;
using TCPI_PR_Portal.Shared;
using System.Text;

namespace TCPI_PR_Portal.Pages
{
    public partial class RequisitionSlip
    {
        private bool success = false;
        private int docEntry = 0;
        private int docNum = 0;
        // table settings
        private bool dense = false;
        private bool hover = true;
        private bool striped = true;
        private bool bordered = false;
        private PRHeaderDto PRHeader = new PRHeaderDto();
        private List<PRLinesDto> PRLines = new List<PRLinesDto>();
        private CodeResponse? LastCodeResponse = new CodeResponse();
        private DocEntryResponse? LastDocEntryResponse = new DocEntryResponse();
        private DocNumResponse? LastDocNumSapResponse = new DocNumResponse();
        private DocNumUdtResponse? LastDocNumUdtResponse = new DocNumUdtResponse();
        private ProjectsResponse? ProjectResponse = new ProjectsResponse();
        private DepartmentsResponse? DepartmentResponse = new DepartmentsResponse();
        private BranchesResponse? BranchResponse = new BranchesResponse();
        private ItemsResponse? ItemResponse = new ItemsResponse();
        private WarehouseResponse? WhsResponse = new WarehouseResponse();
        private VatGroupsResponse? VatGroupResponse = new VatGroupsResponse();
        private ScopeOfWorkResponse? ScopeOfWorkResponse = new ScopeOfWorkResponse();
        private List<ProjectsDto>? Projects = new List<ProjectsDto>();
        private List<DepartmentsDto>? Departments = new List<DepartmentsDto>();
        private List<BranchesDto>? Branches = new List<BranchesDto>();
        private List<ItemsDto>? ItemMaster = new List<ItemsDto>();
        private List<WarehouseDto>? Warehouse = new List<WarehouseDto>();
        private List<VatGroupsDto>? VatGroup = new List<VatGroupsDto>();
        private List<ScopeOfWorkDto>? ScopeOfWork = new List<ScopeOfWorkDto>();
        private List<BreadcrumbItem> _items = new List<BreadcrumbItem>{new BreadcrumbItem("Requisition Slip", href: "requisition-slip", disabled: true)};

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await FetchDropdownData();
                int lastDocEntry = await GetLastDocEntry();
                int lastDocNum = await GetLastDocNum();
                docEntry = lastDocEntry + 1;
                docNum = lastDocNum + 1;
                await Autofill();
                AddTableRow();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
                throw;
            }
        }

        private void SelectProject(string selectedString)
        {
            PRHeader.U_ProjectID = selectedString;
            var search = Projects.Where(e => e.Code == selectedString).Select(e => e.Name);
            PRHeader.U_ProjName = search.First();
        }

        private void SelectItem(PRLinesDto context, string selectedString)
        {
            context.U_ItemCode = selectedString;
            context.U_MaterialCode = selectedString;
            var search = ItemMaster.Where(e => e.ItemCode == selectedString).Select(e => e.ItemName);
            context.U_Dscription = search.First();
            context.U_MaterialDesc = search.First();
        }

        private void SelectScopeOfWork(PRLinesDto context, string selectedString)
        {
            context.U_Scope = selectedString;
            var search = ScopeOfWork.Where(e => e.FactorCode == selectedString).Select(e => e.FactorDescription);
            context.U_ScopeDesc = search.First();
        }

        private void AddTableRow()
        {
            PRLines.Add(new PRLinesDto { U_DocEntry = PRHeader.U_DocEntry.ToString(), U_ItemCode = null, U_Dscription = null, U_WhsCode = null, U_BinLoc = null, U_Scope = null, U_ScopeDesc = null, U_MaterialCode = null, U_MaterialDesc = null, U_Quantity = null, U_InfoPrice = null, U_UomCode = null, U_ItemSpecification = null, U_TaxCode = null, U_OnHandQuantity = null, U_InventoryType = null, U_InventoryPurpose = null });
        }

        private void DeleteTableRow(int index)
        {
            PRLines.RemoveAt(index);
        }
        
        private string IncrementCode(string code)
        {
            return Regex.Replace(code, "\\d+", m => (int.Parse(m.Value) + 1).ToString(new string('0', m.Value.Length)));
        }

        private async Task OnSubmit(EditContext context)
        {
            try
            {
                HttpContent header = new StringContent(JsonConvert.SerializeObject(PRHeader), Encoding.UTF8, "application/json");
                var headerResponse = await HttpClient.PostAsync("U_FT_OPRQ", header);
                foreach (var line in PRLines)
                {
                    string nextCode = await GetNextCode("U_FT_PRQ1?$select=Code&$orderby=Code desc&$top=1");
                    line.Code = nextCode;
                    line.Name = nextCode;
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(line), Encoding.UTF8, "application/json");
                    var linesResponse = await HttpClient.PostAsync("U_FT_PRQ1", content);
                }

                Snackbar.Add("The Purchase Request was successfully posted!", Severity.Success);
                success = true;
                StateHasChanged();

                PRHeader = new PRHeaderDto();
                PRLines = new List<PRLinesDto>();
                int lastDocEntry = await GetLastDocEntry();
                int lastDocNum = await GetLastDocNum();
                docEntry = lastDocEntry + 1;
                docNum = lastDocNum + 1;
                await Autofill();
                AddTableRow();
            }
            catch (Exception ex)
            {
                Snackbar.Add(ex.Message, Severity.Error);
                throw;
            }
        }

        private async Task<string> GetNextCode(string query)
        {
            string lastCode = "PR0000000000";
            var lastCodeResponse = await HttpClient.GetAsync(query);
            LastCodeResponse = await lastCodeResponse.Content.ReadFromJsonAsync<CodeResponse>();
            if (LastCodeResponse.value.Count != 0)
                lastCode = LastCodeResponse.value[0].Code;
            lastCode = IncrementCode(lastCode);
            return lastCode;
        }        

        private async Task<int> GetLastDocNum()
        {
            int docNum = 0;
            int lastDocNumSAP = 0;
            int lastDocNumUdt = 0;
            using var lastDocNumSAPResponse = await HttpClient.GetAsync("PurchaseRequests?$select=DocNum&$orderby=DocNum desc&$top=1");
            LastDocNumSapResponse = await lastDocNumSAPResponse.Content.ReadFromJsonAsync<DocNumResponse>();
            using var lastDocNumUdtResponse = await HttpClient.GetAsync("U_FT_OPRQ?$select=U_DocNum&$orderby=U_DocEntry desc&$top=1");
            LastDocNumUdtResponse = await lastDocNumUdtResponse.Content.ReadFromJsonAsync<DocNumUdtResponse>();
            if (LastDocNumSapResponse.value.Count != 0)
                lastDocNumSAP = LastDocNumSapResponse.value[0].DocNum;
            if (LastDocNumUdtResponse.value.Count != 0)
                lastDocNumUdt = LastDocNumUdtResponse.value[0].U_DocNum;
            if (lastDocNumUdt > lastDocNumSAP)
            {
                docNum = lastDocNumUdt;
            }
            else
                docNum = lastDocNumSAP;
            Console.WriteLine($"SAP DocNum: {lastDocNumSAP}");
            Console.WriteLine($"UDT DocNum: {lastDocNumUdt}");
            return docNum;
        }

        private async Task<int> GetLastDocEntry()
        {
            int lastDocEntry = 0;
            using var lastDocEntryResponse = await HttpClient.GetAsync("U_FT_OPRQ?$select=U_DocEntry&$orderby=U_DocEntry desc&$top=1");
            LastDocEntryResponse = await lastDocEntryResponse.Content.ReadFromJsonAsync<DocEntryResponse>();
            if (LastDocEntryResponse.value.Count != 0)
                lastDocEntry = LastDocEntryResponse.value[0].U_DocEntry;
            return lastDocEntry;
        }

        private async Task FetchDropdownData()
        {
            using var projectsResponse = await HttpClient.GetAsync("Projects?$select=Code,Name");
            ProjectResponse = await projectsResponse.Content.ReadFromJsonAsync<ProjectsResponse>();
            Projects = ProjectResponse.value;
            if (!projectsResponse.IsSuccessStatusCode)
            {
                AuthService.Logout();
                Navigation.NavigateTo("/");
            }

            using var departmentsResponse = await HttpClient.GetAsync("Departments?$select=Code,Name");
            DepartmentResponse = await departmentsResponse.Content.ReadFromJsonAsync<DepartmentsResponse>();
            Departments = DepartmentResponse.value;
            using var branchesResponse = await HttpClient.GetAsync("Branches?$select=Code,Name");
            BranchResponse = await branchesResponse.Content.ReadFromJsonAsync<BranchesResponse>();
            Branches = BranchResponse.value;
            using var itemsResponse = await HttpClient.GetAsync("Items?$select=ItemCode,ItemName");
            ItemResponse = await itemsResponse.Content.ReadFromJsonAsync<ItemsResponse>();
            ItemMaster = ItemResponse.value;
            using var whsResponse = await HttpClient.GetAsync("Warehouses?$select=WarehouseCode, WarehouseName&$orderby=WarehouseCode asc");
            WhsResponse = await whsResponse.Content.ReadFromJsonAsync<WarehouseResponse>();
            Warehouse = WhsResponse.value;
            using var vatGroupResponse = await HttpClient.GetAsync("VatGroups?$select=Code,Name&$orderby = Name asc");
            VatGroupResponse = await vatGroupResponse.Content.ReadFromJsonAsync<VatGroupsResponse>();
            VatGroup = VatGroupResponse.value;
            using var scopeOfWorkResponse = await HttpClient.GetAsync("DistributionRules?$select=FactorCode&$filter=InWhichDimension eq 2");
            ScopeOfWorkResponse = await scopeOfWorkResponse.Content.ReadFromJsonAsync<ScopeOfWorkResponse>();
            ScopeOfWork = ScopeOfWorkResponse.value;
        }

        private async Task Autofill()
        {
            string nextCode = await GetNextCode("U_FT_OPRQ?$select=Code&$orderby=Code desc&$top=1");
            PRHeader.Code = nextCode;
            PRHeader.Name = nextCode;
            PRHeader.U_DocNum = docNum.ToString();
            PRHeader.U_DocEntry = docEntry;
            PRHeader.U_DocStatus = "Waiting For Approval";
            PRHeader.U_CardCode = LocalStorage.GetItem<string>("UserCode");
            PRHeader.U_PreparedBy = LocalStorage.GetItem<string>("UserName");
            PRHeader.U_Department = LocalStorage.GetItem<string>("Department");
            PRHeader.U_Branch = LocalStorage.GetItem<string>("Branch");
        }
    }
}