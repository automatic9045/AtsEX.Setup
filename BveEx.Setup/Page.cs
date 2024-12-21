using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BveEx.Setup
{
    internal enum Page
    {
        Preparing,
        Aborted,
        Welcome,
        NotLatestVersion,
        SelectBve6,
        SelectBve5,
        SelectScenarioDirectory,
        Sdk,
        Confirm,
        Installing,
        RequiresElevation,
        Completed,
    }
}
