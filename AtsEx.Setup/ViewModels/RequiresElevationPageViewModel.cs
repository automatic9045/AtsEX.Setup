using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

using AtsEx.Setup.Installing;
using AtsEx.Setup.Models;

namespace AtsEx.Setup.ViewModels
{
    public class RequiresElevationPageViewModel : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly RequiresElevationPageModel Model = new RequiresElevationPageModel();

        public string Caption { get; } = "管理者権限が必要です";
        public string Description { get; } = "指定されたフォルダへ AtsEX をインストールするには管理者権限が必要です。インストールを続行する方法を選択してください。";

        public ReadOnlyReactivePropertySlim<bool> CanUseSimpleInstall { get; }
        public ReadOnlyReactivePropertySlim<bool> CanUseCopyBve { get; }

        public ReactivePropertySlim<ElevationOption> Option { get; }
        public ReactivePropertySlim<ImageSource> ShieldIcon { get; }

        public ReactiveCommand GoBackCommand { get; }
        public ReactiveCommand GoNextCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RequiresElevationPageViewModel()
        {
            CanUseSimpleInstall = Observable.Merge(TargetPath.Bve6Path, TargetPath.Bve5Path).Select(_ =>
            {
                bool? bve6 = TargetPath.Bve6Path.Value is null ? true : CallerInfo.TryCreateFromBvePath(TargetPath.Bve6Path.Value)?.CanUseSimpleInstall;
                bool? bve5 = TargetPath.Bve5Path.Value is null ? true : CallerInfo.TryCreateFromBvePath(TargetPath.Bve5Path.Value)?.CanUseSimpleInstall;

                return (bve6 ?? false) && (bve5 ?? false);
            }).ToReadOnlyReactivePropertySlim().AddTo(Disposables);

            CanUseCopyBve = Observable.Merge(TargetPath.Bve6Path, TargetPath.Bve5Path)
                .Select(_ => !(TargetPath.Bve6Path.Value is null && TargetPath.Bve5Path.Value is null))
                .ToReadOnlyReactivePropertySlim().AddTo(Disposables);

            Option = new ReactivePropertySlim<ElevationOption>(ElevationOption.RunAsAdmin).AddTo(Disposables);
            ShieldIcon = new ReactivePropertySlim<ImageSource>(StockIcons.GetShieldIcon()).AddTo(Disposables);

            GoBackCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.GoBack);
            GoNextCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(() =>
            {
                switch (Option.Value)
                {
                    case ElevationOption.RunAsAdmin:
                        Model.RunAsAdmin();
                        break;
                    case ElevationOption.SimpleInstall:
                        Model.SimpleInstall();
                        break;
                    case ElevationOption.CopyBve:
                        MessageBoxResult result = MessageBox.Show(
                            "[BVE 本体をコピー] は BVE Trainsim 本体の全てのファイルをコピーするため、インストールに時間がかかります。" +
                            "その他のオプションがどうしても利用できない場合にのみ選択してください。\n\n" +
                            "この設定でインストールを開始しても本当によろしいですか?", "AtsEX セットアップウィザード", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result != MessageBoxResult.Yes) break;

                        Model.InstallWithCopyBve();
                        break;
                    default:
                        throw new NotSupportedException();
                }
            });
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
