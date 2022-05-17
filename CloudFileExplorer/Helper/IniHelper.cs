using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CloudFileExplorer.Helper
{
    public class IniHelper
    {
        public string inipath;

        //声明API函数

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        /// <summary> 
        /// 构造方法 
        /// </summary> 
        /// <param name="filePath">文件路径</param> 
        public IniHelper(string filePath)
        {
            if (File.Exists(inipath))
            {
                inipath = filePath;
            }
        }

        public IniHelper()
        {
            
#if DEBUG
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setting.dev.ini");//在当前程序路径创建
#else
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setting.ini");//在当前程序路径创建
#endif

            if (File.Exists(filePath))
            {
                inipath = filePath;
            }
            else
            {
                File.Create(filePath);//创建INI文件
            }
        }


        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.inipath);
        }
        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public string IniReadValue(string Section, string Key,string defVal="")
        {
            StringBuilder temp = new StringBuilder();
            GetPrivateProfileString(Section, Key, defVal, temp, 500, this.inipath);
            return temp.ToString();
        }
        
    }
}
