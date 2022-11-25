using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Solutaris.InfoWARE.ProtectedBrowserStorage.Services;
using System.Security.Claims;


namespace TCPI_PR_Portal.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        protected readonly IIWLocalStorageService _localStorage;

        public CustomAuthStateProvider(IIWLocalStorageService LocalStorage)
        {
            _localStorage = LocalStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string employeeName = await _localStorage.GetItemAsync<string>("EmployeeName");
            string role = await _localStorage.GetItemAsync<string>("Role");

            Console.WriteLine("Authentication State is being executed!");
            Console.WriteLine($"value of employeeName: {employeeName}");
            Console.WriteLine($"value of role: {role}");

            if (employeeName != null && role != null)
            {
                Console.WriteLine("the local storage has value");
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, employeeName),
                    new Claim(ClaimTypes.Role, role)
                };

                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims))));
            }

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        public void MarkUserAsAuthenticated(string employeeName, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employeeName),
                new Claim(ClaimTypes.Role, role)
            };
            var identity = new ClaimsIdentity(claims);
            var authenticatedUser = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(anonymousUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
