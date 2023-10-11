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

namespace AtsEx.Setup.ViewModels
{
    internal class PreparingPageViewModel : IPageViewModel, INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public string Caption { get; } = "準備中";
        public string Description { get; } = "少々お待ちください...";

        public event PropertyChangedEventHandler PropertyChanged;

        public PreparingPageViewModel()
        {
        }

        public void Dispose()
        {
            Disposables.Dispose();
        }
    }
}
