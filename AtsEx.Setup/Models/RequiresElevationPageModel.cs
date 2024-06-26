using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

using AtsEx.Setup.Installing;

namespace AtsEx.Setup.Models
{
    internal class RequiresElevationPageModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public RequiresElevationPageModel()
        {
        }

        public void GoBack()
        {
            Navigator.Instance.Page.Value = Page.Confirm;
        }

        public void RunAsAdmin()
        {
            if (Permission.TryElevate())
            {
                Environment.Exit(0);
            }
        }

        public void SimpleInstall()
        {
            TargetPath.Bve6Path.Value.MarkAsInstalled();
            TargetPath.Bve5Path.Value.MarkAsInstalled();

            Navigator.Instance.Page.Value = Page.Installing;
        }

        public void InstallWithCopyBve()
        {
            TargetPath.CopyBve.Value = true;

            Navigator.Instance.Page.Value = Page.Installing;
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
