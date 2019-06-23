using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Jnw.Common
{
    /// <summary>
    /// 自动更新
    /// Author:Johnny
    /// Time:2013-08-21
    /// </summary>
    public class AutoUpdate
    {
        //服务器地址
        private readonly string _serverAddress = "";
        //配置文件路径(服务器)
        private readonly string _configPath = "";
        //更新日志文件路径(本地)
        private readonly string _updateLogPath = "";
        //本地winform所在文件夹
        private readonly string _winformPath = "";

        private static int _updateVersion = 0;

        private ProgressBar _pbUpdate;

        private ManualResetEvent _mre;

        /// <summary>
        /// 实例化
        /// Author:Johnny
        /// Time:2013-08-21
        /// </summary>
        /// <param name="serverAddress">服务器地址</param>
        /// <param name="configPath">服务器配置文件</param>
        /// <param name="updateLogPath">本地更新日志文件</param>
        /// <param name="winformPath">本地winform所在文件夹</param>
        public AutoUpdate(string serverAddress, string configPath, string updateLogPath, string winformPath)
        {
            this._serverAddress = serverAddress;
            this._configPath = configPath;
            this._updateLogPath = updateLogPath;
            this._winformPath = winformPath;
        }

        /// <summary>
        /// 实例化
        /// Author:Johnny
        /// Time:2013-08-24
        /// </summary>
        public AutoUpdate() { }

        /// <summary>
        /// 检查是否有新版本
        /// 以更新日志中最后一个版本号为准
        /// 如果出错则不更新
        /// Author:Johnny
        /// Time:2013-08-24
        /// EditTime:2014-08-18
        /// LastEditTime:2016-07-10
        /// </summary>
        /// <param name="remoteHost">IP地址或域名</param>
        /// <param name="shareName">共享名</param>
        /// <param name="userName">帐号</param>
        /// <param name="passWord">密码</param>
        /// <param name="exVersion">版本号前的提示字符</param>
        /// <returns></returns>
        public bool CheckUpdate(string remoteHost, string shareName, string userName, string passWord, string exVersion)
        {
            try
            {
                FileUpDownHelper.Connect(remoteHost, shareName, userName, passWord);
                string newVersion = XMLHelper.GetItemValue(_configPath, "version", _updateVersion);
                if (!FileHelper.IsExistFile(_updateLogPath))
                    FileHelper.CreateFile(_updateLogPath);
                var updateInfo = FileHelper.GetContent(_updateLogPath);
                string localVersion = "0";
                for (int i = updateInfo.Length - 1; i >= 0; i--)
                {
                    if (updateInfo[i].IndexOf(exVersion) != -1)
                    {
                        localVersion = updateInfo[i].Replace(exVersion, "");
                        break;
                    }
                }
                if (newVersion != localVersion)
                {
                    int versionCount = XMLHelper.GetItemCount(_configPath, "version");
                    while (newVersion != localVersion && _updateVersion + 1 < versionCount)
                    {
                        newVersion = XMLHelper.GetItemValue(_configPath, "version", ++_updateVersion);
                    }
                    if (localVersion != "0")
                        _updateVersion--;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 检查是否有新版本
        /// 以更新日志中最后一个版本号为准
        /// 如果出错则不更新
        /// Author:Johnny
        /// Time:2015-03-06
        /// LastEditTime:2017-12-29
        /// </summary>
        /// <param name="exVersion">版本号前的提示字符</param>
        /// <returns></returns>
        public bool CheckUpdate(string exVersion)
        {
            try
            {
                string newVersion = XMLHelper.GetItemValue(_configPath, "version", _updateVersion).Trim();
                if (!FileHelper.IsExistFile(_updateLogPath))
                    FileHelper.CreateFile(_updateLogPath);
                var updateInfo = FileHelper.GetContent(_updateLogPath);
                string localVersion = "0";
                for (int i = updateInfo.Length - 1; i >= 0; i--)
                {
                    if (updateInfo[i].IndexOf(exVersion) != -1)
                    {
                        localVersion = updateInfo[i].Replace(exVersion, "").Trim();
                        break;
                    }
                }
                if (newVersion != localVersion)
                {
                    int versionCount = XMLHelper.GetItemCount(_configPath, "version");
                    while (newVersion != localVersion && _updateVersion + 1 < versionCount)
                    {
                        newVersion = XMLHelper.GetItemValue(_configPath, "version", ++_updateVersion);
                    }
                    if (localVersion != "0")
                        _updateVersion--;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 结束进程
        /// Author:Johnny Wong
        /// Time:2013-08-24
        /// EditTime:2018.02.01
        /// </summary>
        /// <param name="processName">进程名</param>
        /// <returns></returns>
        public bool KillProcess(string processName)
        {
            try
            {
                return KillProcess(processName, false, true);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 结束类似进程
        /// Author:Johnny Wong
        /// Time:2018-02-01
        /// </summary>
        /// <param name="processName">进程名</param>
        /// <param name="contain">是否包含</param>
        /// <returns></returns>
        public bool KillProcess(string processName, bool contain)
        {
            try
            {
                return KillProcess(processName, contain, false);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 结束类似进程
        /// Author:Johnny Wong
        /// Time:2018-02-01
        /// </summary>
        /// <param name="processName">进程名</param>
        /// <param name="contain">是否包含</param>
        /// <param name="self">是否包含自己</param>
        /// <returns></returns>
        public bool KillProcess(string processName, bool contain, bool self)
        {
            try
            {
                Process[] pcs = Process.GetProcesses();
                foreach (Process item in pcs)
                {
                    if (contain)
                    {
                        if (item.ProcessName.Contains(processName))
                        {
                            if (self)
                            {
                                item.Kill();
                            }
                            else
                            {
                                if (item.Id != Process.GetCurrentProcess().Id)
                                    item.Kill();
                            }
                        }
                    }
                    else
                    {
                        if (item.ProcessName == processName)
                        {
                            if (self)
                            {
                                item.Kill();
                            }
                            else
                            {
                                if (item.Id != Process.GetCurrentProcess().Id)
                                    item.Kill();
                            }
                        }
                    }

                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 打开更新程序
        /// Author:Johnny Wong
        /// Time:2013-08-24
        /// </summary>
        /// <param name="path">更新程序路径</param>
        /// <returns></returns>
        public bool OpenUpdate(string path)
        {
            try
            {
                Process.Start(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新文件
        /// Author:Johnny Wong
        /// Time:2013-08-26
        /// LastEditTime:2016-07-15
        /// <param name="remoteHost">IP地址或域名</param>
        /// <param name="shareName">共享名</param>
        /// <param name="userName">帐号</param>
        /// <param name="passWord">密码</param>
        /// <param name="bgw">BackgroundWorker实体</param>
        /// </summary>
        /// <returns></returns>
        public bool UpdateFile(string remoteHost, string shareName, string userName, string passWord, BackgroundWorker bgw)
        {
            try
            {
                FileUpDownHelper.Connect(remoteHost, shareName, userName, passWord);
                if (FileHelper.IsExistDirectory(_winformPath + "Temp\\"))
                {
                    FileHelper.DeleteFolder(_winformPath + "Temp\\");
                }
                FileHelper.CreateFolder(_winformPath + "Temp\\");
                var fl = XMLHelper.GetIts(_configPath, _updateVersion);
                bool isS = true;
                string localPath = "";
                string newFile = "";
                string path = "";
                string operate = "";
                int time = 0;
                int all = fl.Length + 4;
                foreach (string ss in fl)
                {
                    if (isS)
                    {
                        if (!string.IsNullOrWhiteSpace(ss))
                        {
                            operate = ss.Substring(ss.IndexOf("@") + 1);
                            if (operate == "add")
                            {
                                path = _winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@"));
                                FileHelper.CreateFolder(path);
                            }
                            if (operate == "remove")
                            {
                                path = _winformPath + ss.Substring(0, ss.IndexOf("@"));
                                FileHelper.DeleteFolder(path);
                            }
                        }
                        isS = false;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(ss))
                        {
                            localPath = (_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@"))).Replace("\\", "/");
                            newFile = "\\\\" + _serverAddress + "\\" + ss.Substring(ss.IndexOf("@") + 1);
                            if (newFile == "\\\\" + _serverAddress + "\\" + "remove")
                            {
                                FileHelper.DeleteFile(_winformPath + "\\" + ss.Substring(0, ss.IndexOf("@")));
                            }
                            else
                            {
                                FileHelper.CopyFile(newFile, localPath);
                            }
                        }
                        isS = true;
                    }
                    time += 1;
                    bgw.ReportProgress(Convert.ToInt32(Convert.ToDouble(time) / Convert.ToDouble(all) * 100));
                }
                FileHelper.CopyFolder(_winformPath + "Temp\\", _winformPath);
                time += 1;
                bgw.ReportProgress(Convert.ToInt32(Convert.ToDouble(time) / Convert.ToDouble(all) * 100));
                FileHelper.DeleteFolder(_winformPath + "Temp\\");
                time += 1;
                bgw.ReportProgress(Convert.ToInt32(Convert.ToDouble(time) / Convert.ToDouble(all) * 100));
                string updateInfo = XMLHelper.GetItemValueByKey(_configPath, "version", "updateInfo");
                time += 1;
                bgw.ReportProgress(Convert.ToInt32(Convert.ToDouble(time) / Convert.ToDouble(all) * 100));
                FileHelper.AppendText(_updateLogPath, "\r\n" + updateInfo + "\r\n\r\n");
                time += 1;
                bgw.ReportProgress(Convert.ToInt32(Convert.ToDouble(time) / Convert.ToDouble(all) * 100));
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新文件
        /// Author:Johnny Wong
        /// Time:2015-03-06
        /// LastEditTime:2019-03-22
        /// <param name="bgw">BackgroundWorker实体</param>
        /// <param name="pb">进度条实体</param>
        /// </summary>
        /// <returns></returns>
        public bool UpdateFile(BackgroundWorker bgw, ProgressBar pb)
        {
            _pbUpdate = pb;
            Action<int> pbMax = (x) => { pb.Maximum = x; };
            Action<int> pbNow = (x) => { pb.Value = x; };
            pb.Invoke(pbMax, 1);
            pb.Invoke(pbNow, 1);
            try
            {
                if (FileHelper.IsExistDirectory(_winformPath + "Temp\\"))
                {
                    FileHelper.DeleteFolder(_winformPath + "Temp\\");
                }
                FileHelper.CreateFolder(_winformPath + "Temp\\");
                var fl = XMLHelper.GetIts(_configPath, _updateVersion);
                bool isS = true;
                string localPath = "";
                string newFile = "";
                string operateFolder = "";
                string operateFile = "";
                string operateNewFile = "";
                string[] files = null;
                int index = -1;
                int time = 0;
                int all = fl.Length + 1;
                WebClient wc = null;
                HttpWebRequest request = null;
                WebResponse response = null;

                //查看是否需要更新启动器
                foreach (string ss in fl)
                {
                    if (isS)
                    {
                        isS = false;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(ss))
                        {
                            operateFile = ss.Substring(ss.IndexOf("@") + 1);
                            localPath = (_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@"))).Replace("\\", "/");
                            newFile = _serverAddress + operateFile;
                            if (operateFile.IndexOf("new") == 0)
                            {
                                //更新启动器--调试时需注释
                                if (operateFile.IndexOf("new") == 0 && Convert.ToInt32(ss.Substring("KentCraft启动器 v".Length, 6)) > Convert.ToInt32(Application.ExecutablePath.Substring(Application.ExecutablePath.IndexOf("KentCraft启动器 v") + "KentCraft启动器 v".Length, 6)))
                                {
                                    wc = new WebClient();
                                    wc.DownloadProgressChanged += WcDownloadProgressChanged;
                                    wc.DownloadFileCompleted += WcDownloadFileCompleted;
                                    wc.DownloadFileAsync(new Uri(newFile.Replace("new@", "")), localPath);//如果非URI,则更新启动器
                                    _mre = new ManualResetEvent(false);
                                    _mre.WaitOne();
                                    //如果Temp下已有更新器,则删除该更新器
                                    if (localPath.Contains("KentCraft启动器 v"))
                                    {
                                        var tempFile = FileHelper.GetFileNames(_winformPath + "Temp\\", "KentCraft启动器 v*.exe", false);
                                        if (tempFile.Length > 1)
                                            FileHelper.DeleteFile(tempFile.First());
                                        throw new Exception();
                                    }
                                }
                            }

                        }
                        isS = true;
                    }
                }

                foreach (string ss in fl)
                {
                    pb.Invoke(pbNow, 0);
                    //文件夹操作
                    if (isS)
                    {
                        if (!string.IsNullOrWhiteSpace(ss))
                        {
                            operateFolder = ss.Substring(ss.IndexOf("@") + 1);
                            //创建文件夹
                            if (operateFolder == "add")
                            {
                                FileHelper.CreateFolder(_winformPath + ss.Substring(0, ss.IndexOf("@")));
                                FileHelper.CreateFolder(_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")));
                            }
                            //移除文件夹
                            else if (operateFolder == "remove")
                            {
                                FileHelper.DeleteFolder(_winformPath + ss.Substring(0, ss.IndexOf("@")));
                                FileHelper.DeleteFolder(_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")));
                            }
                            //移动文件夹
                            else if (operateFolder.Contains("cut") && operateFolder.Contains("@"))
                            {
                                FileHelper.MoveFolder(_winformPath + ss.Substring(0, ss.IndexOf("@")), _winformPath + ss.Substring(ss.LastIndexOf("@") + 1));
                                FileHelper.MoveFolder(_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")), _winformPath + "Temp\\" + ss.Substring(ss.LastIndexOf("@") + 1));
                            }
                            //复制文件夹
                            else if (operateFolder.Contains("copy") && operateFolder.Contains("@"))
                            {
                                FileHelper.CopyFolder(_winformPath + ss.Substring(0, ss.IndexOf("@")), _winformPath + ss.Substring(ss.LastIndexOf("@") + 1));
                                FileHelper.CopyFolder(_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")), _winformPath + "Temp\\" + ss.Substring(ss.LastIndexOf("@") + 1));
                            }
                            //备份文件夹
                            else if (operateFolder == "backup")
                            {
                                FileHelper.CopyFolder(_winformPath + ss.Substring(0, ss.IndexOf("@")), _winformPath + "Backup\\" + ss.Substring(0, ss.IndexOf("@")));
                                FileHelper.CopyFolder(_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")), _winformPath + "Temp\\Backup\\" + ss.Substring(0, ss.IndexOf("@")));
                            }
                            //还原文件夹
                            else if (operateFolder == "restore")
                            {
                                FileHelper.CopyFolder(_winformPath + "Backup\\" + ss.Substring(0, ss.IndexOf("@")), _winformPath + ss.Substring(0, ss.IndexOf("@")));
                                FileHelper.CopyFolder(_winformPath + "Temp\\Backup\\" + ss.Substring(0, ss.IndexOf("@")), _winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")));
                            }
                            //重命名文件夹
                            else if (operateFolder.Contains("rename") && operateFolder.Contains("@"))
                            {
                                FileHelper.MoveFolder(_winformPath + ss.Substring(0, ss.IndexOf("@")), _winformPath + ss.Substring(ss.LastIndexOf("@") + 1));
                                FileHelper.MoveFolder(_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")), _winformPath + "Temp\\" + ss.Substring(ss.LastIndexOf("@") + 1));
                            }
                            //如果没有对应操作,则更新启动器
                            else
                            {
                                throw new Exception();
                            }
                        }
                        isS = false;
                        pb.Invoke(pbMax, 1);
                        pb.Invoke(pbNow, 1);
                    }
                    //文件操作
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(ss))
                        {
                            operateFile = ss.Substring(ss.IndexOf("@") + 1);
                            localPath = (_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@"))).Replace("\\", "/");
                            newFile = _serverAddress + operateFile;
                            //移除文件(md5)
                            if (operateFile == "remove" && FileHelper.IsMd5(ss.Substring(0, ss.IndexOf("@"))))
                            {
                                if (ss.IndexOf("\\") > -1)
                                {
                                    index = ss.LastIndexOf("\\");
                                }
                                else
                                {
                                    index = -1;
                                }

                                if (FileHelper.IsExistDirectory(_winformPath + ss.Substring(0, index == -1 ? 0 : index)))
                                {
                                    files = FileHelper.GetFileNames(_winformPath + ss.Substring(0, index == -1 ? 0 : index));
                                    foreach (string file in files)
                                    {
                                        if (FileHelper.GetMd5(file) == ss.Substring(index + 1, 32))
                                        {
                                            FileHelper.DeleteFile(file);
                                            break;
                                        }
                                    }
                                }
                                if (FileHelper.IsExistDirectory(_winformPath + "Temp\\" + ss.Substring(0, index == -1 ? 0 : index)))
                                {
                                    files = FileHelper.GetFileNames(_winformPath + "Temp\\" + ss.Substring(0, index == -1 ? 0 : index));
                                    foreach (string file in files)
                                    {
                                        if (FileHelper.GetMd5(file) == ss.Substring(index + 1, 32))
                                        {
                                            FileHelper.DeleteFile(file);
                                            break;
                                        }
                                    }
                                }
                            }
                            //移除文件
                            else if (operateFile == "remove")
                            {
                                FileHelper.DeleteFile(_winformPath + ss.Substring(0, ss.IndexOf("@")));
                                FileHelper.DeleteFile(_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")));
                                pb.Invoke(pbMax, 1);
                                pb.Invoke(pbNow, 1);
                            }
                            //移动文件
                            else if (operateFile.Contains("cut") && operateFile.Contains("@"))
                            {
                                FileHelper.MoveFile(_winformPath + ss.Substring(0, ss.IndexOf("@")), _winformPath + ss.Substring(ss.LastIndexOf("@") + 1));
                                FileHelper.MoveFile(_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")), _winformPath + "Temp\\" + ss.Substring(ss.LastIndexOf("@") + 1));
                                pb.Invoke(pbMax, 1);
                                pb.Invoke(pbNow, 1);
                            }
                            //复制文件
                            else if (operateFile.Contains("copy") && operateFile.Contains("@"))
                            {
                                FileHelper.CopyFile(_winformPath + ss.Substring(0, ss.IndexOf("@")), _winformPath + ss.Substring(ss.LastIndexOf("@") + 1));
                                FileHelper.CopyFile(_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")), _winformPath + "Temp\\" + ss.Substring(ss.LastIndexOf("@") + 1));
                                pb.Invoke(pbMax, 1);
                                pb.Invoke(pbNow, 1);
                            }
                            //备份文件
                            else if (operateFile == "backup")
                            {
                                FileHelper.CopyFile(_winformPath + ss.Substring(0, ss.IndexOf("@")), _winformPath + "Backup\\" + ss.Substring(0, ss.IndexOf("@")));
                                FileHelper.CopyFile(_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")), _winformPath + "Temp\\Backup\\" + ss.Substring(0, ss.IndexOf("@")));
                                pb.Invoke(pbMax, 1);
                                pb.Invoke(pbNow, 1);
                            }
                            //还原文件
                            else if (operateFile == "restore")
                            {
                                FileHelper.CopyFile(_winformPath + "Backup\\" + ss.Substring(0, ss.IndexOf("@")), _winformPath + ss.Substring(0, ss.IndexOf("@")));
                                FileHelper.CopyFile(_winformPath + "Temp\\Backup\\" + ss.Substring(0, ss.IndexOf("@")), _winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")));
                                pb.Invoke(pbMax, 1);
                                pb.Invoke(pbNow, 1);
                            }
                            //重命名文件
                            else if (operateFile.Contains("rename") && operateFile.Contains("@"))
                            {
                                //根据md5重命名文件
                                if (FileHelper.IsMd5(ss.Substring(0, ss.IndexOf("@"))))
                                {
                                    if (ss.IndexOf("\\") > -1)
                                    {
                                        index = ss.LastIndexOf("\\");
                                    }
                                    else
                                    {
                                        index = -1;
                                    }
                                    if (FileHelper.IsExistDirectory(_winformPath + ss.Substring(0, index == -1 ? 0 : index)))
                                    {
                                        files = FileHelper.GetFileNames(_winformPath + ss.Substring(0, index == -1 ? 0 : index));
                                        foreach (string file in files)
                                        {
                                            if (FileHelper.GetMd5(file) == ss.Substring(index + 1, 32))
                                            {
                                                FileHelper.MoveFile(file, FileHelper.GetDirectoryName(file) + "\\" + ss.Substring(ss.LastIndexOf("@") + 1));
                                                break;
                                            }
                                        }
                                    }
                                    if (FileHelper.IsExistDirectory(_winformPath + "Temp\\" + ss.Substring(0, index == -1 ? 0 : index)))
                                    {
                                        files = FileHelper.GetFileNames(_winformPath + "Temp\\" + ss.Substring(0, index == -1 ? 0 : index));
                                        foreach (string file in files)
                                        {
                                            if (FileHelper.GetMd5(file) == ss.Substring(index + 1, 32))
                                            {
                                                FileHelper.MoveFile(file, FileHelper.GetDirectoryName(file) + "\\" + ss.Substring(ss.LastIndexOf("@") + 1));
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    FileHelper.MoveFile(_winformPath + ss.Substring(0, ss.IndexOf("@")), _winformPath + ss.Substring(ss.LastIndexOf("@") + 1));
                                    FileHelper.MoveFile(_winformPath + "Temp\\" + ss.Substring(0, ss.IndexOf("@")), _winformPath + "Temp\\" + ss.Substring(ss.LastIndexOf("@") + 1));
                                }
                                pb.Invoke(pbMax, 1);
                                pb.Invoke(pbNow, 1);
                            }
                            //下载文件
                            else
                            {
                                //如果下载文件包含其他操作
                                if (operateFile.Contains("@"))
                                {
                                    operateNewFile = operateFile.Substring(operateFile.IndexOf("@") + 1);
                                    newFile = newFile.Substring(0, newFile.IndexOf("@"));
                                }
                                else
                                {
                                    operateNewFile = "";
                                }

                                //验证下载文件大小
                                request = (HttpWebRequest)System.Net.WebRequest.Create(newFile);
                                response = (HttpWebResponse)request.GetResponse();

                                //下载文件
                                wc = new WebClient();
                                wc.DownloadProgressChanged += WcDownloadProgressChanged;
                                wc.DownloadFileCompleted += WcDownloadFileCompleted;
                                wc.DownloadFileAsync(new Uri(newFile), localPath);//如果非URI,则更新启动器
                                _mre = new ManualResetEvent(false);
                                _mre.WaitOne();

                                Thread.Sleep(777); //不短暂暂停会导致提前解压

                                if (FileHelper.GetFileSize(localPath) != response.ContentLength)
                                {
                                    throw new Exception("文件未成功下载,请重启更新器");
                                }

                                //有需要解压的文件则进行解压
                                //if (localPath.Contains("需解压"))

                                //需解压
                                if (operateNewFile == "unzip")
                                {
                                    pb.Invoke(pbNow, 0);
                                    CompressHelper.Uncompress(localPath, _winformPath + "Temp\\", pb);
                                    FileHelper.DeleteFile(localPath);
                                }

                                //如果Temp下已有更新器,则删除该更新器
                                if (localPath.Contains("KentCraft启动器 v"))
                                {
                                    var tempFile = FileHelper.GetFileNames(_winformPath + "Temp\\", "KentCraft启动器 v*.exe", false);
                                    if (tempFile.Length > 1)
                                        FileHelper.DeleteFile(tempFile.First());
                                }
                            }
                        }
                        isS = true;
                    }
                    time++;
                    bgw.ReportProgress(Convert.ToInt32(Convert.ToDouble(time) / Convert.ToDouble(all) * 100));
                }
                pb.Invoke(pbNow, 0);
                pb.Invoke(pbMax, 4);
                FileHelper.CopyFolder(_winformPath + "Temp\\", _winformPath, "KentCraft启动器 v");
                pb.Invoke(pbNow, 1);
                //删除目录失败不影响整体更新
                try
                {
                    FileHelper.DeleteFolder(_winformPath + "Temp\\");
                }
                catch (Exception) { }
                pb.Invoke(pbNow, 2);
                string updateInfo = XMLHelper.GetItemsValue(_configPath, "version", _updateVersion) + "\r\n\r\n\r\n";
                pb.Invoke(pbNow, 3);
                FileHelper.AppendText(_updateLogPath, updateInfo);
                pb.Invoke(pbNow, 4);
                time++;
                bgw.ReportProgress(Convert.ToInt32(Convert.ToDouble(time) / Convert.ToDouble(all) * 100));
            }
            catch (Exception e)
            {
                //如果更新出错,则有可能是更新器不是最新版本
                var tempFile = FileHelper.GetFileNames(_winformPath + "Temp\\", "KentCraft启动器 v*.exe", false);
                if (tempFile.Length == 1)
                {
                    //移动更新器失败是因为目录下已有更新器文件,并不影响更新器更新
                    try
                    {
                        FileHelper.MoveFile(tempFile.First(), _winformPath + FileHelper.GetFileName(tempFile.First()));
                    }
                    catch (Exception)
                    {

                    }
                    //打开更新的更新器
                    Process.Start(FileHelper.GetFileNames(_winformPath, "KentCraft启动器 v*.exe", false).First());
                    Application.Exit();
                }
                else
                {
                    MessageBox.Show(e.Message, "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// 进度条实时更新
        /// Author:Johnny Wong
        /// Time:2017-12-29
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WcDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Action<DownloadProgressChangedEventArgs> Onchange = ProceChange;
            Onchange.Invoke(e);
        }

        /// <summary>
        /// 进度变更
        /// Author:Johnny Wong
        /// Time:2017-12-29
        /// LastEditTime:2018-01-04
        /// </summary>
        /// <param name="e"></param>
        private void ProceChange(DownloadProgressChangedEventArgs e)
        {
            _pbUpdate.Invoke(new Action(() => { _pbUpdate.Maximum = Convert.ToInt32(e.TotalBytesToReceive) + 1; }));
            _pbUpdate.Invoke(new Action(() =>
            {
                try
                {
                    _pbUpdate.Value = Convert.ToInt32(e.BytesReceived + 1);
                }
                catch (Exception) { }
            }));
        }

        /// <summary>
        /// 下载完成
        /// Author:Johnny Wong
        /// Time:2017-12-29
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WcDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            _mre.Set();
        }

        /// <summary>
        /// Author:Johnny Wong
        /// Time:2013-08-27
        /// 打开更新日志文件
        /// </summary>
        /// <returns></returns>
        public bool OpenUpdateLog()
        {
            return OpenUpdate(_updateLogPath);
        }
    }
}
