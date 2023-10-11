using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Setup.ViewModels
{
    public interface IPageViewModel
    {
        string Caption { get; }
        string Description { get; }
    }
}
