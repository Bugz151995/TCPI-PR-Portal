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
using Newtonsoft.Json.Linq;

namespace TCPI_PR_Portal.Pages
{
    public partial class RequisitionSlip
    {
        private bool success = false;
        private int docEntry = 0;
        private int docNum = 0;
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
        private List<BreadcrumbItem> _items = new List<BreadcrumbItem> { new BreadcrumbItem("Requisition Slip", href: "requisition-slip", disabled: true) };

        private List<object> ItemCodeList = new List<object>();
        private List<object> ItemNameList = new List<object>();
        /// <summary>
        /// Override function when the component initializes
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                await PopulateDropdownFields();
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

        /// <summary>
        /// Callback method for the autofill of the Project Name.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="selectedString"></param>
        private void SelectProject(PRHeaderDto context, string selectedString)
        {
            context.U_ProjectID = selectedString;
            var search = Projects.Where(e => e.Code == selectedString).Select(e => e.Name);
            context.U_ProjName = search.First();
        }

        /// <summary>
        /// Callback method for the autofill of the Item Description and Material Code and Material Description.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="selectedString"></param>
        private void SelectItem(PRLinesDto context, string selectedString)
        {
            context.U_ItemCode = selectedString;
            context.U_MaterialCode = selectedString;
            var search = ItemMaster.Where(e => e.ItemCode == selectedString).Select(e => e.ItemName);
            context.U_Dscription = search.First();
            context.U_MaterialDesc = search.First();
        }

        /// <summary>
        /// Callback method for the autofill of the Scope Description.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="selectedString"></param>
        private void SelectScopeOfWork(PRLinesDto context, string selectedString)
        {
            context.U_Scope = selectedString;
            var search = ScopeOfWork.Where(e => e.FactorCode == selectedString).Select(e => e.FactorDescription);
            context.U_ScopeDesc = search.First();
        }

        /// <summary>
        /// Reset the Purchase Requisition Header DTO and Purchase Requisition Lines DTO.
        /// </summary>
        /// <returns></returns>
        private void ResetFields()
        {
            PRHeader = new PRHeaderDto();
            PRLines = new List<PRLinesDto>();
        }

        /// <summary>
        /// Add Row to the table in lines by assigning values to the DTO.
        /// </summary>
        private void AddTableRow()
        {
            PRLines.Add(new PRLinesDto { U_DocEntry = PRHeader.U_DocEntry.ToString(), U_ItemCode = null, U_Dscription = null, U_WhsCode = null, U_BinLoc = null, U_Scope = null, U_ScopeDesc = null, U_MaterialCode = null, U_MaterialDesc = null, U_Quantity = null, U_InfoPrice = null, U_UomCode = null, U_ItemSpecification = null, U_TaxCode = null, U_OnHandQuantity = null, U_InventoryType = null, U_InventoryPurpose = null });
        }

        /// <summary>
        /// Deletes the specific row in the table through an index assigned to every row
        /// </summary>
        /// <param name="index"></param>
        private void DeleteTableRow(int index)
        {
            PRLines.RemoveAt(index);
        }

        /// <summary>
        /// Increments the Code of the Document using Regex to match the digits and increment it by one.
        /// </summary>
        /// <param name="code"></param>
        /// <returns>The new Code of the Document</returns>
        private string IncrementComplexId(string code)
        {
            return Regex.Replace(code, "\\d+", m => (int.Parse(m.Value) + 1).ToString(new string('0', m.Value.Length)));
        }

        /// <summary>
        /// Post data to Purchase Requisition Header and Purchase Requisition Lines
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task OnSubmit(EditContext context)
        {
            try
            {
                HttpContent header = new StringContent(JsonConvert.SerializeObject(PRHeader), Encoding.UTF8, "application/json");
                var headerResponse = await HttpClient.PostAsync("U_FT_OPRQ", header);

                if (!headerResponse.IsSuccessStatusCode)
                {
                    Snackbar.Add("Oops! Something went wrong. Please try again.", Severity.Error);
                    return;
                }

                foreach (var line in PRLines)
                {
                    string nextCode = await GetNextCode("U_FT_PRQ1?$select=Code&$orderby=Code desc&$top=1");
                    line.Code = nextCode;
                    line.Name = nextCode;
                    HttpContent content = new StringContent(JsonConvert.SerializeObject(line), Encoding.UTF8, "application/json");
                    var linesResponse = await HttpClient.PostAsync("U_FT_PRQ1", content);

                    if (!linesResponse.IsSuccessStatusCode)
                    {
                        Snackbar.Add($"Oops! The Item with Code of {line.Code} did not submit.", Severity.Error);
                    }
                }

                Snackbar.Add("The Purchase Request was successfully submitted!", Severity.Success);
                success = true;
                StateHasChanged();
                ResetFields();
                await Autofill();
                AddTableRow();
            }
            catch (Exception ex)
            {
                Snackbar.Add("Oops! Something went wrong. Please try again.", Severity.Error);
                throw;
            }
        }

        /// <summary>
        /// Assign a unique Code for the Document to be posted
        /// </summary>
        /// <param name="query">
        /// A query string that fetches the last record in the table
        /// </param>
        /// <returns>The new Code for the Document</returns>
        private async Task<string> GetNextCode(string lastRecordQuery)
        {
            string code = "PR0000000000";
            var result = await HttpClient.GetAsync(lastRecordQuery);
            LastCodeResponse = await result.Content.ReadFromJsonAsync<CodeResponse>();

            if (LastCodeResponse.value.Count != 0)
                code = LastCodeResponse.value[0].Code;
            code = IncrementComplexId(code);
            return code;
        }

