using Employees.Data;
using MediatR;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Employees.Feature.AppState
{
    public class AddToastAction : IRequest<AppState>
    {
        public string Header { get; set; }
        public string Body { get; set; }
    }

    public class DelToastAction : IRequest<AppState>
    {
        public ToastItem Item { get; set; }
    }

    public class LogInAction : IRequest<AppState>
    {
        public F0601161Row AbRow { get; set; }
    }

    public class LogOutAction : IRequest<AppState> { }

}
