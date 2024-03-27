using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

using AtsEx.Setup.Models;

namespace AtsEx.Setup.ViewModels
{
    public class AbortedPageViewModel : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly CompletedPageModel Model = new CompletedPageModel();

        public string Caption { get; } = "AtsEX のインストールを中止しました";
        public string Description { get; } = "エラーが発生したか、またはユーザーの操作により、AtsEX のインストールは中止されました。";

        public ReadOnlyReactivePropertySlim<string> Detail { get; }

        public ReactiveCommand FinishCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AbortedPageViewModel()
        {
            Detail = Navigator.Instance.ErrorDetail.ToReadOnlyReactivePropertySlim().AddTo(Disposables);

            FinishCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.Finish);
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