        /// <summary>
        /// Get the last DocNum in either Purchase Request Header's UDT or standard SAP table OPRQ.
        /// </summary>
        /// <returns>The last record in either the UDT or standard SAP</returns>
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
            {
                docNum = lastDocNumSAP;
            }

            return docNum;
        }

        /// <summary>
        /// Get the last DocEntry from Purchase Requisition Header
        /// </summary>
        /// <returns></returns>
        private async Task<int> GetLastDocEntry()
        {
            int docEntry = 0;
            using var result = await HttpClient.GetAsync("U_FT_OPRQ?$select=U_DocEntry&$orderby=U_DocEntry desc&$top=1");
            LastDocEntryResponse = await result.Content.ReadFromJsonAsync<DocEntryResponse>();
            if (LastDocEntryResponse.value.Count != 0)
                docEntry = LastDocEntryResponse.value[0].U_DocEntry;
            return docEntry;
        }

        //TODO: Refactor Queries
        //TODO: We can attach the fetching of data on the onclick event to load the page much faster.
        private async Task PopulateDropdownFields()
        {
            using var projectsResponse = await HttpClient.GetAsync("Projects?$select=Code,Name");
            ProjectResponse = await projectsResponse.Content.ReadFromJsonAsync<ProjectsResponse>();
            Projects = ProjectResponse.value;
            using var departmentsResponse = await HttpClient.GetAsync("Departments?$select=Code,Name");
            DepartmentResponse = await departmentsResponse.Content.ReadFromJsonAsync<DepartmentsResponse>();
            Departments = DepartmentResponse.value;
            using var branchesResponse = await HttpClient.GetAsync("Branches?$select=Code,Name");
            BranchResponse = await branchesResponse.Content.ReadFromJsonAsync<BranchesResponse>();
            Branches = BranchResponse.value;
            //using var itemsResponse = await HttpClient.GetAsync("Items?$select=ItemCode,ItemName");
            //ItemResponse = await itemsResponse.Content.ReadFromJsonAsync<ItemsResponse>();
            //ItemMaster = ItemResponse.value;
            using var whsResponse = await HttpClient.GetAsync("Warehouses?$select=WarehouseCode, WarehouseName&$orderby=WarehouseCode asc");
            WhsResponse = await whsResponse.Content.ReadFromJsonAsync<WarehouseResponse>();
            Warehouse = WhsResponse.value;
            using var vatGroupResponse = await HttpClient.GetAsync("VatGroups?$select=Code,Name&$orderby = Name asc");
            VatGroupResponse = await vatGroupResponse.Content.ReadFromJsonAsync<VatGroupsResponse>();
            VatGroup = VatGroupResponse.value;
            using var scopeOfWorkResponse = await HttpClient.GetAsync("DistributionRules?$select=FactorCode&$filter=InWhichDimension eq 2");
            ScopeOfWorkResponse = await scopeOfWorkResponse.Content.ReadFromJsonAsync<ScopeOfWorkResponse>();
            ScopeOfWork = ScopeOfWorkResponse.value;

            ItemCodeList = await CreateList("Items?$select=ItemCode");
            ItemNameList = await CreateList("Items?$select=ItemName");
        }

        /// <summary>
        /// Auto fill some of the header fields
        /// </summary>
        /// <returns></returns>
        private async Task Autofill()
        {
            string nextCode = await GetNextCode("U_FT_OPRQ?$select=Code&$orderby=Code desc&$top=1");
            int lastDocEntry = await GetLastDocEntry();
            int lastDocNum = await GetLastDocNum();
            PRHeader.Code = nextCode;
            PRHeader.Name = nextCode;
            PRHeader.U_DocNum = Convert.ToString(++lastDocNum);
            PRHeader.U_DocEntry = ++lastDocEntry;
            PRHeader.U_DocStatus = "Waiting For Approval";
            PRHeader.U_CardCode = LocalStorage.GetItem<string>("UserCode");
            PRHeader.U_PreparedBy = LocalStorage.GetItem<string>("UserName");
            PRHeader.U_Department = LocalStorage.GetItem<string>("Department");
            PRHeader.U_Branch = LocalStorage.GetItem<string>("Branch");
        }

        private void OnValueChanged(PRLinesDto context, object value)
        {
            context.U_ItemCode = value.ToString();
            context.U_Dscription = value.ToString();
            context.U_MaterialCode = value.ToString();
            context.U_MaterialDesc = value.ToString();
        }

        private async Task<IEnumerable<object>> SearchItemCode(string value)
        {
            Console.WriteLine(JsonConvert.SerializeObject(ItemCodeList));
            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return ItemCodeList;
            return ItemCodeList.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<object>> SearchItemName(string value)
        {
            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
                return ItemNameList;
            return ItemNameList.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<List<object>> CreateList(string query)
        {
            List<object> items = new List<object>();
            JObject json;
            do
            {
                var result = await GetData(query);
                json = JObject.Parse(result);
                items.AddRange(json["value"].ToObject<List<object>>());
                if (json.ContainsKey("odata.nextLink"))
                    query = json["odata.nextLink"].ToString();
            } while (json.ContainsKey("odata.nextLink"));

            return items;
        }

        private async Task<string> GetData(string query)
        {
            using var response = await HttpClient.GetAsync(query);
            if (!response.IsSuccessStatusCode)
            {
                Snackbar.Add("Oops! Something went wrong. This might be a server fault. Try to log-out and log-in.", Severity.Error);
            }

            string content = await response.Content.ReadAsStringAsync();
            return content;
        }
    }
}