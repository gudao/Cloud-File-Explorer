using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Thrift.Transport;
using Thrift.Protocol;
using HDFS;
using System.IO;
using System.Windows.Forms;
namespace NetHadoop
{
   public class FSClient
    {
       private log4net.ILog logger = log4net.LogManager.GetLogger("LG");

       public string HostIP { get; private set; }
       public int HostPort { get; private set; }
       

       public FSClient(string host,int port)
       {
           HostIP = host;
           HostPort = port;
       }
       public FSClient()
       {
           HostIP = ConfigHelper.HdfsHost;
           HostPort = ConfigHelper.HdfsPort;
       }

       //获取连接
       public ThriftHadoopFileSystem.Client Connect(out TBufferedTransport tsport)
       {

           TSocket hadoop_socket = new TSocket(HostIP, HostPort);
           
           //hadoop_socket.Timeout = 10000;// Ten seconds

           tsport = new TBufferedTransport(hadoop_socket);
           
           TBinaryProtocol hadoop_protocol = new TBinaryProtocol(tsport, false, false);

           ThriftHadoopFileSystem.Client client = new ThriftHadoopFileSystem.Client(hadoop_protocol);
           try
           {
               tsport.Open();
               return client;
           }
           catch (Exception ee)
           {
               logger.Error("打开连接失败！", ee);
               tsport = null;
               return null;
           }
       }

       //列表目录
       public List<FileStatus> GetFlolderList(string path)
       {
           TBufferedTransport tsport = null;
           ThriftHadoopFileSystem.Client client = Connect(out tsport);
           List<FileStatus> result = null; //new List<FileStatus>();
           //客户端可连接 且目录存在
           if (client != null && client.exists(new Pathname { pathname = path }))
           {
               result = client.listStatus(new Pathname() { pathname = path });
               tsport.Close();
           }
           return result;
       }

       //文件状态
       public FileStatus GetFileStatus(string path)
       {
           TBufferedTransport tsport = null;
           ThriftHadoopFileSystem.Client client = Connect(out tsport);
           FileStatus result = null; //new List<FileStatus>();
           //客户端可连接 且目录存在
           if (client != null&& client.exists(new Pathname { pathname = path }))
           {
               result = client.stat(new Pathname() { pathname = path });
               tsport.Close();
           }
           return result;
       }



       //新建文件夹
       public bool MakeDir(string path)
       {
           TBufferedTransport tsport = null;
           ThriftHadoopFileSystem.Client client = Connect(out tsport);
           bool result = false;
           if (client != null)
           {
               Pathname pn = new Pathname() { pathname = path };
               if (!client.exists(pn))//如果不存在才执行
                   result = client.mkdirs(pn);
               tsport.Close();
           }
           return result;
       }

       //重命名文件或文件夹
       public bool ReName(string oldPath,string newPath)
       {
           TBufferedTransport tsport = null;
           ThriftHadoopFileSystem.Client client = Connect(out tsport);
           bool result = false;
           if (client != null)
           {
               Pathname pn=new Pathname() { pathname = oldPath };
               if (client.exists(pn))//如果存在才执行
                   result = client.rename(pn, new Pathname() { pathname = newPath });
              
               tsport.Close();
           }
           return result;
       }

