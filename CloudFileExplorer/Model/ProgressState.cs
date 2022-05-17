using System;
using System.Collections.Generic;
using System.Text;

namespace CloudFileExplorer.Model
{
    public class ProgressState
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string ListBoxMsg { get; set; }
        /// <summary>
        /// 当前文件名
        /// </summary>
        public string CurrentTitle { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int totalCount { get; set; }
        /// <summary>
        /// 当前数
        /// </summary>
        public int CurrentCount { get; set; }
    }
}
