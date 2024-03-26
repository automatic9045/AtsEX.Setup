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

using AtsEx.Setup.Models;
using AtsEx.Setup.Releases;

namespace AtsEx.Setup.ViewModels
{
    public class ConfirmPageViewModel : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly ConfirmPageModel Model = new ConfirmPageModel();

        public string Caption { get; } = "インストールを開始します";
        public string Description { get; } = "以下の構成で AtsEX のインストールを開始します。";

        public ReadOnlyReactivePropertySlim<string> Bve6Path { get; }
        public ReadOnlyReactivePropertySlim<string> Bve5Path { get; }
        public ReadOnlyReactivePropertySlim<string> ScenarioDirectory { get; }

        public ReactiveCommand GoBackCommand { get; }
        public ReactiveCommand BeginInstallCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ConfirmPageViewModel()
        {
            Bve6Path = TargetPath.Bve6Path.Select(x => x ?? "(AtsEX を BVE Trainsim 6 にインストールしない)").ToReadOnlyReactivePropertySlim().AddTo(Disposables);
            Bve5Path = TargetPath.Bve5Path.Select(x => x ?? "(AtsEX を BVE Trainsim 5 にインストールしない)").ToReadOnlyReactivePropertySlim().AddTo(Disposables);
            ScenarioDirectory = TargetPath.ScenarioDirectory.Select(x => x ?? "(AtsEX サンプルをインストールしない)").ToReadOnlyReactivePropertySlim().AddTo(Disposables);

            GoBackCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.GoBack);
            BeginInstallCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.BeginInstall);
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
