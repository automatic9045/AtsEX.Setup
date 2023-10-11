using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    internal class AbortedPageModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public AbortedPageModel()
        {
        }

        public void Finish()
        {
            Application.Current.Shutdown();
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
