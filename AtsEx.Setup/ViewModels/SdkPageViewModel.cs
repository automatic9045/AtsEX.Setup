using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

using AtsEx.Setup.Models;

namespace AtsEx.Setup.ViewModels
{
    internal class SdkPageViewModel : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly SdkPageModel Model = new SdkPageModel();

        public string Caption { get; } = "AtsEX プラグインの開発を予定されていますか?";
        public string Description { get; } = "AtsEX SDK は、AtsEX プラグインの開発を支援するパッケージです。AtsEX プラグインを開発される場合はインストールをおすすめします。";

        public ReactivePropertySlim<bool> InstallSdk { get; }
        public ReadOnlyReactivePropertySlim<bool> DoNotInstallSdk { get; }

        public ReactiveCommand GoBackCommand { get; }
        public ReactiveCommand GoNextCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        internal SdkPageViewModel()
        {
            InstallSdk = new ReactivePropertySlim<bool>(false).AddTo(Disposables);
            DoNotInstallSdk = InstallSdk.Inverse().ToReadOnlyReactivePropertySlim().AddTo(Disposables);

            GoBackCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.GoBack);
            GoNextCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(() => Model.GoNext(InstallSdk.Value));
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
