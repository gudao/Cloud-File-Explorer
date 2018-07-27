using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
namespace NetHadoop
{
    public static class ConfigHelper
    {
        /// <summary>
        /// Thrift IP地址 默认192.168.1.1
        /// </summary>
        public static readonly string HdfsHost = ConfigurationManager.AppSettings["HdfsThriftIP"] ?? "192.168.1.1";
        
        /// <summary>
        /// Thrift 端口 默认8888
        /// </summary>
        public static readonly int HdfsPort = Convert.ToInt32(ConfigurationManager.AppSettings["HdfsThriftPort"] ?? "8888");

        /// <summary>
        /// 服务器根路径 默认hdfs://192.168.1.1:9000/user/root
        /// </summary>
        public static readonly string HdfsRoot = ConfigurationManager.AppSettings["HdfsRoot"] ?? "hdfs://192.168.1.1:9000/user/root";

        /// <summary>
        /// Hadoop建议份数为3
        /// </summary>
        public static readonly short HDFSREPLICATION = Convert.ToInt16(ConfigurationManager.AppSettings["HDFSREPLICATION"] ?? "3");


        //阿里云OSS配置
        public static readonly string AccessKeyId = "AccessKeyId";

        public static readonly string AccessKeySecret = "AccessKeySecret";

        public static readonly string Endpoint = "Endpoint";


    }
}
