using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

using Vanara.PInvoke;

namespace AtsEx.Setup.ViewModels
{
    internal static class StockIcons
    {
        public static BitmapSource GetIcon(Shell32.SHSTOCKICONID iconId, Shell32.SHGSI flags)
        {
            Shell32.SHSTOCKICONINFO iconInfo = new Shell32.SHSTOCKICONINFO()
            {
                cbSize = (uint)Marshal.SizeOf<Shell32.SHSTOCKICONINFO>(),
            };

            HRESULT result = Shell32.SHGetStockIconInfo(iconId, flags, ref iconInfo);
            if (result != HRESULT.S_OK)
            {
                Exception exception = result.GetException();
                ExceptionDispatchInfo.Capture(exception).Throw();
            }

            return Imaging.CreateBitmapSourceFromHIcon((IntPtr)iconInfo.hIcon, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public static BitmapSource GetShieldIcon()
            => GetIcon(Shell32.SHSTOCKICONID.SIID_SHIELD, Shell32.SHGSI.SHGSI_ICON | Shell32.SHGSI.SHGSI_SMALLICON);
    }
}
