using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

using BveEx.Setup.Models;
using BveEx.Setup.Releases;

namespace BveEx.Setup.ViewModels
{
    public class NotLatestVersionPageViewModel : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly NotLatestVersionPageModel Model = new NotLatestVersionPageModel();

        public string Caption { get; } = "この BveEX は最新版ではありません";
        public string Description { get; } = "更に新しいバージョンの BveEX がリリースされています。このインストーラーでインストールされる BveEX は最新のものではありません。";

        public ReactivePropertySlim<string> CurrentVersion { get; }
        public ReactivePropertySlim<string> LatestVersion { get; }

        public ReactiveCommand GoBackCommand { get; }
        public ReactiveCommand GetLatestCommand { get; }
        public ReactiveCommand ContinueCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public NotLatestVersionPageViewModel()
        {
            CurrentVersion = new ReactivePropertySlim<string>($"Version {BveExVersion.Current}").AddTo(Disposables);
            LatestVersion = new ReactivePropertySlim<string>($"Version {BveExVersion.Latest}").AddTo(Disposables);

            GoBackCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.GoBack);
            GetLatestCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.GetLatest);
            ContinueCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.Continue);
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
