using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    internal class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public ReactivePropertySlim<IPageViewModel> Page { get; }

        public AsyncReactiveCommand GoToWelcomePageCommand { get; }
        public ReactiveCommand MinimizeCommand { get; }
        public ReactiveCommand CloseCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            Page = new ReactivePropertySlim<IPageViewModel>(new PreparingPageViewModel()).AddTo(Disposables);

            MinimizeCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Minimize);
            CloseCommand = new ReactiveCommand(Navigator.Instance.CanClose).AddTo(Disposables).WithSubscribe(Close);

            GoToWelcomePageCommand = new AsyncReactiveCommand().AddTo(Disposables).WithSubscribe(async () => await Task.Run(() =>
            {
                Navigator.Instance.Start();
                Navigator.Instance.PageViewModel.Subscribe(x => Page.Value = x);
            }));
            GoToWelcomePageCommand.Execute();
        }

        private void Minimize()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void Close()
        {
            Application.Current.MainWindow.Close();
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
