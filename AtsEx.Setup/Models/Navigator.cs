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

using AtsEx.Setup.ViewModels;

namespace AtsEx.Setup.Models
{
    internal class Navigator
    {
        public static Navigator Instance { get; } = new Navigator();

        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public ReactivePropertySlim<Page> Page { get; }
        public ReadOnlyReactivePropertySlim<IPageViewModel> PageViewModel { get; private set; } = null;
        public ReactivePropertySlim<bool> CanClose { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private Navigator()
        {
            Page = new ReactivePropertySlim<Page>(Setup.Page.Welcome).AddTo(Disposables);
            PageViewModel = Observable.Return<IPageViewModel>(new PreparingPageViewModel()).ToReadOnlyReactivePropertySlim();
            CanClose = new ReactivePropertySlim<bool>(true).AddTo(Disposables);
        }

        public void Start()
        {
            PageViewModel = Page.Select(x => x.Convert()).ToReadOnlyReactivePropertySlim().AddTo(Disposables);
        }

        public void Abort(string detail)
        {
            PageViewModel = Observable.Return<IPageViewModel>(new AbortedPageViewModel()).ToReadOnlyReactivePropertySlim();
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
