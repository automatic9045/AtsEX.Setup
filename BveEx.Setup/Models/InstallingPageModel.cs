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

using BveEx.Setup.Installing;
using System.Security.Principal;

namespace BveEx.Setup.Models
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
                    using (Installer installer = new Installer(stateReporter))
                    {
                        await Task.Run(() =>
                        {
                            if (TargetPath.CopyBve.Value)
                            {
                                if (TargetPath.Bve6Path.Value.HasInstalled)
                                {
                                    string newBve5Path = installer.CopyBve(TargetPath.Bve5Path.Value.Path, 5);
                                    TargetPath.Bve5Path.Value = new InstallationTarget(newBve5Path);
                                }
                                else
                                {
                                    string newBve6Path = installer.CopyBve(TargetPath.Bve6Path.Value.Path, 6);
                                    TargetPath.Bve6Path.Value = new InstallationTarget(newBve6Path);
                                }

                                TargetPath.CopyBve.Value = false;
                            }

                            installer.Install();
                        });
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    // 管理者権限かどうかを確認
                    using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
                    {
                        WindowsPrincipal principal = new WindowsPrincipal(identity);
                        if (principal.IsInRole(WindowsBuiltInRole.Administrator))
                        {
                            Navigator.Instance.Abort($"エラーが発生しました。\n" +
                                $"以下のことをお試しください。\n" +
                                $"・ PC を再起動の上もう一度お試しください。\n" +
                                $"・ ウィルス対策ソフトがインストールされている場合はこのインストーラーを許可リストに追加してください。\n" +
                                $"\nエラーの詳細：\n{ex.Message}");
                            return;
                        }
                    }
                    Navigator.Instance.Page.Value = Page.RequiresElevation;
                    return;
                }
                catch (IOException ex)
                {
                    switch (ex.HResult)
                    {
                        case unchecked((int)0x80070020):
                            Navigator.Instance.Abort($"{ex.Message}\n当該リソースを解放するか、PC を再起動の上もう一度お試しください。");
                            return;
                        default:
                            throw;
                    }
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
            if (MessageBox.Show("本当にインストールを中止してもよろしいですか?", "BveEX セットアップウィザード", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
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
