using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AtsEx.Setup.Models
{
    internal class SelectBve6PageModel : SelectBvePageModelBase
    {
        protected override BveFileValidatorBase Validator { get; } = new FileValidator();

        public SelectBve6PageModel() : base(6)
        {
        }

        public override void GoBack()
        {
            Navigator.Instance.Page.Value = Page.Welcome;
        }

        public override void GoNext(string path)
        {
            if (path is null)
            {
                if (MessageBox.Show("[スキップ] を選択した場合、BVE Trainsim 6 で AtsEX の機能を使用することはできません。\n本当に BVE Trainsim 6 への AtsEX のインストールをスキップしてもよろしいですか?", "AtsEX セットアップウィザード", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                {
                    return;
                }
            }
            else
            {
                TargetPath.Bve6Path.Value = new InstallationTarget(path);
            }

            Navigator.Instance.Page.Value = Page.SelectBve5;
        }


        private class FileValidator : BveFileValidatorBase
        {
            protected override BveFileError Validate(AssemblyName assemblyName)
            {
                return assemblyName.Version.Major != 6 ? BveFileError.DifferentVersion : BveFileError.None;
            }
        }
    }
}
