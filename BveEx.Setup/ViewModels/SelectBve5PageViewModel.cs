using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BveEx.Setup.Models;

namespace BveEx.Setup.ViewModels
{
    public class SelectBve5PageViewModel : SelectBvePageViewModelBase
    {
        private protected override BveFileErrorConverter Converter { get; } = new SelectBve5ErrorConverter();

        public SelectBve5PageViewModel() : base(new SelectBve5PageModel(), 5, Data.InstallationSettings.Loaded.Bve5Path, GetInitialPath())
        {
        }

        private static string GetInitialPath()
        {
            string programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            string defaultPath = System.IO.Path.Combine(programFilesPath, @"mackoy\BveTs5\BveTs.exe");
            return File.Exists(defaultPath) ? defaultPath : null;
        }


        private class SelectBve5ErrorConverter : BveFileErrorConverter
        {
            public override string Convert(BveFileError error)
            {
                switch (error)
                {
                    case BveFileError.DifferentVersion:
                        return "アプリケーションのバージョンが 5 ではありません。BVE Trainsim 6 か、Bve Trainsim 5.6 以前の実行ファイルを選択している可能性があります。";
                    case BveFileError.TooOld:
                        return "アプリケーションのバージョンが 5.8 以降ではありません。BVE Trainsim 5.7 以前の実行ファイルを選択している可能性があります。";
                    default:
                        return base.Convert(error);
                }
            }
        }
    }
}
