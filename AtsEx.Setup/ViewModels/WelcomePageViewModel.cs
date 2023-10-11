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

using AtsEx.Setup.Models;
using AtsEx.Setup.Releases;

namespace AtsEx.Setup.ViewModels
{
    public class WelcomePageViewModel : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly WelcomePageModel Model = new WelcomePageModel();

        public string Caption { get; } = "AtsEX セットアップウィザードへようこそ";
        public string Description { get; } = "お使いの BVE Trainsim 5 / 6 へ AtsEX を自動的にインストールします。";

        public ReactivePropertySlim<string> Version { get; }
        public ReactivePropertySlim<string> Edition { get; }

        public AsyncReactiveCommand GoNextCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public WelcomePageViewModel()
        {
            Version = new ReactivePropertySlim<string>($"Version {AtsExVersion.Current}").AddTo(Disposables);
            Edition = new ReactivePropertySlim<string>($"入力デバイスプラグイン版").AddTo(Disposables);

            GoNextCommand = new AsyncReactiveCommand().AddTo(Disposables).WithSubscribe(Model.GoNextAsync);
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
