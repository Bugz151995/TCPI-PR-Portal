﻿using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace TCPI_PR_Portal.Client
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }
    }
}
