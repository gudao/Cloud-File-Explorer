using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace CloudFileExplorer.Helper
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }
    public static class SystemFileHelper
    {
        [DllImport("Shell32.dll", EntryPoint = "SHGetFileInfo", SetLastError = true,
        CharSet = CharSet.Auto)]
         static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes,
            ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);

        [DllImport("User32.dll", EntryPoint = "DestroyIcon")]
        public static extern int DestroyIcon(IntPtr hIcon);

        #region API 参数的常量定义

        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; //大图标 32×32
        public const uint SHGFI_SMALLICON = 0x1; //小图标 16×16
        public const uint SHGFI_USEFILEATTRIBUTES = 0x10;

        #endregion
       public static Icon GetFileIcon(string fileName, bool isLargeIcon)
        {
           
            SHFILEINFO shfi = new SHFILEINFO();
            IntPtr hI;
            if (fileName == "文件夹")
            {
                fileName = "C:\\Windows\\";
                SHGetFileInfo(fileName, (uint)0x80, ref shfi,
                     (uint)Marshal.SizeOf(shfi),//Marshal.SizeOf返回对象的非托管大小
                        (uint)(0x100 | 0x400 | SHGFI_SMALLICON));//取得icon和typename
            }
            else
            {
                if (isLargeIcon)
                    hI = SHGetFileInfo(fileName, 0, ref shfi,
                         (uint)Marshal.SizeOf(shfi),
                         SHGFI_ICON | SHGFI_USEFILEATTRIBUTES |
                         SHGFI_LARGEICON);
                else
                    hI = SHGetFileInfo(fileName, 0, ref shfi,
                         (uint)Marshal.SizeOf(shfi),
                         SHGFI_ICON | SHGFI_USEFILEATTRIBUTES |
                         SHGFI_SMALLICON);
            }
            Icon icon = Icon.FromHandle(shfi.hIcon).Clone() as Icon;

            DestroyIcon(shfi.hIcon); //释放资源
            return icon;
        }
        

    }
}
