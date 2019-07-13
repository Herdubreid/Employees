using BlazorState;
using Employees.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading;
using System.Threading.Tasks;

namespace Employees.Feature.AppState
{
    public partial class AppState
    {
        public class AddToastHandler : RequestHandler<AddToastAction, AppState>
        {
            AppState AppState => Store.GetState<AppState>();
            public override Task<AppState> Handle(AddToastAction aRequest, CancellationToken aCancellationToken)
            {
                AppState.ToastItems.Add(new Data.ToastItem
                {
                    Index = AppState.ToastItems.Count,
                    Header = aRequest.Header,
                    Body = aRequest.Body
                });
                return Task.FromResult(AppState);
            }
            public AddToastHandler(IStore aStore) : base(aStore) { }
        }

        public class DelToastHandler : RequestHandler<DelToastAction, AppState>
        {
            AppState AppState => Store.GetState<AppState>();
            public DelToastHandler(IStore aStore) : base(aStore) { }
            public override Task<AppState> Handle(DelToastAction aRequest, CancellationToken aCancellationToken)
            {
                var success = AppState.ToastItems.Remove(aRequest.Item);
                return Task.FromResult(AppState);
            }
        }
        public class LogInHandler : RequestHandler<LogInAction, AppState>
        {
            AbAuthenticationStateProvider Auth { get; set; }
            AppState AppState => Store.GetState<AppState>();
            public override Task<AppState> Handle(LogInAction aRequest, CancellationToken aCancellationToken)
            {
                AppState.Employee = aRequest.AbRow;
                AppState.Employee.Connected = true;
                Auth.Notify();
                return Task.FromResult(AppState);
            }
            public LogInHandler(IStore aStore, AuthenticationStateProvider auth) : base(aStore)
            {
                Auth = auth as AbAuthenticationStateProvider;
            }
        }
        public class LogOutHandler : RequestHandler<LogOutAction, AppState>
        {
            GlobalAppState GlobalAppState { get; set; }
            AbAuthenticationStateProvider Auth { get; set; }
            AppState AppState => Store.GetState<AppState>();
            public override Task<AppState> Handle(LogOutAction aRequest, CancellationToken aCancellationToken)
            {
                GlobalAppState.Employees
                    .Find(e => e.F0601161_AN8 == AppState.Employee.F0601161_AN8)
                    .Connected = false;
                AppState.Employee = null;
                Auth.Notify();
                return Task.FromResult(AppState);
            }
            public LogOutHandler(IStore aStore, GlobalAppState globalAppState, AuthenticationStateProvider auth) : base(aStore)
            {
                GlobalAppState = globalAppState;
                Auth = auth as AbAuthenticationStateProvider;
            }
        }
    }
}
