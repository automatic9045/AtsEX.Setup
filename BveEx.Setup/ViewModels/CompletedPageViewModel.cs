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

using BveEx.Setup.Models;

namespace BveEx.Setup.ViewModels
{
    public class CompletedPageViewModel : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly CompletedPageModel Model = new CompletedPageModel();

        public string Caption { get; } = "BveEX のインストールが完了しました";
        public string Description { get; } = "BveEX はお使いの BVE Trainsim 6 / 5 へ正常にインストールされました。";

        public ReactiveCommand FinishCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CompletedPageViewModel()
        {
            FinishCommand = new ReactiveCommand().AddTo(Disposables).WithSubscribe(Model.Finish);
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
