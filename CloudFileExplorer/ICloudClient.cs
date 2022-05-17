using CloudFileExplorer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CloudFileExplorer
{
   public interface ICloudClient
    {
        string BucketName { get; set; }

        bool Delete(string path, bool recursive);
        bool Exist(string path);
        List<string> GetAllBucket();
        FileInfoModel GetFileStatus(string path);
        List<FileInfoModel> GetFlolderList(BackgroundWorker worker, string path);
        bool MakeDir(string path);
        List<string> MoveFile(string[] sourcePath, string dectPath);
        void MutDownLoad(BackgroundWorker worker, string localRootPath, List<FileInfoModel> fileList, int fileType);
        void MutUpload(BackgroundWorker worker, List<string> localPath, string remoteRootPath, string localRootPath);
        bool ReName(string oldPath, string newPath);
        Tuple<int, long> StatisticSize(string path);
    }
}
