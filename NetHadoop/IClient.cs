using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NetHadoop
{
    interface IClient
    {
        //列表目录
        List<FileInfoModel> GetFlolderList(string path);

        //获取文件信息
        FileInfoModel GetFileStatus(string path);

        bool MakeDir(string path);

        bool ReName(string oldPath, string newPath);

        void MutDownLoad(BackgroundWorker worker, string localRootPath, List<FileInfoModel> fileList, int fileType);

        void MutUpload(BackgroundWorker worker, List<string> localPath, string remoteRootPath, string localRootPath);

        bool Delete(string path, bool recursive);

        List<string> MoveFile(string[] sourcePath, string dectPath);

    }
}
