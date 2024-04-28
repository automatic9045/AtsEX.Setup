using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

namespace AtsEx.Setup.Models
{
    internal class SdkPageModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public SdkPageModel()
        {
        }

        public void GoBack()
        {
            Navigator.Instance.Page.Value = Page.SelectScenarioDirectory;
        }

        public void GoNext(bool installSdk)
        {
            TargetPath.InstallSdk.Value = installSdk;
            Navigator.Instance.Page.Value = Page.Confirm;
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
