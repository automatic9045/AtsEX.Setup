using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

namespace AtsEx.Setup.Models
{
    internal class ConfirmPageModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public ConfirmPageModel()
        {
        }

        public void GoBack()
        {
            Navigator.Instance.Page.Value = Page.Sdk;
        }

        public void BeginInstall()
        {
            ProcessEnumerator processEnumerator = new ProcessEnumerator();
            foreach (ProcessInfo processInfo in processEnumerator.GetProcesses())
            {
                if (!(processInfo.Path is null) && (processInfo.Path == TargetPath.Bve6Path.Value.Path || processInfo.Path == TargetPath.Bve5Path.Value.Path))
                {
                    if (MessageBox.Show($"適用対象となる BVE は、インストール前に終了しておく必要があります。\n[OK] をクリックすると '{processInfo.Path}' の実行を終了します。", "AtsEX セットアップウィザード", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        try
                        {
                            Process process = Process.GetProcessById((int)processInfo.Id);
                            process.Kill();
                            process.WaitForExit();
                        }
                        catch { }
                    }
                    else
                    {
                        return;
                    }
                }
            }

            Navigator.Instance.Page.Value = Page.Installing;
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
