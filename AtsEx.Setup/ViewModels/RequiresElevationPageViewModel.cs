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
    public class RequiresElevationPageViewModel : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly RequiresElevationPageModel Model = new RequiresElevationPageModel();

        public string Caption { get; } = "管理者権限が必要です";
        public string Description { get; } = "指定されたフォルダへ AtsEX をインストールするには管理者権限が必要です。インストールを続行する方法を選択してください。";

        public ReactivePropertySlim<ElevationOption> Option { get; }

        public ReactiveCommand GoBackCommand { get; }
        public ReactiveCommand GoNextCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RequiresElevationPageViewModel()
        {
            Option = new ReactivePropertySlim<ElevationOption>(ElevationOption.RunAsAdmin).AddTo(Disposables);

            GoBackCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.GoBack);
            GoNextCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(() =>
            {
                switch (Option.Value)
                {
                    case ElevationOption.RunAsAdmin:
                        Model.RunAsAdmin();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            });
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
