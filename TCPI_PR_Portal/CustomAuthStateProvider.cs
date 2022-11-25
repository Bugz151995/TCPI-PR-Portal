﻿using Microsoft.AspNetCore.Components;
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

            if (employeeName != null && role != null)
            {

                var claims = new List<Claim>();

                claims.Add(new Claim(ClaimTypes.Name, employeeName));
                claims.Add(new Claim(ClaimTypes.Role, role));

                return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, "test"))));
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
