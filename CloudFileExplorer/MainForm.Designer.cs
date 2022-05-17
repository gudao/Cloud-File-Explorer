﻿namespace CloudFileExplorer
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.mystatusbar = new System.Windows.Forms.StatusStrip();
            this.lbFileInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbProgressTxt = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.lbCurrentCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbChuHao = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbTotalCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.mytoolbar = new System.Windows.Forms.ToolStrip();
            this.btNewFlolder = new System.Windows.Forms.ToolStripButton();
            this.btUp = new System.Windows.Forms.ToolStripButton();
            this.tbBucketName = new System.Windows.Forms.ToolStripComboBox();
            this.tbAddress = new System.Windows.Forms.ToolStripComboBox();
            this.tbSearch = new System.Windows.Forms.ToolStripTextBox();
            this.listMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemReName = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuListDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.menuUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemUploadFloder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemCut = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuOutFileList = new System.Windows.Forms.ToolStripMenuItem();
            this.StaticToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lvFiles = new System.Windows.Forms.ListViewNF();
            this.mystatusbar.SuspendLayout();
            this.mytoolbar.SuspendLayout();
            this.listMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // mystatusbar
            // 
            this.mystatusbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbFileInfo,
            this.lbProgressTxt,
            this.lbProgressBar,
            this.lbCurrentCount,
            this.lbChuHao,
            this.lbTotalCount});
            this.mystatusbar.Location = new System.Drawing.Point(0, 577);
            this.mystatusbar.Name = "mystatusbar";
            this.mystatusbar.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.mystatusbar.Size = new System.Drawing.Size(898, 29);
            this.mystatusbar.TabIndex = 0;
            this.mystatusbar.Text = "statusStrip1";
            // 
            // lbFileInfo
            // 
            this.lbFileInfo.Name = "lbFileInfo";
            this.lbFileInfo.Size = new System.Drawing.Size(61, 24);
            this.lbFileInfo.Text = "lbFileInfo";
            // 
            // lbProgressTxt
            // 
            this.lbProgressTxt.Name = "lbProgressTxt";
            this.lbProgressTxt.Size = new System.Drawing.Size(658, 24);
            this.lbProgressTxt.Spring = true;
            this.lbProgressTxt.Text = "lbProgressTxt";
            // 
            // lbProgressBar
            // 
            this.lbProgressBar.Name = "lbProgressBar";
            this.lbProgressBar.Size = new System.Drawing.Size(117, 23);
            // 
            // lbCurrentCount
            // 
            this.lbCurrentCount.Name = "lbCurrentCount";
            this.lbCurrentCount.Size = new System.Drawing.Size(15, 24);
            this.lbCurrentCount.Text = "0";
            // 
            // lbChuHao
            // 
            this.lbChuHao.Name = "lbChuHao";
            this.lbChuHao.Size = new System.Drawing.Size(13, 24);
            this.lbChuHao.Text = "/";
            // 
            // lbTotalCount
            // 
            this.lbTotalCount.Name = "lbTotalCount";
            this.lbTotalCount.Size = new System.Drawing.Size(15, 24);
            this.lbTotalCount.Text = "0";
            // 
            // mytoolbar
            // 
            this.mytoolbar.CanOverflow = false;
            this.mytoolbar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.mytoolbar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btNewFlolder,
            this.btUp,
            this.tbBucketName,
            this.tbAddress,
            this.tbSearch});
            this.mytoolbar.Location = new System.Drawing.Point(0, 0);
            this.mytoolbar.Name = "mytoolbar";
            this.mytoolbar.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.mytoolbar.Size = new System.Drawing.Size(898, 31);
            this.mytoolbar.TabIndex = 1;
            this.mytoolbar.Text = "toolStrip1";
            // 
            // btNewFlolder
            // 
            this.btNewFlolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btNewFlolder.Image = ((System.Drawing.Image)(resources.GetObject("btNewFlolder.Image")));
            this.btNewFlolder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btNewFlolder.Name = "btNewFlolder";
            this.btNewFlolder.Size = new System.Drawing.Size(23, 22);
            this.btNewFlolder.Text = "新建文件夹";
            this.btNewFlolder.Click += new System.EventHandler(this.menuNewFolder_Click);
            // 
            // btUp
            // 
            this.btUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btUp.Image = ((System.Drawing.Image)(resources.GetObject("btUp.Image")));
            this.btUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btUp.Name = "btUp";
            this.btUp.Size = new System.Drawing.Size(23, 22);
            this.btUp.Text = "上一级";
            this.btUp.Click += new System.EventHandler(this.btUp_Click);
            // 
            // tbBucketName
            // 
            this.tbBucketName.Name = "tbBucketName";
            this.tbBucketName.Size = new System.Drawing.Size(116, 25);
            this.tbBucketName.SelectedIndexChanged += new System.EventHandler(this.tbBucketName_SelectedIndexChanged);
            // 
            // tbAddress
            // 
            this.tbAddress.MaxDropDownItems = 15;
            this.tbAddress.Name = "tbAddress";
            this.tbAddress.Size = new System.Drawing.Size(140, 25);
            this.tbAddress.SelectedIndexChanged += new System.EventHandler(this.tbAddress_SelectedIndexChanged);
            this.tbAddress.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbAddress_KeyUp);
            // 
            // tbSearch
            // 
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(116, 25);
            this.tbSearch.Text = "搜索";
            this.tbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyUp);
            this.tbSearch.Click += new System.EventHandler(this.tbSearch_Click);
            // 
            // listMenu
            // 
            this.listMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNewFolder,
            this.MenuItemReName,
            this.menuRefresh,
            this.toolStripSeparator1,
            this.menuListDownload,
            this.menuDownload,
            this.menuUpload,
            this.MenuItemUploadFloder,
            this.toolStripSeparator2,
            this.MenuItemCut,
            this.MenuItemCopy,
            this.MenuItemPaste,
            this.menuDelete,
            this.toolStripSeparator3,
            this.menuOutFileList,
            this.StaticToolStripMenuItem});
            this.listMenu.Name = "listMenu";
            this.listMenu.Size = new System.Drawing.Size(180, 308);
            // 
            // menuNewFolder
            // 
            this.menuNewFolder.Name = "menuNewFolder";
            this.menuNewFolder.Size = new System.Drawing.Size(179, 22);
            this.menuNewFolder.Text = "新建文件夹";
            this.menuNewFolder.Click += new System.EventHandler(this.menuNewFolder_Click);
            // 
            // MenuItemReName
            // 
            this.MenuItemReName.Enabled = false;
            this.MenuItemReName.Name = "MenuItemReName";
            this.MenuItemReName.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.MenuItemReName.Size = new System.Drawing.Size(179, 22);
            this.MenuItemReName.Text = "重命名";
            this.MenuItemReName.Click += new System.EventHandler(this.MenuItemReName_Click);
            // 
            // menuRefresh
            // 
            this.menuRefresh.Name = "menuRefresh";
            this.menuRefresh.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.menuRefresh.Size = new System.Drawing.Size(179, 22);
            this.menuRefresh.Text = "刷新";
            this.menuRefresh.Click += new System.EventHandler(this.menuRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(176, 6);
            // 
            // menuListDownload
            // 
            this.menuListDownload.Name = "menuListDownload";
            this.menuListDownload.Size = new System.Drawing.Size(179, 22);
            this.menuListDownload.Text = "根据列表下载";
            this.menuListDownload.Click += new System.EventHandler(this.menuListDownload_Click);
            // 
            // menuDownload
            // 
            this.menuDownload.Name = "menuDownload";
            this.menuDownload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.menuDownload.Size = new System.Drawing.Size(179, 22);
            this.menuDownload.Text = "下载";
            this.menuDownload.Click += new System.EventHandler(this.menuDownload_Click);
            // 
            // menuUpload
            // 
            this.menuUpload.Name = "menuUpload";
            this.menuUpload.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.U)));
            this.menuUpload.Size = new System.Drawing.Size(179, 22);
            this.menuUpload.Text = "上传文件";
            this.menuUpload.Click += new System.EventHandler(this.menuUpload_Click);
            // 
            // MenuItemUploadFloder
            // 
            this.MenuItemUploadFloder.Name = "MenuItemUploadFloder";
            this.MenuItemUploadFloder.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.MenuItemUploadFloder.Size = new System.Drawing.Size(179, 22);
            this.MenuItemUploadFloder.Text = "上传文件夹";
            this.MenuItemUploadFloder.Click += new System.EventHandler(this.MenuItemUploadFloder_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(176, 6);
            // 
            // MenuItemCut
            // 
            this.MenuItemCut.Name = "MenuItemCut";
            this.MenuItemCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.MenuItemCut.Size = new System.Drawing.Size(179, 22);
            this.MenuItemCut.Text = "剪切";
            this.MenuItemCut.Click += new System.EventHandler(this.MenuItemCut_Click);
            // 
            // MenuItemCopy
            // 
            this.MenuItemCopy.Name = "MenuItemCopy";
            this.MenuItemCopy.Size = new System.Drawing.Size(179, 22);
            this.MenuItemCopy.Text = "复制";
            this.MenuItemCopy.Click += new System.EventHandler(this.MenuItemCopy_Click);
            // 
            // MenuItemPaste
            // 
            this.MenuItemPaste.Name = "MenuItemPaste";
            this.MenuItemPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.MenuItemPaste.Size = new System.Drawing.Size(179, 22);
            this.MenuItemPaste.Text = "粘贴";
            this.MenuItemPaste.Click += new System.EventHandler(this.MenuItemPaste_Click);
            // 
            // menuDelete
            // 
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Delete)));
            this.menuDelete.Size = new System.Drawing.Size(179, 22);
            this.menuDelete.Text = "删除";
            this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(176, 6);
            // 
            // menuOutFileList
            // 
            this.menuOutFileList.Name = "menuOutFileList";
            this.menuOutFileList.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menuOutFileList.Size = new System.Drawing.Size(179, 22);
            this.menuOutFileList.Text = "导出列表";
            this.menuOutFileList.Click += new System.EventHandler(this.menuOutFileList_Click);
            // 
            // StaticToolStripMenuItem
            // 
            this.StaticToolStripMenuItem.Name = "StaticToolStripMenuItem";
            this.StaticToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.StaticToolStripMenuItem.Text = "统计文件大小";
            this.StaticToolStripMenuItem.Click += new System.EventHandler(this.StaticToolStripMenuItem_Click);
            // 
            // lvFiles
            // 
            this.lvFiles.AllowDrop = true;
            this.lvFiles.ContextMenuStrip = this.listMenu;
            this.lvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFiles.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lvFiles.FullRowSelect = true;
            this.lvFiles.HideSelection = false;
            this.lvFiles.LabelEdit = true;
            this.lvFiles.Location = new System.Drawing.Point(0, 31);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(898, 546);
            this.lvFiles.TabIndex = 2;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.listView1_AfterLabelEdit);
            this.lvFiles.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvFiles_ItemDrag);
            this.lvFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvFiles_DragDrop);
            this.lvFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvFiles_DragEnter);
            this.lvFiles.KeyUp += new System.Windows.Forms.KeyEventHandler(this.lvFiles_KeyUp);
            this.lvFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvFiles_MouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 606);
            this.Controls.Add(this.lvFiles);
            this.Controls.Add(this.mytoolbar);
            this.Controls.Add(this.mystatusbar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cloud File Explorer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AliOSSForm_FormClosed);
            this.SizeChanged += new System.EventHandler(this.Form_SizeChanged);
            this.mystatusbar.ResumeLayout(false);
            this.mystatusbar.PerformLayout();
            this.mytoolbar.ResumeLayout(false);
            this.mytoolbar.PerformLayout();
            this.listMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip mystatusbar;
        private System.Windows.Forms.ToolStrip mytoolbar;
        private System.Windows.Forms.ListViewNF lvFiles;
        private System.Windows.Forms.ToolStripButton btUp;
        private System.Windows.Forms.ToolStripComboBox tbAddress;
        private System.Windows.Forms.ToolStripTextBox tbSearch;
        private System.Windows.Forms.ToolStripButton btNewFlolder;
        private System.Windows.Forms.ToolStripStatusLabel lbFileInfo;
        private System.Windows.Forms.ToolStripStatusLabel lbProgressTxt;
        private System.Windows.Forms.ToolStripProgressBar lbProgressBar;
        private System.Windows.Forms.ContextMenuStrip listMenu;
        private System.Windows.Forms.ToolStripMenuItem menuNewFolder;
        private System.Windows.Forms.ToolStripMenuItem MenuItemReName;
        private System.Windows.Forms.ToolStripMenuItem menuRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuDownload;
        private System.Windows.Forms.ToolStripMenuItem menuUpload;
        private System.Windows.Forms.ToolStripMenuItem MenuItemUploadFloder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem MenuItemCut;
        private System.Windows.Forms.ToolStripMenuItem MenuItemPaste;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripStatusLabel lbCurrentCount;
        private System.Windows.Forms.ToolStripStatusLabel lbChuHao;
        private System.Windows.Forms.ToolStripStatusLabel lbTotalCount;
        private System.Windows.Forms.ToolStripComboBox tbBucketName;
        private System.Windows.Forms.ToolStripMenuItem menuOutFileList;
        private System.Windows.Forms.ToolStripMenuItem menuListDownload;
        private System.Windows.Forms.ToolStripMenuItem MenuItemCopy;
        private System.Windows.Forms.ToolStripMenuItem StaticToolStripMenuItem;
        private System.Windows.Forms.ListViewNF listViewnf1;
    }
}

