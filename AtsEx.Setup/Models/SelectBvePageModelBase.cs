using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Win32;
using Reactive.Bindings;
using Reactive.Bindings.Disposables;
using Reactive.Bindings.Extensions;

using AtsEx.Setup.Views;

namespace AtsEx.Setup.Models
{
    internal abstract class SelectBvePageModelBase : INotifyPropertyChanged, IDisposable
    {
        private readonly CompositeDisposable Disposables = new CompositeDisposable();
        private readonly int BveVersion;

        protected abstract BveFileValidatorBase Validator { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public SelectBvePageModelBase(int bveVersion)
        {
            BveVersion = bveVersion;
        }

        public BveFileInfo OpenFileByProcess()
        {
            using (SelectBveByProcessWindow dialog = new SelectBveByProcessWindow(Validator))
            {
                return dialog.ShowDialog() == true ? CreateFileInfo(dialog.Path) : null;
            }
        }

        public BveFileInfo OpenFile(string directory)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "アプリケーション (*.exe)|*.exe|全てのファイル (*.*)|*.*",
                InitialDirectory = directory,
                Title = $"AtsEX を適用する BVE Trainsim {BveVersion} を選択してください",
            };

            return dialog.ShowDialog() == true ? CreateFileInfo(dialog.FileName) : null;
        }

        public BveFileInfo CreateFileInfo(string path)
        {
            BveFileError error = Validator.Validate(path);
            return new BveFileInfo(path, error);
        }

        public abstract void GoBack();
        public abstract void GoNext(string path);

        public void Dispose()
        {
            Disposables.Dispose();
        }


        internal class BveFileInfo
        {
            public string Path { get; }
            public BveFileError Error { get; }

            public BveFileInfo(string path, BveFileError error)
            {
                Path = path;
                Error = error;
            }
        }
    }
}
