using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BveEx.Setup.Models
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
                if (TargetPath.Bve6Path.Value.HasInstalled)
                {
                    MessageBox.Show("BVE Trainsim 6 / 5 両方に対して BveEX のインストールを [スキップ] することはできません。", "BveEX セットアップウィザード", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (MessageBox.Show("[スキップ] を選択した場合、BVE Trainsim 5 で BveEX の機能を使用することはできません。\n本当に BVE Trainsim 5 への BveEX のインストールをスキップしてもよろしいですか?", "BveEX セットアップウィザード", MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                {
                    return;
                }
            }
            else
            {
                TargetPath.Bve5Path.Value = new InstallationTarget(path);
            }

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