       //下载
       public bool Open(ThriftHadoopFileSystem.Client client,string path,string savePath,long fileLength)
       {
           bool result = false;
           if (client != null)
           {
               ThriftHandle th = client.open(new Pathname() { pathname = path });
               
               // 创建文件流
               FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write);
               long totalBytes = 0;
               int readLength=1024*1024;
               try
               {
                   UTF8Encoding utf8 = new UTF8Encoding(false,true);

                   while (true)
                   {
                       int needRead = readLength;
                       if (fileLength - totalBytes < readLength)
                       {
                           needRead = (int)(fileLength - totalBytes);
                       }
                       if (needRead <= 0)
                           break;

                       byte[] fileBuffer = client.read(th, totalBytes, readLength);


                       byte[] myfileBuffer =  Encoding.Convert(utf8, Encoding.GetEncoding("iso-8859-1"), fileBuffer);
                       
                       
                       totalBytes += readLength;

                       fs.Write(myfileBuffer, 0, myfileBuffer.Length);
                       
                   }
                   result = true;
                   
                  
               }
               catch (Exception ee)
               {
                   throw ee;
               }
               finally
               {
                   fs.Dispose();
                   if (client != null)
                       client.close(th);
               }
           }
           return result;
       }
       //批量下载
       public void MutDownLoad(BackgroundWorker worker, string localRootPath, List<FileStatus> fileList,int fileType)
       {
           //相同操作
           bool sameOp = false;
           //是否覆盖
           bool IsOver = false;
            //准备创建连接
           Thrift.Transport.TBufferedTransport btsport = null;
           ThriftHadoopFileSystem.Client thriftClient = Connect(out btsport);

           if (thriftClient != null)//连接成功
           {
               int totalCount = fileList.Count;
               int currentCount = 0;
               //开始上传
               worker.ReportProgress(0, new ProgressState() { ListBoxMsg = "开始下载！", totalCount = totalCount, CurrentCount = 0 });
               //循环
               foreach (FileStatus myfile in fileList)
               {
                   currentCount++;
                   int pgPresent = (int)((double)currentCount / totalCount * 100);
                   
                    string fileName = Path.GetFileName(myfile.Path);
                   if (myfile.Isdir == false)
                   {
                       
                       string savePath = localRootPath + "/" + fileName;
                       if (fileType == 1&&myfile.FileName!=null)
                       {
                           savePath = localRootPath + "/" + myfile.FileName;
                       }
                       if (fileType == 2 && myfile.FileName != null)
                       {
                           savePath = localRootPath + "/" + myfile.FileName+"/"+fileName;
                       }
                       //显示总进度
                       worker.ReportProgress(pgPresent, new ProgressState() { CurrentTitle = fileName, CurrentCount = currentCount });

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
                       ThriftHandle th = thriftClient.open(new Pathname() { pathname = myfile.Path });
                       // 创建文件流
                       FileStream fs = new FileStream(savePath, FileMode.Create, FileAccess.Write);
                       long totalBytes = 0;
                       int readLength = 1024 * 1024;
                       try
                       {
                           UTF8Encoding utf8 = new UTF8Encoding(false, true);
                           while (true)
                           {
                               int needRead = readLength;
                               if (myfile.Length - totalBytes < readLength)
                               {
                                   needRead = (int)(myfile.Length - totalBytes);
                               }
                               if (needRead <= 0)
                                   break;

                               byte[] fileBuffer = thriftClient.read(th, totalBytes, needRead);
                               byte[] myfileBuffer = Encoding.Convert(utf8, Encoding.GetEncoding("iso-8859-1"), fileBuffer);
                               totalBytes += needRead;
                               fs.Write(myfileBuffer, 0, myfileBuffer.Length);

                               //显示单个上传进度
                               int mypresent = (int)((double)totalBytes / myfile.Length * 100);
                               worker.ReportProgress(pgPresent, new ProgressState() { CurrentTitle = mypresent + "% " + fileName });

                           }
                           result = true;
                       }
                       catch (Exception ee)
                       {
                           throw ee;
                       }
                       finally
                       {
                           fs.Dispose();
                           if (thriftClient != null)
                               thriftClient.close(th);
                       }

                       #endregion

                       string msg = string.Format("{0} 下载{1}", Path.GetFileName(fileName), result ? "成功" : "失败");
                       worker.ReportProgress(pgPresent, new ProgressState() { ListBoxMsg = msg });
                   }
                   else
                   {
                       worker.ReportProgress(pgPresent, new ProgressState() { ListBoxMsg = fileName+"不是文件！"});
                   }
               }
           }

           //释放连接
           if (btsport != null)
           {
               btsport.Close();
           }


       }

