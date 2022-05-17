using System;
using System.Collections.Generic;
using System.Text;

namespace CloudFileExplorer.Model
{
    public class FileInfoModel
    {
        //路径
        public string Path { get; set; }

        //文件大小
        public long Size { get; set; }

        //是否为文件夹
        public bool Isdir { get; set; }

        //修改时间
        public long Modification_time { get; set; }

        //文件名
        public string FileName { get; set; }
    }
}
