using System;
using System.Collections.Generic;
using System.Text;

namespace NetHadoop
{
    public class ProgressState
    {
        public string ListBoxMsg { get; set; }

        public string CurrentTitle { get; set; }

        public int totalCount { get; set; }
        public int CurrentCount { get; set; }
    }
}
