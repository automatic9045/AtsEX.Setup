using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

using BveEx.Setup.Installing;
using BveEx.Setup.Models;

namespace BveEx.Setup.ViewModels
{
    public class InstallingPageViewModel : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly InstallingPageModel Model = new InstallingPageModel();
        private readonly Progress<InstallationState> Progress;

        private int TargetProgressValueSpeed = 0;
        private int TargetProgressValue = 0;

        public string Caption { get; } = "BveEX をインストール中です";
        public string Description { get; } = "そのまましばらくお待ちください。";

        public ReactivePropertySlim<int> ProgressValue { get; }
        public ReactivePropertySlim<string> ProgressDetail { get; }

        public AsyncReactiveCommand InstallCommand { get; }
        public ReactiveCommand AbortCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public InstallingPageViewModel()
        {
            Progress = new Progress<InstallationState>(x =>
            {
                TargetProgressValue = x.Value;
                ProgressDetail.Value = x.Detail;
            });

            ProgressValue = new ReactivePropertySlim<int>().AddTo(Disposables);
            ProgressDetail = new ReactivePropertySlim<string>().AddTo(Disposables);

            InstallCommand = new AsyncReactiveCommand().AddTo(Disposables).WithSubscribe(async () =>
            {
                Navigator.Instance.CanClose.Value = false;
                ProgressValue.Value = 0;
                ProgressDetail.Value = "インストールを開始する準備をしています...";

                await Model.InstallAsync(Progress);

                Navigator.Instance.CanClose.Value = true;
            });
            AbortCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.Abort);

            Navigator.Instance.Page.Subscribe(x =>
            {
                if (x == Page.Installing) InstallCommand.Execute();
            });

            CompositionTarget.Rendering += OnRendering;
        }

        public void Dispose()
        {
            CompositionTarget.Rendering -= OnRendering;
            Disposables.Dispose();
        }

        private void OnRendering(object sender, EventArgs e)
        {
            if (Navigator.Instance.Page.Value != Page.Installing) return;

            if (ProgressValue.Value < TargetProgressValue)
            {
                int maxSpeed = GetMaxSpeed(TargetProgressValue - ProgressValue.Value);
                TargetProgressValueSpeed = Math.Min(TargetProgressValueSpeed + 1, maxSpeed);

                ProgressValue.Value += TargetProgressValueSpeed;
            }


            int GetMaxSpeed(int diff)
            {
                int max = 8;

                int sum = 0;
                for (int i = 1; i <= max; i++)
                {
                    sum += i;
                    if (diff <= sum) return i;
                }

                return max;
            }
        }
    }
}
