using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Windows.Storage.Streams;

namespace HelloSSH
{
    static class Util
    {
        public static void OpenURI(Uri uri)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = uri.AbsoluteUri,
                UseShellExecute = true
            });
        }
        // https://stackoverflow.com/questions/1127647/convert-system-drawing-icon-to-system-media-imagesource
        public static ImageSource ToImageSource(this Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        public static byte[] GetIBufferBytes(IBuffer buffer)
        {
            var ret = new byte[buffer.Length];
            var reader = DataReader.FromBuffer(buffer);
            reader.ReadBytes(ret);
            return ret;
        }

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("USER32.DLL")]
        public static extern bool IsWindow(IntPtr hWnd);
        [DllImport("USER32.DLL")]
        public static extern bool BringWindowToTop(IntPtr hWnd);
    }
}
