using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;

/// <summary>
///FileUploader 的摘要说明
///名称:文件上传
///功能:文件类型判断
///创建人:Johnny Wong
///创建日期：2013-02-25
///最新修改: 2017-10-19 
/// </summary>

namespace Jnw.Common
{
    public class FileUpDownHelper
    {
        public FileUpDownHelper()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 判断是否为合法的文件
        /// Author:Johnny Wong
        /// Time:2013-02-25
        /// EditTime:2013-07-12
        /// LastUpdateTime: 2019.03.25
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <returns></returns>
        public static bool IsLegalFile(string fileName)
        {
            //这是文件后缀名
            string extend = "jpg gif png bmp jpeg zip rar txt xls xlsx doc docx dps et ppt pptx wps xlsm 7z";
            string str = new FileInfo(fileName).Extension;
            return extend.Contains(str);
        }

        /// <summary>
        /// 创建一个不重复的文件名
        /// Author:Johnny Wong
        /// Time:2017-10-19
        /// UpdateTime: 2019.03.25
        /// <param name="needGuid">是否需要Guid格式</param>
        /// </summary>
        /// <returns></returns>
        public static string CreateNewFileName(bool needGuid)
        {
            return needGuid ? Guid.NewGuid().ToString() : DateTime.Now.ToString("yyyyMMddhhmmss");
        }

        /// <summary>
        /// 下载文件(winform)
        /// Author:Johnny
        /// Time:2013-08-19
        /// </summary>
        /// <param name="uri">需下载的文件路径(包含文件名)</param>
        /// <param name="savePath">下载文件的存放路径(不包含文件名)</param>
        /// <returns></returns>
        public static bool DownloadFile(string uri, string savePath)
        {
            string fileName;  //被下载的文件名
            if (uri.IndexOf("\\") > -1)
            {
                fileName = uri.Substring(uri.LastIndexOf("\\") + 1);
            }
            else
            {
                fileName = uri.Substring(uri.LastIndexOf("/") + 1);
            }


            if (!savePath.EndsWith("/") && !savePath.EndsWith("\\"))
            {
                savePath = savePath + "/";
            }

            savePath += fileName;   //另存为的绝对路径＋文件名

            WebClient client = new WebClient();
            try
            {
                client.DownloadFile(uri, savePath);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 上传文件(winform)
        /// Author:Johnny
        /// Time:2013-8-19
        /// </summary>
        /// <param name="localFilePath">需要上传的文件</param>
        /// <param name="serverFolder">上传后放置的文件夹</param>
        /// <returns></returns>
        public static bool UploadFile(string localFilePath, string serverFolder)
        {
            return UploadFile(localFilePath, serverFolder, "");
        }

        /// <summary>
        /// 上传文件(winform)
        /// Author:Johnny
        /// Time:2013-8-19
        /// </summary>
        /// <param name="localFilePath">需要上传的文件</param>
        /// <param name="serverFolder">上传后放置的文件夹</param>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static bool UploadFile(string localFilePath, string serverFolder, string fileName)
        {
            string fileNameExt, newFileName, uriString;
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                fileNameExt = localFilePath.Substring(localFilePath.LastIndexOf(".") + 1);
                newFileName = fileName + fileNameExt;
            }
            else
            {
                newFileName = localFilePath.Substring(localFilePath.LastIndexOf("\\") + 1);
            }

            if (!serverFolder.EndsWith("/") && !serverFolder.EndsWith("\\"))
            {
                serverFolder = serverFolder + "/";
            }

            uriString = serverFolder + newFileName;   //服务器保存路径
            /// 创建WebClient实例
            WebClient myWebClient = new WebClient();
            myWebClient.Credentials = CredentialCache.DefaultCredentials;

            // 要上传的文件
            FileStream fs = new FileStream(newFileName, FileMode.Open, FileAccess.Read);
            BinaryReader r = new BinaryReader(fs);
            try
            {
                //使用UploadFile方法可以用下面的格式
                //myWebClient.UploadFile(uriString,"PUT",localFilePath);
                byte[] postArray = r.ReadBytes((int)fs.Length);
                Stream postStream = myWebClient.OpenWrite(uriString, "PUT");
                if (postStream.CanWrite)
                {
                    postStream.Write(postArray, 0, postArray.Length);
                }
                else
                {
                    throw new Exception("文件目前不可写！");
                }
                postStream.Close();
            }
            catch
            {
                //MessageBox.Show("文件上传失败，请稍候重试~");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Author:Johnny Wong
        /// Date:2013-08-23
        /// 连接至共享文件夹(winform)
        /// </summary>
        /// <param name="remoteHost">IP地址或域名</param>
        /// <param name="shareName">共享名</param>
        /// <param name="userName">帐号</param>
        /// <param name="passWord">密码</param>
        /// <returns>连接结果</returns>        
        public static bool Connect(string remoteHost, string shareName, string userName, string passWord)
        {
            bool Flag = false;
            Process proc = new Process();
            try
            {
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string dosLine = @"net use \\" + remoteHost + @"\" + shareName + " /User:" + userName + " " + passWord + " /PERSISTENT:YES";
                proc.StandardInput.WriteLine(dosLine);
                proc.StandardInput.WriteLine("exit");
                while (!proc.HasExited)
                {
                    proc.WaitForExit(1000);
                }

                string errormsg = proc.StandardError.ReadToEnd();
                proc.StandardError.Close();
                if (String.IsNullOrEmpty(errormsg))
                {
                    Flag = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                proc.Close();
                proc.Dispose();
            }
            return Flag;
        }

        /// <summary>
        /// Author:Johnny Wong
        /// Date:2013-08-23
        /// 打开指定的路径
        /// </summary>
        /// <param name="path">路径</param>
        public static void OpenFolder(string path)
        {
            Process.Start("explorer.exe", path);
        }
    }
}