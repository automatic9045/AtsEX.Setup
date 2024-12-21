using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

namespace BveEx.Setup.Models
{
    internal class CompletedPageModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public CompletedPageModel()
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
