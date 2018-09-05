using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO;
namespace NetHadoop
{
   public class AliYunClient
    {

        private log4net.ILog logger = log4net.LogManager.GetLogger("LG");
        private OssClient client = new OssClient(ConfigHelper.Endpoint, ConfigHelper.AccessKeyId, ConfigHelper.AccessKeySecret);

        public string BucketName { get; set; }



        public List<string> GetAllBucket()
        {
            var l = new List<string>();
            try
            {
                var buckets = client.ListBuckets();

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


        public bool Delete(string path, bool recursive)
        {
            try
            {
                client.DeleteObject(BucketName, path);
                return true;
            }
            catch (Exception ee)
            {
                logger.Error("Delete Error:" + ee);
                return false;
            }
        }

        public FileInfoModel GetFileStatus(string path)
        {
            try
            {
                
                var ossObject = client.GetObject(BucketName, path);
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

        public bool Exist(string path)
        {
            try
            {
                return client.DoesObjectExist(BucketName, path.TrimStart('/'));
            }
            catch (Exception ee)
            {

                logger.Error("DoesObjectExist Error:" + ee);
            }

            return false;
        }

        public List<FileInfoModel> GetFlolderList(BackgroundWorker worker, string path)
        {
            worker.ReportProgress(0, new ProgressState() { CurrentTitle = "开始读取文件列表" });
            try
            {
                List<FileInfoModel> list = new List<FileInfoModel>();
                ObjectListing result = null;
                string nextMarker = string.Empty;
                do
                {
                    var listObjectsRequest = new ListObjectsRequest(BucketName)
                    {
                        Marker = nextMarker,
                        MaxKeys = 100,
                        Prefix=path.Substring(1,path.Length-1),
                         Delimiter="/"
                    };
                    result = client.ListObjects(listObjectsRequest);

                    foreach (var summary in result.ObjectSummaries)
                    {
                        if(summary.Key!=path.TrimStart('/'))
                        {
                            list.Add(SumToModel(summary));
                        }
                    }

                    foreach (var prefixes in result.CommonPrefixes)
                    {

                        list.Add(PathToModel(prefixes));
                    }
                    nextMarker = result.NextMarker;
                    worker.ReportProgress(100, new ProgressState() { CurrentTitle ="读取文件："+list.Count });
                } while (result.IsTruncated);

              
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
                    client.PutObject(BucketName, path.TrimStart('/'), memStream);
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
                var metadata = new ObjectMetadata();

                string fileName = Path.GetFileName(item);

                var req = new CopyObjectRequest(BucketName, item, BucketName, dectPath+fileName)
                {
                    NewObjectMetadata = metadata
                };
                try
                {
                    client.CopyObject(req);
                    client.DeleteObject(BucketName, item);
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

                    var ossObj = client.GetObject(BucketName, myfile.Path);

                    using (var requestStream = ossObj.Content)
                    {
                        long totalBytes = 0;
                        using (var fs = File.Open(savePath, FileMode.OpenOrCreate))
                        {
                            int length = 4 * 1024;
                            var buf = new byte[length];
                            do
                            {
                                length = requestStream.Read(buf, 0, length);
                                fs.Write(buf, 0, length);

                                totalBytes += length;
                                int mypresent = (int)((double)totalBytes / myfile.Size * 100);
                                worker.ReportProgress(pgPresent, new ProgressState() { CurrentTitle = mypresent + "% " + fileName });

                            } while (length != 0);

                            result = true;
                        }
                    }


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

            List<string> NoSuccessList = new List<string>();
            List<string> skipList = new List<string>();

            int totalCount = localPath.Count;
            int currentCount = 0;

            foreach (string localFilePath in localPath)
            {
                string fileName = Path.GetFileName(localFilePath);
                if (!string.IsNullOrEmpty(localRootPath))//如果是文件夹，则包含原有路径
                    fileName = Path.GetFileName(localRootPath)+"/"+fileName;

                string remoteFilePath =  remoteRootPath.TrimStart('/') + fileName;
                currentCount++;
                int pgPresent = (int)((double)currentCount / totalCount * 100);
                //显示总进度
                worker.ReportProgress(pgPresent, new ProgressState() { CurrentTitle = fileName, CurrentCount = currentCount,totalCount=totalCount });

                #region 是否存在
                if (!sameOp)
                {
                    bool exsitFile = client.DoesObjectExist(BucketName, remoteFilePath);
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
                            skipList.Add(fileName);
                            continue;
                        }
                    }
                }
                else
                {
                    bool exsitFile = client.DoesObjectExist(BucketName, remoteFilePath);
                    if (exsitFile)
                    {
                        if (!IsOver)
                        {
                            worker.ReportProgress(pgPresent, new ProgressState() { ListBoxMsg = fileName + "跳过 " });
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
                   client.PutObject(BucketName, remoteFilePath, localFilePath);
                    singleResult = true ;
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
            throw new NotImplementedException();
        }


        private FileInfoModel ObjToModel(OssObject obj)
        {
            return new FileInfoModel()
            {
                FileName = Path.GetFileName(obj.Key),
                Path = obj.Key,
                Isdir = obj.ContentLength == 0&&obj.Key.EndsWith("/"),
                Modification_time = obj.Metadata.LastModified.ToFileTimeUtc(),
                Size = obj.ContentLength
            };
        }

        private FileInfoModel SumToModel(OssObjectSummary obj)
        {
            return new FileInfoModel()
            {
                FileName = Path.GetFileName(obj.Key),
                Path = obj.Key,
                Isdir = obj.Size == 0 && obj.Key.EndsWith("/"),
                Modification_time = obj.LastModified.ToFileTimeUtc(),
                Size = obj.Size
            };
        }

        private FileInfoModel PathToModel(string obj)
        {
            return new FileInfoModel()
            {
                FileName = Path.GetFileName(obj.TrimEnd('/')),
                Path = obj,
                Isdir = true,
                Modification_time = 0,
                Size = 0
            };
        }


    }
}
