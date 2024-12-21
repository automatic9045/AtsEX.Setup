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

using BveEx.Setup.Releases;
using BveEx.Setup.ViewModels;

namespace BveEx.Setup.Models
{
    internal class NotLatestVersionPageModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public NotLatestVersionPageModel()
        {
        }

        public void GoBack()
        {
            Navigator.Instance.Page.Value = Page.Welcome;
        }

        public void GetLatest()
        {
            Process.Start("https://www.okaoka-depot.com/AtsEX/");
            Application.Current.Shutdown();
        }

        public void Continue()
        {
            if (MessageBox.Show("このインストーラーでインストールされる BveEX は最新のものではありません。\n本当に続行してよろしいですか?",
                "BveEX セットアップウィザード", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Navigator.Instance.Page.Value = Page.SelectBve6;
            }
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
