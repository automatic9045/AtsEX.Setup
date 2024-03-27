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
using AtsEx.Setup.Releases;

namespace AtsEx.Setup.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public ReactiveProperty<IPageViewModel> Page { get; }

        public AsyncReactiveCommand GoToWelcomePageCommand { get; }
        public ReactiveCommand MinimizeCommand { get; }
        public ReactiveCommand CloseCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            Page = Navigator.Instance.Page.Select(x => x.Convert()).ToReactiveProperty().AddTo(Disposables);

            MinimizeCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Minimize);
            CloseCommand = new ReactiveCommand(Navigator.Instance.CanClose).AddTo(Disposables).WithSubscribe(Close);

            GoToWelcomePageCommand = new AsyncReactiveCommand().AddTo(Disposables).WithSubscribe(async () =>
            {
                await AtsExVersion.CommunicateAsync();
                Navigator.Instance.Page.Value = Setup.Page.Welcome;
            });
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
