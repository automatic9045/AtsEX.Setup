using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

using AtsEx.Setup.Installing;

namespace AtsEx.Setup.Models
{
    internal class InstallingPageModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public InstallingPageModel()
        {
        }

        public async Task InstallAsync(IProgress<InstallationState> stateReporter)
        {
            try
            {
                try
                {
                    Installer installer = new Installer(stateReporter);
                    await Task.Run(() => installer.Install());
                }
                catch (UnauthorizedAccessException)
                {
                    if (Permission.TryElevate())
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Navigator.Instance.Abort("指定されたフォルダへのインストールには管理者権限が必要です。");
                        return;
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                Navigator.Instance.Abort(ex.ToString());
                return;
            }

            Navigator.Instance.Page.Value = Page.Completed;
        }

        public void Abort()
        {
            if (MessageBox.Show("本当にインストールを中止してもよろしいですか?", "AtsEX セットアップウィザード", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Navigator.Instance.Page.Value = Page.Welcome;
            }
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
