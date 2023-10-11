using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AtsEx.Setup.Models;

namespace AtsEx.Setup.ViewModels
{
    public class SelectBve6PageViewModel : SelectBvePageViewModelBase
    {
        private protected override BveFileErrorConverter Converter { get; } = new SelectBve6ErrorConverter();

        public SelectBve6PageViewModel() : base(new SelectBve6PageModel(), 6)
        {
        }


        private class SelectBve6ErrorConverter : BveFileErrorConverter
        {
            public override string Convert(BveFileError error)
            {
                switch (error)
                {
                    case BveFileError.DifferentVersion:
                        return "アプリケーションのバージョンが 6 ではありません。BVE Trainsim 5 の実行ファイルを選択している可能性があります。";
                    case BveFileError.TooOld:
                        throw new InvalidOperationException();
                    default:
                        return base.Convert(error);
                }
            }
        }
    }
}
