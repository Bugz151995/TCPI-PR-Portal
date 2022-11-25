using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Solutaris.InfoWARE.ProtectedBrowserStorage.Services;
using System.Security.Claims;
using System.Security.Principal;

namespace TCPI_PR_Portal.Services
{
    public class AuthService : IAuthService
    {
        public Task<AuthenticationState> MarkUserAsAuthenticated(string employeeName, string role)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, employeeName),
                    new Claim(ClaimTypes.Role, role)
                };

            var identity = new ClaimsIdentity(claims, "UserAuthentication");
            var authenticatedUser = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            return authState;
        }

        public Task<AuthenticationState> MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            return authState;
        }
    }
}