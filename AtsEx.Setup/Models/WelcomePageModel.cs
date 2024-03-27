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

using AtsEx.Setup.Releases;

namespace AtsEx.Setup.Models
{
    internal class WelcomePageModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public WelcomePageModel()
        {
        }

        public void GoNext()
        {
            bool isNotLatest = false;
            try
            {
                isNotLatest = AtsExVersion.Current < AtsExVersion.Latest;
            }
            catch { }

            Navigator.Instance.Page.Value = isNotLatest ? Page.NotLatestVersion : Page.SelectBve6;
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
