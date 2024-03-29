/*
 * MinIO .NET Library for Amazon S3 Compatible Cloud Storage, (C) 2020, 2021 MinIO, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;


using Minio.DataModel;
using Minio.DataModel.Tags;
using Minio.DataModel.ObjectLock;

namespace Minio
{
    internal class SelectObjectContentResponse : GenericResponse
    {
        internal SelectResponseStream ResponseStream { get; private set; }
        internal SelectObjectContentResponse(HttpStatusCode statusCode, string responseContent, byte[] responseRawBytes)
                    : base(statusCode, responseContent)
        {
            this.ResponseStream = new SelectResponseStream(new MemoryStream(responseRawBytes));
        }

    }


    internal class StatObjectResponse : GenericResponse
    {
        internal ObjectStat ObjectInfo { get; set; }
        internal StatObjectResponse(HttpStatusCode statusCode, string responseContent, Dictionary<string, string> responseHeaders, StatObjectArgs args)
                    : base(statusCode, responseContent)
        {
            // StatObjectResponse object is populated with available stats from the response.
            this.ObjectInfo = ObjectStat.FromResponseHeaders(args.ObjectName, responseHeaders);
        }
    }

    internal class RemoveObjectsResponse : GenericResponse
    {
        internal DeleteObjectsResult DeletedObjectsResult { get; private set; }
        internal RemoveObjectsResponse(HttpStatusCode statusCode, string responseContent)
                    : base(statusCode, responseContent)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(responseContent)))
            {
                this.DeletedObjectsResult = (DeleteObjectsResult)new XmlSerializer(typeof(DeleteObjectsResult)).Deserialize(stream);
            }
        }
    }

    internal class GetMultipartUploadsListResponse : GenericResponse
    {
        internal Tuple<ListMultipartUploadsResult, List<Upload>> UploadResult { get; private set; }
        internal GetMultipartUploadsListResponse(HttpStatusCode statusCode, string responseContent)
                    : base(statusCode, responseContent)
        {
            ListMultipartUploadsResult uploadsResult = null;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(responseContent)))
            {
                uploadsResult = (ListMultipartUploadsResult)new XmlSerializer(typeof(ListMultipartUploadsResult)).Deserialize(stream);
            }
            XDocument root = XDocument.Parse(responseContent);
            var itemCheck = root.Root.Descendants("{http://s3.amazonaws.com/doc/2006-03-01/}Upload").FirstOrDefault();
            if (uploadsResult == null || itemCheck == null || !itemCheck.HasElements)
            {
                return;
            }
            var uploads = from c in root.Root.Descendants("{http://s3.amazonaws.com/doc/2006-03-01/}Upload")
                          select new Upload
                          {
                              Key = c.Element("{http://s3.amazonaws.com/doc/2006-03-01/}Key").Value,
                              UploadId = c.Element("{http://s3.amazonaws.com/doc/2006-03-01/}UploadId").Value,
                              Initiated = c.Element("{http://s3.amazonaws.com/doc/2006-03-01/}Initiated").Value
                          };
            this.UploadResult = new Tuple<ListMultipartUploadsResult, List<Upload>>(uploadsResult, uploads.ToList());
        }
    }

    public class PresignedPostPolicyResponse
    {
        internal Tuple<string, Dictionary<string, string>> URIPolicyTuple { get; private set; }

        public PresignedPostPolicyResponse(PresignedPostPolicyArgs args, Uri URI)
        {
            URIPolicyTuple = Tuple.Create(URI.AbsolutePath, args.Policy.GetFormData());
        }
    }

    public class GetLegalHoldResponse : GenericResponse
    {
        internal ObjectLegalHoldConfiguration CurrentLegalHoldConfiguration { get; private set; }
        internal string Status { get; private set; }
        public GetLegalHoldResponse(HttpStatusCode statusCode, string responseContent)
            : base(statusCode, responseContent)
        {
            if (string.IsNullOrEmpty(responseContent) || !HttpStatusCode.OK.Equals(statusCode))
            {
                this.CurrentLegalHoldConfiguration = null;
                return;
            }
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(responseContent)))
            {
                CurrentLegalHoldConfiguration = (ObjectLegalHoldConfiguration)new XmlSerializer(typeof(ObjectLegalHoldConfiguration)).Deserialize(stream);
            }
            if (this.CurrentLegalHoldConfiguration == null
                    || string.IsNullOrEmpty(this.CurrentLegalHoldConfiguration.Status))
            {
                Status = "OFF";
            }
            else
            {
                Status = this.CurrentLegalHoldConfiguration.Status;
            }
        }
    }

    internal class GetObjectTagsResponse : GenericResponse
    {
        public GetObjectTagsResponse(HttpStatusCode statusCode, string responseContent)
            : base(statusCode, responseContent)
        {
            if (string.IsNullOrEmpty(responseContent) ||
                    !HttpStatusCode.OK.Equals(statusCode))
            {
                this.ObjectTags = null;
                return;
            }
            responseContent = utils.RemoveNamespaceInXML(responseContent);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(responseContent)))
            {
                this.ObjectTags = (Tagging)new XmlSerializer(typeof(Tagging)).Deserialize(stream);
            }
        }

        public Tagging ObjectTags { get; set; }
    }

    internal class GetRetentionResponse : GenericResponse
    {
        internal ObjectRetentionConfiguration CurrentRetentionConfiguration { get; private set; }
        public GetRetentionResponse(HttpStatusCode statusCode, string responseContent)
            : base(statusCode, responseContent)
        {
            if (string.IsNullOrEmpty(responseContent) && !HttpStatusCode.OK.Equals(statusCode))
            {
                this.CurrentRetentionConfiguration = null;
                return;
            }
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(responseContent)))
            {
                CurrentRetentionConfiguration = (ObjectRetentionConfiguration)new XmlSerializer(typeof(ObjectRetentionConfiguration)).Deserialize(stream);
            }
        }
    }

    internal class CopyObjectResponse : GenericResponse
    {
        internal CopyObjectResult CopyObjectRequestResult { get; set; }
        internal CopyPartResult CopyPartRequestResult { get; set; }

        public CopyObjectResponse(HttpStatusCode statusCode, string content, Type reqType)
            : base(statusCode, content)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                if (reqType == typeof(CopyObjectResult))
                {
                    this.CopyObjectRequestResult = (CopyObjectResult)new XmlSerializer(typeof(CopyObjectResult)).Deserialize(stream);
                }
                else
                {
                    this.CopyPartRequestResult = (CopyPartResult)new XmlSerializer(typeof(CopyPartResult)).Deserialize(stream);
                }
            }
        }
    }

    internal class NewMultipartUploadResponse : GenericResponse
    {
        internal string UploadId { get; private set; }
        internal NewMultipartUploadResponse(HttpStatusCode statusCode, string responseContent)
                    : base(statusCode, responseContent)
        {
            InitiateMultipartUploadResult newUpload = null;
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(responseContent)))
            {
                newUpload = (InitiateMultipartUploadResult)new XmlSerializer(typeof(InitiateMultipartUploadResult)).Deserialize(stream);
            }
            this.UploadId = newUpload.UploadId;
        }
    }

    internal class PutObjectResponse : GenericResponse
    {
        internal string Etag;

        internal PutObjectResponse(HttpStatusCode statusCode, string responseContent, Dictionary<string, string> responseHeaders)
                    : base(statusCode, responseContent)
        {
            if (responseHeaders.ContainsKey("Etag"))
            {
                if (!string.IsNullOrEmpty("Etag"))
                    this.Etag = responseHeaders["ETag"];
                return;
            }

            foreach (KeyValuePair<string, string> parameter in responseHeaders)
            {
                if (parameter.Key.Equals("ETag", StringComparison.OrdinalIgnoreCase))
                {
                    this.Etag = parameter.Value.ToString();
                    return;
                }
            }
        }

    }
}