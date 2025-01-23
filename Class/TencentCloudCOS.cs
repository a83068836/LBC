using COSXML;
using COSXML.Auth;
using COSXML.Common;
using COSXML.CosException;
using COSXML.Model;
using COSXML.Model.Bucket;
using COSXML.Model.Object;
using COSXML.Model.Service;
using COSXML.Model.Tag;
using COSXML.Network;
using COSXML.Transfer;
using COSXML.Utils;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;
using System.Xml.Linq;

namespace LBC
{
    public class CosBuilder
    {
        private CosXmlServer cosXml;
        private string _appid;
        private string _region;
        private CosXmlConfig cosXmlConfig;
        private QCloudCredentialProvider cosCredentialProvider;
        public CosBuilder()
        {
            // 构建一个 CoxXmlServer 对象
            
        }


        public CosBuilder SetAccount(string appid, string region)
        {
            _appid = appid;
            _region = region;
            return this;
        }
        public CosBuilder SetCosXmlServer(int ConnectionTimeoutMs = 60000, int ReadWriteTimeoutMs = 40000, bool IsHttps = true, bool SetDebugLog = true)
        {
            cosXmlConfig = new CosXmlConfig.Builder()
                .SetConnectionTimeoutMs(ConnectionTimeoutMs)
                .SetReadWriteTimeoutMs(ReadWriteTimeoutMs)
                .IsHttps(true)
                .SetAppid(_appid)
                .SetRegion(_region)
                .SetDebugLog(true)
                .Build();
            return this;
        }
        public CosBuilder SetSecret(string secretId, string secretKey, long durationSecond = 600)
        {

            cosCredentialProvider = new DefaultQCloudCredentialProvider(secretId, secretKey, durationSecond);
            return this;
        }
        public CosXmlServer Builder()
        {
            //初始化 CosXmlServer
            cosXml = new CosXmlServer(cosXmlConfig, cosCredentialProvider);
            return cosXml;
        }
        public static async Task UploadFileAsync(string filePath)
        {
            var cosClient = new CosBuilder()
                .SetAccount(App.global.appid, App.global.region)
                .SetCosXmlServer()
                .SetSecret(App.global.SecretId, App.global.SecretKey)
                .Builder();

            BucketClient bucketClient = new BucketClient(cosClient, App.global.buketName, App.global.appid);

            string convertedPath = ConvertToCOSPath(filePath);

            // 使用异步方法上传文件
            await bucketClient.UpFile(convertedPath, filePath);
        }
        public static async Task<bool> CheckObjectAsync(string filePath)
        {
            var cosClient = new CosBuilder()
                .SetAccount(App.global.appid, App.global.region)
                .SetCosXmlServer()
                .SetSecret(App.global.SecretId, App.global.SecretKey)
                .Builder();

            BucketClient bucketClient = new BucketClient(cosClient, App.global.buketName, App.global.appid);

            string convertedPath = ConvertToCOSPath(filePath);
            // 使用异步方法上传文件
            return await bucketClient.CheckObject(convertedPath, filePath);
        }
        public static async Task<List<BucketItem>> SelectBucket()
        {
            var cosClient = new CosBuilder()
                .SetAccount(App.global.appid, App.global.region)
                .SetCosXmlServer()
                .SetSecret(App.global.SecretId, App.global.SecretKey)
                .Builder();

            CosClient bucketClient = new CosClient(cosClient, App.global.appid);
            List<BucketItem> bucketItems = new List<BucketItem>();
            // 使用异步方法上传文件
            var responseString = await bucketClient.SelectBucket();
            try
            {
                XDocument doc = XDocument.Parse(responseString); // 解析 XML

                var owner = doc.Descendants("Owner").Select(o => new
                {
                    ID = o.Element("ID").Value,
                    DisplayName = o.Element("DisplayName").Value
                }).FirstOrDefault();

                var isTruncated = doc.Element("ListAllMyBucketsResult").Element("IsTruncated").Value;

                var buckets = doc.Descendants("Bucket").Select(b => new
                {
                    Name = b.Element("Name").Value,
                    Location = b.Element("Location").Value,
                    CreationDate = DateTime.Parse(b.Element("CreationDate").Value),
                    BucketType = b.Element("BucketType").Value
                });
                
                foreach (var bucket in buckets)
                {
                    bucketItems.Add(new BucketItem() { Name = bucket.Name, Location = bucket.Location, CreationDate = bucket.CreationDate, BucketType = bucket.BucketType });
                }
                return bucketItems;
            }
            catch (Exception ex)
            {
                return bucketItems;

            } 
        }
        public static async Task<ResponseModel> CreateBucket(string buketName)
        {
            var cosClient = new CosBuilder()
                .SetAccount(App.global.appid, App.global.region)
                .SetCosXmlServer()
                .SetSecret(App.global.SecretId, App.global.SecretKey)
                .Builder();

            CosClient bucketClient = new CosClient(cosClient, App.global.appid);
            ;
            // 使用异步方法上传文件
            return await bucketClient.CreateBucket(buketName);
        }
        public static string ConvertToCOSPath(string originalPath)
        {
            // 去除盘符和文件名
            string[] parts = originalPath.Split('\\');
            string cosPath = string.Join("/", parts[1..]);

            // 根据需要转换为小写
            //cosPath = cosPath.ToLower();

            return cosPath;
        }

