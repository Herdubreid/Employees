using BlazorState;
using Employees.Feature.AppState;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Employees.Data
{
    public class AbAuthenticationStateProvider : AuthenticationStateProvider
    {
        protected IStore Store { get; set; }
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state = Store.GetState<AppState>();
            if (state.Employee != null)
            {
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, state.Employee.F0601161_ALPH)
                }, "AbAuthentication");
                var user = new ClaimsPrincipal(identity);
                return Task.FromResult(new AuthenticationState(user));
            }
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
        }
        public void Notify()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public AbAuthenticationStateProvider(IStore aStore)
        {
            Store = aStore;
        }
    }
}
