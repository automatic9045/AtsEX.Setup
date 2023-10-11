using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

using AtsEx.Setup.Releases;

namespace AtsEx.Setup.Models
{
    internal class SelectBveByProcessWindowModel : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();

        public event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            Disposables.Dispose();
        }

        public IReadOnlyList<ProcessInfo> GetProcesses(BveFileValidatorBase validator)
        {
            ProcessEnumerator processEnumerator = new ProcessEnumerator();
            return processEnumerator.GetProcesses()
                .Where(processInfo =>
                {
                    BveFileError error = validator.Validate(processInfo.Path);
                    switch (error)
                    {
                        case BveFileError.None:
                        case BveFileError.MayNotBve:
                        case BveFileError.TooOld:
                            return true;

                        default:
                            return false;
                    }
                }).ToList();
        }
    }
}
