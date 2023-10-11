using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AtsEx.Setup.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            if (!Models.Navigator.Instance.CanClose.Value)
            {
                MessageBox.Show("アプリケーションやレジストリが破損する恐れがあるため、インストールを中断しないでください。", "AtsEX セットアップウィザード", MessageBoxButton.OK, MessageBoxImage.Error);
                e.Cancel = true;
            }
        }
    }
}
