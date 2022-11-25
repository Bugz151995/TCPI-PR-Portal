using Microsoft.AspNetCore.Components.Authorization;

namespace TCPI_PR_Portal.Services
{
    public interface IAuthService
    {
        void Login(string employeeName, string role);
        void Logout();
    }
}