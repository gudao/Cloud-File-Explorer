using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CloudFileExplorer
{
    public partial class SureDialog : Form
    {
        private MyShowDialogResult myresult = null;
        public SureDialog()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        public void ShowDialog(string fileName, MyShowDialogResult result)
        {
            label1.Text = "此位置已包含同名文件： " + fileName;

            myresult = result;
            base.ShowDialog();

        }
        //覆盖
        private void button1_Click(object sender, EventArgs e)
        {
            if (myresult != null)
            {
                myresult.IsCheck = checkBox1.Checked;
                myresult.Result = true;
            }
            this.Close();
        }
        //跳过
        private void button2_Click(object sender, EventArgs e)
        {
            if (myresult != null)
            {
                myresult.IsCheck = checkBox1.Checked;
                myresult.Result = false;
            }
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (myresult != null)
            {
                myresult.IsCheck = checkBox1.Checked;
                myresult.Result = false;
                myresult.IsJumpSameFolder = true;
            }
            this.Close();
        }
    }
    public class MyShowDialogResult 
    {
        /// <summary>
        /// 是否覆盖
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 是否相同操作
        /// </summary>
        public bool IsCheck { get; set; }

        /// <summary>
        /// 跳过并跳过同目录文件
        /// </summary>
        public bool IsJumpSameFolder { get; set; }
    }
}
