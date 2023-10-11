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

        public async Task GoNextAsync()
        {
            bool isNotLatest = false;
            try
            {
                Version latest = await Task.Run(() => AtsExVersion.Latest);
                isNotLatest = AtsExVersion.Current < latest;
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
