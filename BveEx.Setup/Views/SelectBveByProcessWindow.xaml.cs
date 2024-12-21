using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using BveEx.Setup.ViewModels;

namespace BveEx.Setup.Views
{
    /// <summary>
    /// SelectBveByProcessWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SelectBveByProcessWindow : Window, IDisposable
    {
        private readonly SelectBveByProcessWindowViewModel ViewModel;

        public string Path => ViewModel.SelectedProcess?.Path;

        internal SelectBveByProcessWindow(BveFileValidatorBase validator)
        {
            InitializeComponent();

            ViewModel = new SelectBveByProcessWindowViewModel(validator, new DialogService(this));
            DataContext = ViewModel;
        }

        public void Dispose()
        {
            ViewModel.Dispose();
        }


        private class DialogService : SelectBveByProcessWindowViewModel.IDialogService
        {
            private readonly Window Window;

            public Cursor Cursor
            {
                get => Window.Cursor;
                set => Window.Cursor = value;
            }

            public DialogService(Window window)
            {
                Window = window;
            }

            public void Close(bool? dialogResult)
            {
                Window.DialogResult = dialogResult;
                Window.Close();
            }
        }
    }
}