        public static async Task<ListBucket> SelectObjectList()
        {
            var cosClient = new CosBuilder()
                .SetAccount(App.global.appid, App.global.region)
                .SetCosXmlServer()
                .SetSecret(App.global.SecretId, App.global.SecretKey)
                .Builder();

            BucketClient bucketClient = new BucketClient(cosClient, App.global.buketName, App.global.appid);


            // 使用异步方法上传文件
            return await bucketClient.SelectObjectList();
        }
        public static async Task<ResponseModel> DownObject(string key, string path)
        {
            var cosClient = new CosBuilder()
                .SetAccount(App.global.appid, App.global.region)
                .SetCosXmlServer()
                .SetSecret(App.global.SecretId, App.global.SecretKey)
                .Builder();

            BucketClient bucketClient = new BucketClient(cosClient, App.global.buketName, App.global.appid);

            string[] strings =ConvertToCOSPath(key, path);
            // 使用异步方法下载文件
            return await bucketClient.DownObject(key, strings[0], strings[1]);
            //return null;
        }
        public static string[] ConvertToCOSPath(string key,string path)
        {
            string[] result = new string[2];
            if (string.IsNullOrEmpty(path))
                Directory.CreateDirectory(path);

            // 获取目录的父目录
            string localDir = Path.GetDirectoryName(path);

            // 确保路径以 '\' 结尾
            if (!localDir.EndsWith("\\"))
            {
                localDir += "\\";
            }
            localDir += key.Replace("/", "\\");
            string localFileName = Path.GetFileName(localDir);
            result[0] = localDir.Replace("\\" + localFileName, "");
            result[1] = localFileName;
            return result;
        }
    }
    public interface ICosClient
    {
        // 创建存储桶
        Task<ResponseModel> CreateBucket(string buketName);

        // 获取存储桶列表
        Task<string> SelectBucket(int tokenTome = 600);
    }
    public interface IBucketClient
    {
        // 上传文件
        Task<ResponseModel> UpFile(string key, string srcPath);

        // 分块上传大文件
        Task<ResponseModel> UpBigFile(string key, string srcPath, Action<long, long> progressCallback, Action<CosResult> successCallback);

        /// <summary>
        /// 检测文件是否存在
        /// </summary>
        /// <param name="cosFilePath">cos文件路径</param>
        /// <param name="localFilePath">本地文件路径</param>
        /// <returns></returns>
        Task<bool> CheckObject(string cosFilePath, string localFilePath);
        // 查询存储桶的文件列表
        Task<ListBucket> SelectObjectList();

        // 下载文件
        Task<ResponseModel> DownObject(string key, string localDir, string localFileName);

        // 删除文件
        Task<ResponseModel> DeleteObject(string buketName);
    }
    /// <summary>
    /// Cos客户端
    /// </summary>
    public class CosClient : ICosClient
    {
        CosXmlServer _cosXml;
        private readonly string _appid;
        public CosClient(CosXmlServer cosXml, string appid)
        {
            _cosXml = cosXml;
            _appid = appid;
        }
        public async Task<ResponseModel> CreateBucket(string buketName)
        {
            try
            {
                string bucket = buketName + "-" + _appid; //存储桶名称 格式：BucketName-APPID
                PutBucketRequest request = new PutBucketRequest(bucket);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.Seconds), 600);
                //执行请求
                PutBucketResult result = await Task.FromResult(_cosXml.PutBucket(request));

