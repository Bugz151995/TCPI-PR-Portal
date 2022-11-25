using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace TCPI_PR_Portal.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonymous = new ClaimsIdentity();
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymous)));
        }
    }
}
