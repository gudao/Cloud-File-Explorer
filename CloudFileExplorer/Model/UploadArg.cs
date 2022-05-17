using System;
using System.Collections.Generic;
using System.Text;

namespace CloudFileExplorer.Model
{
    public class UploadArg
    {
        public UploadArg()
        {
            FilePathList = new List<string>();
        }
        public List<string> FilePathList { get; set; }
        public string LocalFileRoot { get; set; }
    }
}
