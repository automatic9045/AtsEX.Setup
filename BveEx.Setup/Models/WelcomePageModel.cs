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

using BveEx.Setup.Releases;

namespace BveEx.Setup.Models
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
                isNotLatest = BveExVersion.Current < BveExVersion.Latest;
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
