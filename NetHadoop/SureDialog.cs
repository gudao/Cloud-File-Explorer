using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NetHadoop
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
        
    }
    public class MyShowDialogResult 
    {
        public bool Result { get; set; }
        public bool IsCheck { get; set; }
    }
}
