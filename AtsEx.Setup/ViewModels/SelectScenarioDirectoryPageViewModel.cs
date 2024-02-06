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
    public class SelectScenarioDirectoryPageViewModel : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly SelectScenarioDirectoryPageModel Model = new SelectScenarioDirectoryPageModel();

        public string Caption { get; } = "シナリオフォルダを探す";
        public string Description { get; } = "AtsEX サンプルシナリオを配置するシナリオフォルダを選択してください。";

        private readonly ReactivePropertySlim<string> _Path = null;
        public ReadOnlyReactivePropertySlim<string> Path { get; }

        public ReactiveCommand OpenFolderCommand { get; }

        public ReactiveCommand GoBackCommand { get; }
        public ReactiveCommand GoNextCommand { get; }
        public ReactiveCommand SkipCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        internal SelectScenarioDirectoryPageViewModel()
        {
            string document = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string defaultScenarioDirectory = System.IO.Path.Combine(document, @"BveTs\Scenarios");

            _Path = new ReactivePropertySlim<string>(Directory.Exists(defaultScenarioDirectory) ? defaultScenarioDirectory : null);
            Path = _Path.Select(x => x ?? "(フォルダ未選択)").ToReadOnlyReactivePropertySlim().AddTo(Disposables);

            OpenFolderCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(() =>
            {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog()
                {
                    SelectedPath = defaultScenarioDirectory,
                    Description = "AtsEX サンプルシナリオを配置するシナリオフォルダを選択してください",
                    ShowNewFolderButton = true,
                };

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    _Path.Value = dialog.SelectedPath;
                }
            });

            GoBackCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.GoBack);
            GoNextCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(() => Model.GoNext(_Path.Value));
            SkipCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(() => Model.GoNext(null));
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
