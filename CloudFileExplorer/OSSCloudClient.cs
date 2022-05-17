using Aliyun.OSS;
using CloudFileExplorer.Model;
using OnceMi.AspNetCore.OSS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace CloudFileExplorer
{
    public class OSSCloudClient : ICloudClient
    {
        private IOSSService _service;
        private log4net.ILog logger = log4net.LogManager.GetLogger("LG");
        public OSSCloudClient()
        {
            _service =  new DefaultServiceFactory().Create();
        }
        public string BucketName { get; set; }

        public bool Delete(string path, bool recursive)
        {
            return _service.RemoveObjectAsync(BucketName, path).Result;
        }

        public bool Exist(string path)
        {
            return _service.ObjectsExistsAsync(BucketName, path).Result;
        }

        public List<string> GetAllBucket()
        {
            var l = new List<string>();
            try
            {
                var buckets = _service.ListBucketsAsync().Result;

                foreach (var bucket in buckets)
                {
                    l.Add(bucket.Name);
                }
            }
            catch (Exception)
            {
            }
            return l;
        }

        public FileInfoModel GetFileStatus(string path)
        {
            try
            {

                var ossObject = _service.GetObjectMetadataAsync(BucketName, path).Result;
                if (ossObject != null)
                {
                    return ObjToModel(ossObject);
                }
            }
            catch (Exception ee)
            {

                logger.Error("GetFileStatus Error:" + ee);
            }

            return null;
        }
        private FileInfoModel ObjToModel(ItemMeta obj)
        {
            return new FileInfoModel()
            {
                FileName = Path.GetFileName(obj.ObjectName),
                Path = obj.ObjectName,
                Isdir = obj.ObjectName.EndsWith("/"),
                Modification_time = obj.LastModified.ToFileTimeUtc(),
                Size = obj.Size
            };
        }
        private FileInfoModel ObjToModel(Item obj)
        {
            return new FileInfoModel()
            {
                //如果是文件夹区分
                FileName = Path.GetFileName(obj.Key.TrimEnd('/')),
                Path = obj.Key,
                Isdir = obj.IsDir,
                Modification_time = obj.LastModifiedDateTime.HasValue?obj.LastModifiedDateTime.Value.ToFileTimeUtc():0,
                Size = (long)obj.Size
            };
        }
        public List<FileInfoModel> GetFlolderList(BackgroundWorker worker, string path)
        {
            worker.ReportProgress(0, new ProgressState() { CurrentTitle = "开始读取文件列表" });
            try
            {
                List<FileInfoModel> list = new List<FileInfoModel>();
              var result=   _service.ListObjectsAsync(BucketName, path.Substring(1, path.Length - 1)).Result;
                foreach (var item in result)
                {
                    if (path.EndsWith(item.Key))
                        continue;
                    list.Add(ObjToModel(item));
                }
                return list;
            }
            catch (Exception ee)
            {
                logger.Error("GetFlolderList Error:" + ee);
                return new List<FileInfoModel>();
            }
        }

        public bool MakeDir(string path)
        {
            try
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    _service.PutObjectAsync(BucketName, path.TrimStart('/'), memStream).Wait();
                }
                return true;
            }
            catch (Exception ee)
            {
                logger.Error("MakeDir Error:" + ee);
                return false;
            }
        }

        public List<string> MoveFile(string[] sourcePath, string dectPath)
        {
            List<string> list = new List<string>();
            foreach (string item in sourcePath)
            {
                string fileName = Path.GetFileName(item);
                try
                {
                    _service.CopyObjectAsync(BucketName, item, BucketName, dectPath + fileName).Wait();
                    _service.RemoveObjectAsync(BucketName, item).Wait();
                   
                    list.Add(item);
                }
                catch (Exception ee)
                {
                    logger.Error("MoveFile Error:" + ee);
                }
            }
            return list;
        }

        public void MutDownLoad(BackgroundWorker worker, string localRootPath, List<FileInfoModel> fileList, int fileType)
        {
            //相同操作
            bool sameOp = false;
            //是否覆盖
            bool IsOver = false;

            int totalCount = fileList.Count;
            int currentCount = 0;

            foreach (FileInfoModel myfile in fileList)
            {
                currentCount++;
                int pgPresent = (int)((double)currentCount / totalCount * 100);

                string fileName = Path.GetFileName(myfile.Path);
                if (myfile.Isdir == false)
                {

                    string savePath = localRootPath + "/" + fileName;
                    if (fileType == 1 && myfile.FileName != null)
                    {
                        savePath = localRootPath + "/" + myfile.FileName;
                    }
                    if (fileType == 2 && myfile.FileName != null)
                    {
                        savePath = localRootPath + "/" + myfile.FileName + "/" + fileName;
                    }
                    //显示总进度
                    worker.ReportProgress(pgPresent, new ProgressState() { CurrentTitle = fileName, CurrentCount = currentCount, totalCount = totalCount });

                    #region 是否存在
                    if (!sameOp)
                    {
                        bool exsitFile = File.Exists(savePath);
                        if (exsitFile)
                        {
                            SureDialog myDialog = new SureDialog();
                            MyShowDialogResult myDR = new MyShowDialogResult();
                            myDialog.ShowDialog(fileName, myDR);
                            sameOp = myDR.IsCheck;
                            IsOver = myDR.Result;
                            if (!myDR.Result)
                            {
                                worker.ReportProgress(pgPresent, new ProgressState() { ListBoxMsg = fileName + " 跳过 " });
                                continue;
                            }
                        }
                    }
                    else
                    {
                        bool exsitFile = File.Exists(savePath);
                        if (exsitFile)
                        {
                            if (!IsOver)
                            {
                                worker.ReportProgress(pgPresent, new ProgressState() { ListBoxMsg = fileName + "跳过 " });
                                continue;
                            }
                        }
                    }
                    #endregion

                    #region 单个下载
                    bool result = false;

                    var ossObj = _service.GetObjectAsync(BucketName, myfile.Path, savePath);

                    #endregion

                    string msg = string.Format("{0} 下载{1}", Path.GetFileName(fileName), result ? "成功" : "失败");
                    worker.ReportProgress(pgPresent, new ProgressState() { ListBoxMsg = msg });
                }
                else
                {
                    worker.ReportProgress(pgPresent, new ProgressState() { ListBoxMsg = fileName + "不是文件！" });
                }
            }
        }

        public void MutUpload(BackgroundWorker worker, List<string> localPath, string remoteRootPath, string localRootPath)
        {
            //相同操作
            bool sameOp = false;
            //是否覆盖
            bool IsOver = false;
            bool IsJumpSameFolder = false;
            string lastFileFolder = "";


            List<string> NoSuccessList = new List<string>();
            List<string> skipList = new List<string>();

            int totalCount = localPath.Count;
            int currentCount = 0;


            foreach (string localFilePath in localPath)
            {
                System.Threading.Thread.Sleep(5);
                string fileName = Path.GetFileName(localFilePath);
                if (!string.IsNullOrEmpty(localRootPath))//如果是文件夹，则包含原有路径
                {
                    fileName = Path.GetFileName(localRootPath) + "/" + localFilePath.Replace(localRootPath + "\\", "");

                }
                fileName = fileName.Replace("\\", "/");
                string remoteFilePath = remoteRootPath.TrimStart('/') + fileName;
                currentCount++;
                int pgPresent = (int)((double)currentCount / totalCount * 100);
                //显示总进度
                worker.ReportProgress(pgPresent, new ProgressState() { CurrentTitle = fileName, CurrentCount = currentCount, totalCount = totalCount });

                #region 是否存在
                if (!sameOp)
                {
                    bool exsitFile = _service.ObjectsExistsAsync(BucketName, remoteFilePath).Result;
                    if (exsitFile)
                    {
                        SureDialog myDialog = new SureDialog();
                        MyShowDialogResult myDR = new MyShowDialogResult();
                        myDialog.ShowDialog(fileName, myDR);
                        sameOp = myDR.IsCheck;
                        IsOver = myDR.Result;
                        IsJumpSameFolder = myDR.IsJumpSameFolder;
                        lastFileFolder = Path.GetDirectoryName(fileName);
                        if (!myDR.Result)
                        {
                            worker.ReportProgress(pgPresent, new ProgressState() { ListBoxMsg = fileName + " 跳过 " });
                            skipList.Add(fileName);
                            continue;
                        }
                    }
                }
                else
                {
                    //跳过
                    if (IsJumpSameFolder)
                    {
                        var folderName = Path.GetDirectoryName(fileName);
                        if (folderName == lastFileFolder)
                        {
                            worker.ReportProgress(pgPresent, new ProgressState() { CurrentTitle = fileName + "同目录：" + folderName });
                            continue;
                        }
                    }
                    bool exsitFile = _service.ObjectsExistsAsync(BucketName, remoteFilePath).Result;
                    if (exsitFile)
                    {
                        if (!IsOver)
                        {
                            lastFileFolder = Path.GetDirectoryName(fileName);
                            worker.ReportProgress(pgPresent, new ProgressState() { CurrentTitle = fileName + "跳过 " });
                            skipList.Add(fileName);
                            continue;
                        }
                    }
                }
                #endregion


                #region 上传单个文件
                bool singleResult = false;

                try
                {
                    _service.PutObjectAsync(BucketName, remoteFilePath, localFilePath).Wait();
                    singleResult = true;
                }
                catch (Exception ee)
                {
                    singleResult = false;
                    logger.Error("upload Error:" + ee);
                }

                #endregion
                //显示单个上传结果
                string msg = string.Format("{0} 上传{1}", fileName, singleResult ? "成功" : "失败");
                worker.ReportProgress(pgPresent, new ProgressState() { ListBoxMsg = msg });
                if (!singleResult)
                {
                    NoSuccessList.Add(fileName);
                }
            }
        }

        public bool ReName(string oldPath, string newPath)
        {
            bool moveResult = _service.CopyObjectAsync(BucketName, oldPath, BucketName, newPath).Result;
            bool deleteResult = _service.RemoveObjectAsync(BucketName, oldPath).Result;
            return moveResult && deleteResult;
        }

        public Tuple<int, long> StatisticSize(string path)
        {
            int fileCount = 0;

            ulong sumSize = 0;

            try
            {
                var fileList = _service.ListObjectsAsync(BucketName, path).Result;

                foreach (var item in fileList)
                {
                    if (!item.IsDir)
                    {
                        fileCount++;
                        sumSize += item.Size;
                    }
                    else
                    {
                        var theSum = StatisticSize(item.Key);
                        fileCount += theSum.Item1;
                        sumSize += (ulong)theSum.Item2;
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("List object failed. {0}", ex.Message);
            }
            return new Tuple<int, long>(fileCount, (long)sumSize);
        }
    }
}
