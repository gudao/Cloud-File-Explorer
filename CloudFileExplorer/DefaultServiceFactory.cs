using CloudFileExplorer.Helper;
using Microsoft.Extensions.Configuration;
using OnceMi.AspNetCore.OSS;
using System;
using System.Collections.Generic;
using System.Text;

namespace CloudFileExplorer
{
   public class DefaultServiceFactory
    {
        private ICacheProvider _cache = new DefaultCacheProvider();
        public IOSSService Create()
        {
            #region 参数验证


            OSSOptions options = LoadOptions();
            if (options == null ||
                (options.Provider == OSSProvider.Invalid
                && string.IsNullOrEmpty(options.Endpoint)
                && string.IsNullOrEmpty(options.SecretKey)
                && string.IsNullOrEmpty(options.AccessKey)))
                throw new ArgumentException($"Cannot get options.");
            if (options.Provider == OSSProvider.Invalid)
                throw new ArgumentNullException(nameof(options.Provider));
            if (string.IsNullOrEmpty(options.Endpoint) && options.Provider != OSSProvider.Qiniu)
                throw new ArgumentNullException(nameof(options.Endpoint), "When your provider is Minio/QCloud/Aliyun/HuaweiCloud, endpoint can not null.");
            if (string.IsNullOrEmpty(options.SecretKey))
                throw new ArgumentNullException(nameof(options.SecretKey), "SecretKey can not null.");
            if (string.IsNullOrEmpty(options.AccessKey))
                throw new ArgumentNullException(nameof(options.AccessKey), "AccessKey can not null.");
            if ((options.Provider == OSSProvider.Minio
                || options.Provider == OSSProvider.QCloud
                || options.Provider == OSSProvider.Qiniu
                || options.Provider == OSSProvider.HuaweiCloud)
                && string.IsNullOrEmpty(options.Region))
            {
                throw new ArgumentNullException(nameof(options.Region), "When your provider is Minio/QCloud/Qiniu/HuaweiCloud, region can not null.");
            }

            #endregion

            switch (options.Provider)
            {
                case OSSProvider.Aliyun:
                    return new AliyunOSSService(_cache, options);
                case OSSProvider.Minio:
                    return new MinioOSSService(_cache, options);
                case OSSProvider.QCloud:
                    return new QCloudOSSService(_cache, options);
                case OSSProvider.Qiniu:
                    return new QiniuOSSService(_cache, options);
                case OSSProvider.HuaweiCloud:
                    return new HaweiOSSService(_cache, options);
                default:
                    throw new Exception("Unknow provider type");
            }
        }

        private OSSOptions LoadOptions()
        {
            OSSOptions options = new OSSOptions();
            IniHelper ini = new IniHelper();
            options.AccessKey = ini.IniReadValue("Providers", "AccessKey");
            options.Endpoint = ini.IniReadValue("Providers", "Endpoint");
            options.IsEnableCache = Convert.ToBoolean(ini.IniReadValue("Providers", "IsEnableCache","true"));
            options.IsEnableHttps = Convert.ToBoolean(ini.IniReadValue("Providers", "IsEnableHttps", "false"));
            options.Provider = (OSSProvider)Enum.Parse(typeof(OSSProvider), ini.IniReadValue("Providers", "Provider"));
            options.Region = ini.IniReadValue("Providers", "Region");
            options.SecretKey = ini.IniReadValue("Providers", "SecretKey");
            return options;
        }

        private OSSOptions LoadSetting()
        {
            return null;
        }
    }
}
