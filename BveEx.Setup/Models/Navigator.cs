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

using BveEx.Setup.ViewModels;

namespace BveEx.Setup.Models
{
    internal class Navigator
    {
        public static Navigator Instance { get; private set; } = null;

        public static void Initialize(bool isInteractive)
        {
            Instance = new Navigator(isInteractive);
        }


        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public bool IsInteractive { get; }

        public ReactivePropertySlim<Page> Page { get; }
        public ReactivePropertySlim<bool> CanClose { get; }
        public ReactivePropertySlim<string> ErrorDetail { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private Navigator(bool isInteractive)
        {
            IsInteractive = isInteractive;

            Page = new ReactivePropertySlim<Page>(Setup.Page.Preparing).AddTo(Disposables);
            CanClose = new ReactivePropertySlim<bool>(true).AddTo(Disposables);
            ErrorDetail = new ReactivePropertySlim<string>(null).AddTo(Disposables);
        }

        public void Abort(string detail)
        {
            ErrorDetail.Value = detail;
            CanClose.Value = true;
            Page.Value = Setup.Page.Aborted;
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