       //上传文件
       public bool Create(ThriftHadoopFileSystem.Client client, string localPath, string path)
       {
           bool result = false;
           if (client != null)
           {
               ThriftHandle th = null;
               FileStream fs = null;
               try
               {

                   //创建一个文件
                   th = client.createFile(new Pathname() { pathname = path }, 1, true, 1024, ConfigHelper.HDFSREPLICATION, 1024*1024*64);
                   
                   UTF8Encoding utf8 = new UTF8Encoding(false,true);

                   fs = new FileStream(localPath, FileMode.Open, FileAccess.Read);

                   byte[] fileBuffer = new byte[1024 * 1024];	// 每次传1MB
                   int bytesRead;
                   while ((bytesRead = fs.Read(fileBuffer, 0, fileBuffer.Length)) > 0)
                   {
                       byte[] realBuffer = new byte[bytesRead];
                       Array.Copy(fileBuffer, realBuffer, bytesRead);
                       //将utf8转为可存储编码
                       byte[] buf = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), utf8, realBuffer);
                       //发送
                       client.write(th,buf);
                       //清仓缓存
                       Array.Clear(fileBuffer, 0, fileBuffer.Length);
                   }
                   result = true;
               }
               catch (Exception ee)
               {
                   throw ee;
               }
               finally
               {
                   if (th != null)
                       client.close(th);
                   if (fs != null)
                       fs.Close();
               }
           }

           return result;
       }

       //批量上传
       public void MutUpload(BackgroundWorker worker, List<string>localPath, string remoteRootPath,string localRootPath)
       {
           //相同操作
           bool sameOp = false;
           //是否覆盖
           bool IsOver = false;
           //准备创建连接
           Thrift.Transport.TBufferedTransport btsport = null;
           ThriftHadoopFileSystem.Client thriftClient = Connect(out btsport);
           List<string> NoSuccessList = new List<string>();
           List<string> skipList = new List<string>();
           if (thriftClient != null)//连接成功
           {
               int totalCount = localPath.Count;
               int currentCount = 0;
               //开始上传
               worker.ReportProgress(0, new ProgressState() { ListBoxMsg = "开始上传！",totalCount=totalCount, CurrentCount=0 });
               //循环
               foreach (string localFilePath in localPath)
               {
                   string fileName = Path.GetFileName(localFilePath);
                   if (!string.IsNullOrEmpty(localRootPath))//如果是文件夹，则包含原有路径
                       fileName = localFilePath.Replace(localRootPath, "");

                   string remoteFilePath = remoteRootPath + "/" + fileName;
                   currentCount++;
                   int pgPresent = (int)((double)currentCount / totalCount * 100);
                   //显示总进度
                   worker.ReportProgress(pgPresent, new ProgressState() { CurrentTitle = fileName, CurrentCount = currentCount });

                   #region 是否存在
                   if (!sameOp)
                   {
                       bool exsitFile = thriftClient.exists(new Pathname() { pathname = remoteFilePath });
                       if (exsitFile)
                       {
                           SureDialog myDialog = new SureDialog();
                           MyShowDialogResult myDR = new MyShowDialogResult();
                           myDialog.ShowDialog(fileName, myDR);
                           sameOp = myDR.IsCheck;
                           IsOver = myDR.Result;
                           if (!myDR.Result)
                           {
                               worker.ReportProgress(pgPresent, new ProgressState() { ListBoxMsg =  fileName +" 跳过 " });
                               skipList.Add(fileName);
                               continue;
                           }
                       }
                   }
                   else
                   {
                        bool exsitFile = thriftClient.exists(new Pathname() { pathname = remoteFilePath });
                        if (exsitFile)
                        {
                            if (!IsOver)
                            {
                                worker.ReportProgress(pgPresent, new ProgressState() { ListBoxMsg =fileName + "跳过 " });
                                skipList.Add(fileName);
                                continue;
                            }
                        }
                   }
                   #endregion
                   

                    #region 上传单个文件
                   bool singleResult = false;
                   ThriftHandle th = null;
                   FileStream fs = null;
                   try
                   {
                       Pathname myNewFile=new Pathname() { pathname = remoteFilePath };

                       //创建一个文件
                       th = thriftClient.createFile(myNewFile, 1, true, 1024*1024*10, ConfigHelper.HDFSREPLICATION, 1024 * 1024 * 512);
                       
                       UTF8Encoding utf8 = new UTF8Encoding(false, true);

                       fs = new FileStream(localFilePath, FileMode.Open, FileAccess.Read);

                       byte[] fileBuffer = new byte[1024 * 1024*10];	// 每次传1MB
                       int bytesRead;
                       long bytesTotal = 0;

                       while ((bytesRead = fs.Read(fileBuffer, 0, fileBuffer.Length)) > 0)
                       {
                           bytesTotal += bytesRead;
                           byte[] realBuffer = new byte[bytesRead];
                           Array.Copy(fileBuffer, realBuffer, bytesRead);
                           //将utf8转为可存储编码
                           realBuffer = Encoding.Convert(Encoding.GetEncoding("iso-8859-1"), utf8, realBuffer);
                           //发送
                           thriftClient.write(th, realBuffer);
                           //清仓缓存
                           Array.Clear(fileBuffer, 0, fileBuffer.Length);
                           realBuffer = null;
                           //显示单个上传进度
                           int mypresent = (int)((double)bytesTotal / fs.Length * 100);
                           worker.ReportProgress(pgPresent, new ProgressState() { CurrentTitle = mypresent + "% " + fileName });
                       }
                       singleResult = true;
                   }
                   catch (Exception ee)
                   {
                       //显示上传错误
                       worker.ReportProgress(pgPresent, new ProgressState() { ListBoxMsg = ee.Message });
                   }
                   finally
                   {
                       if (th != null)
                           thriftClient.close(th);
                       if (fs != null)
                           fs.Close();
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

           //释放连接
           if (btsport != null)
           {
               btsport.Close();
           }
           //输出没有上传成功的
           if (NoSuccessList.Count > 0)
           {
               try
               {
                   File.WriteAllText("c:/UploadNoSuccess.txt", string.Join("\r\n", NoSuccessList.ToArray()));
                   worker.ReportProgress(100, new ProgressState() { ListBoxMsg = NoSuccessList.Count + "个上传错误！请查看c:/UploadNoSuccess.txt" });
               }
               catch (Exception ee)
               {
                   worker.ReportProgress(100, new ProgressState() { ListBoxMsg =ee.Message  });
               }
           }
           //输出跳过的
           if (skipList.Count > 0)
           {
               try
               {
                   File.WriteAllText("c:/UploadSkip.txt", string.Join("\r\n", skipList.ToArray()));
                   worker.ReportProgress(100, new ProgressState() { ListBoxMsg = "跳过" + skipList.Count + "个文件！请查看c:/UploadSkip.txt" });
               }
               catch (Exception ee)
               {
                   worker.ReportProgress(100, new ProgressState() { ListBoxMsg = ee.Message });
               }
           }
       }

       
       /// <summary>
       /// 删除文件或文件夹
       /// </summary>
       /// <param name="path"></param>
       /// <param name="recursive">是否删除子文件夹</param>
       /// <returns></returns>
       public bool Delete(string path,bool recursive)
       {
           TBufferedTransport tsport = null;
           ThriftHadoopFileSystem.Client client = Connect(out tsport);
           bool result = false;
           if (client != null)
           {
               Pathname pn = new Pathname() { pathname = path };
               if (client.exists(pn))//如果不存在才执行
                   result = client.rm(pn, recursive);
               tsport.Close();
               
           }
           return result;
       }

       //剪切
       public List<string> MoveFile(string[] sourcePath, string dectPath)
       {
           TBufferedTransport tsport = null;
           ThriftHadoopFileSystem.Client client = Connect(out tsport);
           List<string> result = new List<string>();
           if (client != null)
           {
               foreach (string itemSource in sourcePath)
               {
                   Pathname pn = new Pathname() { pathname = itemSource };
                  string fileName= itemSource.Substring(itemSource.LastIndexOf('/')+1);
                  if (client.exists(pn))//如果存在才执行
                  { 
                    bool thResult=  client.rename(pn, new Pathname() { pathname = dectPath + "/" + fileName });
                    if (!thResult)
                    {
                        result.Add(fileName);
                    }
                  }
               }
               tsport.Close();
           }
           return result;
       }



    }
}
