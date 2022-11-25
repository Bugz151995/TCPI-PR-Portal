using Microsoft.AspNetCore.Components.Authorization;

namespace TCPI_PR_Portal.Services
{
    public interface IAuthService
    {
        Task<AuthenticationState> MarkUserAsAuthenticated(string employeeName, string role);
        Task<AuthenticationState> MarkUserAsLoggedOut();
    }
}