using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtsEx.Setup
{
    internal enum BveFileError
    {
        None,
        MayNotBve,
        NotSelected,
        InvalidFormat,
        VersionNotDefined,
        DifferentVersion,
        TooOld,
    }

    internal class BveFileErrorConverter
    {
        public virtual string Convert(BveFileError error)
        {
            switch (error)
            {
                case BveFileError.None:
                    return null;
                case BveFileError.MayNotBve:
                    return "ファイル名が \"BveTs.exe\" ではありません。選択されたアプリケーションは BVE ではない可能性があります。";
                case BveFileError.NotSelected:
                    return "ファイルが選択されていません。";
                case BveFileError.InvalidFormat:
                    return "アプリケーション情報の読込に失敗しました。選択されたファイルは BVE 本体ではありません。";
                case BveFileError.VersionNotDefined:
                    return "アプリケーションにバージョン情報が定義されていません。選択されたアプリケーションは BVE ではありません。";
                case BveFileError.DifferentVersion:
                    return "バージョンが異なります。";
                case BveFileError.TooOld:
                    return "バージョンが古すぎます。";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
