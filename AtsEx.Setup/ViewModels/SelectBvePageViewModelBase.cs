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
    public abstract class SelectBvePageViewModelBase : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly SelectBvePageModelBase Model;

        private protected abstract BveFileErrorConverter Converter { get; }

        public string Caption { get; }
        public string Description { get; }

        private readonly ReactivePropertySlim<string> _Path = null;
        public ReadOnlyReactivePropertySlim<string> Path { get; }

        private protected ReactivePropertySlim<BveFileError> Error { get; }
        public ReadOnlyReactivePropertySlim<string> ErrorText { get; }

        public ReactiveCommand OpenFileByProcessCommand { get; }
        public ReactiveCommand OpenFileCommand { get; }

        public ReactiveCommand GoBackCommand { get; }
        public ReactiveCommand GoNextCommand { get; }
        public ReactiveCommand SkipCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private protected SelectBvePageViewModelBase(SelectBvePageModelBase model, int bveVersion, string initialPath)
        {
            Model = model;

            Caption = $"BVE Trainsim {bveVersion} を探す";
            Description = $"AtsEX を適用する BVE Trainsim {bveVersion} を選択してください。";

            _Path = new ReactivePropertySlim<string>();
            Path = _Path.Select(x => x ?? "(ファイル未選択)").ToReadOnlyReactivePropertySlim().AddTo(Disposables);

            Error = new ReactivePropertySlim<BveFileError>(BveFileError.NotSelected).AddTo(Disposables);
            ErrorText = Error.Select(Converter.Convert).ToReadOnlyReactivePropertySlim().AddTo(Disposables);

            SelectBvePageModelBase.BveFileInfo defaultFile = initialPath is null ? null : Model.CreateFileInfo(initialPath);
            TrySetFileData(defaultFile);

            OpenFileByProcessCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(() =>
            {
                SelectBvePageModelBase.BveFileInfo file = Model.OpenFileByProcess();
                TrySetFileData(file);
            });
            OpenFileCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(() =>
            {
                string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                SelectBvePageModelBase.BveFileInfo file = Model.OpenFile(desktop);
                TrySetFileData(file);
            });

            GoBackCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.GoBack);
            GoNextCommand = new ReactiveCommand(Error.Select(
                x => x == BveFileError.None || x == BveFileError.MayNotBve))
                .AddTo(Disposables).WithSubscribe(() => Model.GoNext(_Path.Value));
            SkipCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(() => Model.GoNext(null));


            void TrySetFileData(SelectBvePageModelBase.BveFileInfo file)
            {
                if (!(file is null))
                {
                    _Path.Value = file.Path;
                    Error.Value = file.Error;
                }
            }
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
