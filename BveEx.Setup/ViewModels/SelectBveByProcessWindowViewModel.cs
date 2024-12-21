using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

using BveEx.Setup.Models;

namespace BveEx.Setup.ViewModels
{
    public class SelectBveByProcessWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly SelectBveByProcessWindowModel Model;

        public ReactivePropertySlim<bool> IsUpdating { get; }
        public ReactivePropertySlim<IReadOnlyList<ProcessInfo>> Processes { get; }
        public ReadOnlyReactivePropertySlim<int> ProcessCount { get; }
        public ReactiveProperty<int> SelectedProcessIndex { get; }
        public ReadOnlyReactivePropertySlim<string> SelectedProcessPath { get; }

        public ProcessInfo SelectedProcess
            => 0 <= SelectedProcessIndex.Value && SelectedProcessIndex.Value < Processes.Value.Count ? Processes.Value[SelectedProcessIndex.Value] : null;

        public AsyncReactiveCommand UpdateCommand { get; }
        public ReactiveCommand CancelCommand { get; }
        public ReactiveCommand SelectCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        internal SelectBveByProcessWindowViewModel(BveFileValidatorBase validator, IDialogService dialogService)
        {
            Model = new SelectBveByProcessWindowModel().AddTo(Disposables);

            IsUpdating = new ReactivePropertySlim<bool>(true).AddTo(Disposables);
            Processes = new ReactivePropertySlim<IReadOnlyList<ProcessInfo>>(new List<ProcessInfo>(0)).AddTo(Disposables);
            ProcessCount = Processes.Select(x => x.Count).ToReadOnlyReactivePropertySlim().AddTo(Disposables);
            SelectedProcessIndex = new ReactiveProperty<int>(-1).AddTo(Disposables);
            SelectedProcessPath = SelectedProcessIndex.Select(_ => SelectedProcess?.Path).ToReadOnlyReactivePropertySlim().AddTo(Disposables);

            UpdateCommand = new AsyncReactiveCommand(IsUpdating).AddTo(Disposables).WithSubscribe(async () =>
            {
                IsUpdating.Value = false;
                dialogService.Cursor = Cursors.Wait;
                await Task.Run(() => Processes.Value = Model.GetProcesses(validator));
                dialogService.Cursor = Cursors.Arrow;
                IsUpdating.Value = true;
            });

            CancelCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(() => dialogService.Close(false));
            SelectCommand = new ReactiveCommand(SelectedProcessIndex.Select(_ => !(SelectedProcess is null))).AddTo(Disposables).WithSubscribe(() => dialogService.Close(true));
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }


        internal interface IDialogService
        {
            Cursor Cursor { get; set; }
            void Close(bool? dialogResult);
        }
    }
}
