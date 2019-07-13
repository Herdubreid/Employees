using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Data
{
    public class ToastItem : IEquatable<ToastItem>
    {
        public int Index { get; set; }
        public string Header { get; set; }
        public string Body { get; set; }

        public bool Equals(ToastItem other)
        {
            return other.Index == Index;
        }
    }
}