                return new ResponseModel { Code = 200, Message = result.GetResultInfo() };
            }
            catch (CosClientException clientEx)
            {
                return new ResponseModel { Code = 0, Message = "CosClientException: " + clientEx.Message + clientEx.InnerException };
            }
            catch (CosServerException serverEx)
            {
                return new ResponseModel { Code = 200, Message = "CosServerException: " + serverEx.GetInfo() };
            }
        }
        public async Task<string> SelectBucket(int tokenTome = 600)
        {
            try
            {
                GetServiceRequest request = new GetServiceRequest();
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.Seconds), tokenTome);
                //执行请求
                GetServiceResult result = await Task.FromResult(_cosXml.GetService(request));
                //return new ResponseModel { Code = 200, Message = "Success", Data = result.GetResultInfo() };
                return result.RawContentBodyString;
            }
            catch (COSXML.CosException.CosClientException clientEx)
            {
                return "";// new ResponseModel { Code = 0, Message = "CosClientException: " + clientEx.Message };
            }
            catch (CosServerException serverEx)
            {
                return "";//new ResponseModel { Code = 0, Message = "CosServerException: " + serverEx.GetInfo() };
            }
        }

    }

    /// <summary>
    /// 存储桶客户端
    /// </summary>
    public class BucketClient : IBucketClient
    {
        private readonly CosXmlServer _cosXml;
        private readonly string _buketName;
        private readonly string _appid;
        public BucketClient(CosXmlServer cosXml, string buketName, string appid)
        {
            _cosXml = cosXml;
            _buketName = buketName;
            _appid = appid;
        }
        public async Task<ResponseModel> UpFile(string cosFilePath, string localFilePath)
        {
            try
            {
                string bucket = _buketName + "-" + _appid; //存储桶名称 格式：BucketName-APPID
                PutObjectRequest request = new PutObjectRequest(bucket, cosFilePath, localFilePath/*new byte[0]*/);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.Seconds), 600);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                PutObjectResult result = await Task.FromResult(_cosXml.PutObject(request));
                return new ResponseModel { Code = 200, Message = result.GetResultInfo() };
            }
            catch (CosClientException clientEx)
            {
                return new ResponseModel { Code = 0, Message = "CosClientException: " + clientEx.Message };
            }
            catch (CosServerException serverEx)
            {
                return new ResponseModel { Code = 0, Message = "CosServerException: " + serverEx.GetInfo() };
            }
        }

        public async Task<bool> CheckObject(string cosFilePath, string localFilePath)
        {
            string bucket = _buketName + "-" + _appid; //存储桶名称 格式：BucketName-APPID
            // 要检查的文件在 COS 中的完整路径

            // 构造 HeadObject 请求
            HeadObjectRequest headObjectRequest = new HeadObjectRequest(bucket, cosFilePath);

            // 发送 HeadObject 请求
            HeadObjectResult headObjectResult = _cosXml.HeadObject(headObjectRequest);

            // 判断文件是否存在
            if (headObjectResult != null && headObjectResult.httpCode == 200)
            {
                // 文件存在，获取文件的 ETag 和 LastModified 时间戳
                string eTag = headObjectResult.eTag;
                DateTime lastModified = DateTime.Parse(headObjectResult.lastModified);
                FileInfo fileInfo = new FileInfo(localFilePath);
                if (fileInfo.Exists)
                {
                    long fileSizeInBytes = fileInfo.Length;
                    if (headObjectResult.size == fileSizeInBytes)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
                
                return true;

                // TODO: 进行进一步处理，比较 ETag 或者 LastModified 时间戳来检查文件是否修改
            }
            else
            {
                // 文件不存在
                return false;
                Console.WriteLine("文件不存在");
            }
        }
        public async Task<bool> UploadFileAsync(string folder, string objectKey, string localFilePath)
        {
            try
            {
                string bucket = _buketName + "-" + _appid; //存储桶名称 格式：BucketName-APPID
                // 构造对象键（Object Key），将文件上传到指定文件夹下
                string fullObjectKey = folder.TrimEnd('/') + "/" + objectKey;

                // 创建 PutObjectRequest 对象
                PutObjectRequest request = new PutObjectRequest(bucket, "Mirserver/Mir200/Envir/MapQuest_Def/"+ objectKey, localFilePath);
                //PutObjectRequest request = new PutObjectRequest(bucket, fullObjectKey, localFilePath);

                // 异步上传对象
                PutObjectResult result = await Task.FromResult(_cosXml.PutObject(request));

                // 检查上传结果
                if (result.httpCode != 200)
                {
                    throw new Exception($"上传失败：{result.httpMessage}");
                }

                return true; // 上传成功
            }
            catch (Exception ex)
            {
                Console.WriteLine($"上传失败：{ex.Message}");
                return false; // 上传失败
            }
        }


    /// <summary>
    /// 上传大文件、分块上传
    /// </summary>
    /// <param name="key"></param>
    /// <param name="srcPath"></param>
    /// <param name="progressCallback">委托，可用于显示分块信息</param>
    /// <param name="successCallback">委托，当任务成功时回调</param>
    /// <returns></returns>
    public async Task<ResponseModel> UpBigFile(string key, string srcPath, Action<long, long> progressCallback, Action<CosResult> successCallback)
        {
            ResponseModel responseModel = new ResponseModel();
            string bucket = _buketName + "-" + _appid; //存储桶名称 格式：BucketName-APPID

            TransferManager transferManager = new TransferManager(_cosXml, new TransferConfig());
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, key);
            uploadTask.SetSrcPath(srcPath);
            uploadTask.progressCallback = delegate (long completed, long total)
            {
                progressCallback(completed, total);
                //Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };
            uploadTask.successCallback = delegate (CosResult cosResult)
            {
                COSXMLUploadTask.UploadTaskResult result = cosResult as COSXMLUploadTask.UploadTaskResult;
                successCallback(cosResult);
                responseModel.Code = 200;
                responseModel.Message = result.GetResultInfo();
            };
            uploadTask.failCallback = delegate (CosClientException clientEx, CosServerException serverEx)
            {
                if (clientEx != null)
                {
                    responseModel.Code = 0;
                    responseModel.Message = clientEx.Message;
                }
                if (serverEx != null)
                {
                    responseModel.Code = 0;
                    responseModel.Message = "CosServerException: " + serverEx.GetInfo();
                }
            };
            await Task.Run(() =>
            {
                transferManager.Upload(uploadTask);
            });
            return responseModel;
        }

        public async Task<ListBucket> SelectObjectList()
        {
            try
            {
                string bucket = _buketName + "-" + _appid; //存储桶名称 格式：BucketName-APPID
                string nextMarker = string.Empty;
                int count = 0;
                ListBucket list = new ListBucket();
                list.contentsList = new List<ListBucket.Contents>();
                while (true)
                {
                    GetBucketRequest request = new GetBucketRequest(bucket);
                    if (nextMarker != "")
                        request.SetMarker(nextMarker);
                    //设置签名有效时长
                    request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.Seconds), 600);
                    //执行请求
                    GetBucketResult result = await Task.FromResult(_cosXml.GetBucket(request));

                    //bucket的相关信息
                    ListBucket info = result.listBucket;
                    list.contentsList.AddRange(info.contentsList);
                    count += info.contentsList.Count;
                    if (info.isTruncated)
                    {
                        // 数据被截断，记录下数据下标
                        nextMarker = info.nextMarker;
                    }
                    else
                    {
                        return list;
                    }      
                }     
            }
            catch (Exception ex) 
            {
                return null;
            }
        }
        public async Task<ResponseModel> DownObject(string key, string localDir, string localFileName)
        {
            try
            {
                string bucket = _buketName + "-" + _appid; //存储桶名称 格式：BucketName-APPID
                GetObjectRequest request = new GetObjectRequest(bucket, key, localDir, localFileName);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.Seconds), 600);
                //设置进度回调
                request.SetCosProgressCallback(delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
                });
                //执行请求
                GetObjectResult result = await Task.FromResult(_cosXml.GetObject(request));

                return new ResponseModel { Code = 200, Message = result.GetResultInfo() };
            }
            catch (CosClientException clientEx)
            {
                return new ResponseModel { Code = 0, Message = "CosClientException: " + clientEx.Message };
            }
            catch (CosServerException serverEx)
            {
                return new ResponseModel { Code = 0, Message = serverEx.GetInfo() };
            }
        }
        public async Task<ResponseModel> DeleteObject(string buketName)
        {
            try
            {
                string bucket = _buketName + "-" + _appid; //存储桶名称 格式：BucketName-APPID
                string key = "exampleobject"; //对象在存储桶中的位置，即称对象键.
                DeleteObjectRequest request = new DeleteObjectRequest(bucket, key);
                //设置签名有效时长
                request.SetSign(TimeUtils.GetCurrentTime(TimeUnit.Seconds), 600);
                //执行请求
                DeleteObjectResult result = await Task.FromResult(_cosXml.DeleteObject(request));

                return new ResponseModel { Code = 200, Message = result.GetResultInfo() };
            }
            catch (CosClientException clientEx)
            {
                return new ResponseModel { Code = 0, Message = "CosClientException: " + clientEx.Message };
            }
            catch (CosServerException serverEx)
            {
                return new ResponseModel { Code = 0, Message = "CosServerException: " + serverEx.GetInfo() };
            }
        }
    }

    /// <summary>
    /// 消息响应
    /// </summary>
    public class ResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public dynamic Data { get; set; }
    }
    public class ListAllMyBuckets
    { 
    public string owner {  get; set; }
    }
    public class BucketItem
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime CreationDate { get; set; }
        public string BucketType { get; set; }

        public override string ToString()
        {
            // 在 ComboBox 中显示的文本
            return $"{Name} - {Location}";
        }
    }
}
