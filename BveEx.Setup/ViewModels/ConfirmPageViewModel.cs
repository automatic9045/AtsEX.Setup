using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
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
    public class ConfirmPageViewModel : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly ConfirmPageModel Model = new ConfirmPageModel();

        public string Caption { get; } = "インストールを開始します";
        public string Description { get; } = "以下の構成で BveEX のインストールを開始します。";

        public ReadOnlyReactivePropertySlim<string> Bve6Path { get; }
        public ReadOnlyReactivePropertySlim<string> Bve5Path { get; }
        public ReadOnlyReactivePropertySlim<string> ScenarioDirectory { get; }
        public ReadOnlyReactivePropertySlim<string> SdkOption { get; }

        public ReactiveCommand GoBackCommand { get; }
        public ReactiveCommand BeginInstallCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ConfirmPageViewModel()
        {
            Bve6Path = TargetPath.Bve6Path.Select(x => x.HasInstalled ? "(BveEX を BVE Trainsim 6 にインストールしない)" : x.Path).ToReadOnlyReactivePropertySlim().AddTo(Disposables);
            Bve5Path = TargetPath.Bve5Path.Select(x => x.HasInstalled ? "(BveEX を BVE Trainsim 5 にインストールしない)" : x.Path).ToReadOnlyReactivePropertySlim().AddTo(Disposables);
            ScenarioDirectory = TargetPath.ScenarioDirectory.Select(x => x.HasInstalled ? "(BveEX サンプルをインストールしない)" : x.Path).ToReadOnlyReactivePropertySlim().AddTo(Disposables);
            SdkOption = TargetPath.InstallSdk.Select(x => "SDK をインストール" + (x ? "する" : "しない")).ToReadOnlyReactivePropertySlim().AddTo(Disposables);

            GoBackCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.GoBack);
            BeginInstallCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.BeginInstall);
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
