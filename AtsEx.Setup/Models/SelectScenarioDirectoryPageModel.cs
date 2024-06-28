using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

namespace AtsEx.Setup.Models
{
    internal class SelectScenarioDirectoryPageModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public SelectScenarioDirectoryPageModel()
        {
        }

        public void GoBack()
        {
            Navigator.Instance.Page.Value = Page.SelectBve5;
        }

        public void GoNext(string path)
        {
            if (path is null)
            {
                if (MessageBox.Show("[スキップ] を選択した場合、AtsEX サンプルシナリオはお使いの環境にインストールされません。\n本当に AtsEX サンプルシナリオのインストールをスキップしてもよろしいですか?", "AtsEX セットアップウィザード", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                {
                    return;
                }
            }
            else if (!Directory.Exists(path))
            {
                MessageBox.Show($"指定されたフォルダ '{path}' は存在しません。\nもう一度よくご確認ください。", "AtsEX セットアップウィザード", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                TargetPath.ScenarioDirectory.Value = new InstallationTarget(path);
            }

            Navigator.Instance.Page.Value = Page.Sdk;
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
