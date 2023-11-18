using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AtsEx.Setup.Models
{
    internal class SelectBve5PageModel : SelectBvePageModelBase
    {
        protected override BveFileValidatorBase Validator { get; } = new FileValidator();

        public SelectBve5PageModel() : base(5)
        {
        }

        public override void GoBack()
        {
            Navigator.Instance.Page.Value = Page.SelectBve6;
        }

        public override void GoNext(string path)
        {
            if (path is null)
            {
                if (TargetPath.Bve6Path is null)
                {
                    MessageBox.Show("BVE Trainsim 6 / 5 両方に対して AtsEX のインストールを [スキップ] することはできません。", "AtsEX セットアップウィザード", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (MessageBox.Show("[スキップ] を選択した場合、BVE Trainsim 5 で AtsEX の機能を使用することはできません。\n本当に BVE Trainsim 5 への AtsEX のインストールをスキップしてもよろしいですか?", "AtsEX セットアップウィザード", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                {
                    return;
                }
            }

            TargetPath.Bve5Path = path;
            Navigator.Instance.Page.Value = Page.SelectScenarioDirectory;
        }


        private class FileValidator : BveFileValidatorBase
        {
            protected override BveFileError Validate(AssemblyName assemblyName)
            {
                return assemblyName.Version.Major != 5 ? BveFileError.DifferentVersion
                    : assemblyName.Version.Minor < 8 ? BveFileError.TooOld
                    : BveFileError.None;
            }
        }
    }
}
