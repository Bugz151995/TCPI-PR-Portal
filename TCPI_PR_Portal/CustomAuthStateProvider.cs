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

            var identity = new ClaimsIdentity();

            if (employeeName != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, employeeName),
                    new Claim(ClaimTypes.Role, role)
                };

                identity = new ClaimsIdentity(claims, "UserAuthentication");
            }
            

            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }
    }
}
