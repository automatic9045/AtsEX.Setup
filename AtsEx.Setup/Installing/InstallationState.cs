using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Setup.Installing
{
    internal class InstallationState
    {
        public int Value { get; }
        public string Detail { get; }

        public InstallationState(int value, string detail)
        {
            Value = value;
            Detail = detail;
        }
    }
}
