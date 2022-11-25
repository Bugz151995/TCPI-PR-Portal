using Microsoft.AspNetCore.Components.Authorization;

namespace TCPI_PR_Portal.Services
{
    public interface IAuthService
    {
        Task Login(string employeeName, string role);
        Task Logout();
    }
}