using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Solutaris.InfoWARE.ProtectedBrowserStorage.Services;
using System.Security.Claims;
using System.Security.Principal;
using TCPI_PR_Portal.Client;

namespace TCPI_PR_Portal.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IIWLocalStorageService _localStorage;

        public AuthService(AuthenticationStateProvider authenticationStateProvider,
                           IIWLocalStorageService SessionStorage)
        {
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = SessionStorage;
        }

        public void Login(string employeeName, string role)
        {
            ((CustomAuthStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(employeeName, role);
        }

        public void Logout()
        {
            _localStorage.RemoveAllItems();
            ((CustomAuthStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        }
    }
}