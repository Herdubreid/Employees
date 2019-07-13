using BlazorState;
using Employees.Data;
using System;
using System.Collections.ObjectModel;

namespace Employees.Feature.AppState
{
    public partial class AppState : State<AppState>
    {
        public ObservableCollection<ToastItem> ToastItems { get; set; }
        public F0601161Row Employee { get; set; }
        protected override void Initialize()
        {
            ToastItems = new ObservableCollection<ToastItem>
            {
                new ToastItem
                {
                    Header = "Welcome!",
                    Body = $"Initialised at {DateTime.Now.ToString()}."
                }
            };
        }
        public AppState() { }
    }
}
