using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Jnw.Common;

namespace KentCraftAutoUpdater
{
    public partial class CheckUpdate2 : GenericUpdate
    {
        private const string ServerAddress = "";
        private const string ConfigPath = "";
        //private const string ConfigPath = @"E:\编程\C#\制作\我的世界\updateconfig.xml";
        private const string UpdateLogPath = "更新日志.txt";
        private const string TipString = "版本号:";
        private readonly static string WinformPath = Environment.CurrentDirectory + "\\";

        public CheckUpdate2()
            : base(ServerAddress, ConfigPath, UpdateLogPath, TipString, WinformPath)
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗体加载时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CheckUpdate_Load(object sender, EventArgs e)
        {
            try
            {
                //杀掉更新器进程
                Process[] pcs = Process.GetProcesses();
                foreach (Process item in pcs)
                {
                    if (item.ProcessName.Contains("KentCraft"))
                    {
                        if (item.Id != Process.GetCurrentProcess().Id)
                            item.Kill();
                    }
                }
                //最新的更新器
                string newest = "";
                //获取所有更新器
                var newExe = FileHelper.GetFileNames(WinformPath, "KentCraft启动器 v*.exe", false);
                //获取最新更新器
                foreach (var temp in newExe)
                {
                    if (newest == "" || Convert.ToInt32(temp.Substring((WinformPath + "KentCraft启动器 v").Length, 6)) > Convert.ToInt32(newest.Substring((WinformPath + "KentCraft启动器 v").Length, 6)))
                    {
                        newest = temp;
                    }
                }
                //如果有多个更新器,则打开最新的更新器(不包含自身)
                if (newExe.Length > 1 && !Application.ExecutablePath.Contains(FileHelper.GetFileName(newest)))
                {
                    Process.Start(newest);
                    Application.Exit();
                    ManualResetEvent _mr1e = new ManualResetEvent(false);
                    _mr1e.WaitOne();
                }
                //一直到正确移除老启动器
                //MessageBox.Show(newExe.Length.ToString());
                //MessageBox.Show(Application.ExecutablePath);
                //MessageBox.Show(FileHelper.GetFileName(newest));
                //MessageBox.Show(Application.ExecutablePath.Contains(FileHelper.GetFileName(newest)).ToString());
                while (newExe.Length > 1 && Application.ExecutablePath.Contains(FileHelper.GetFileName(newest)))
                {
                    //MessageBox.Show(newExe.Length.ToString());
                    //移除所有比该更新器版本低的更新器
                    MessageBox.Show("点确定移除过时启动器", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);//必须有这行代码否则无法删除
                    var exe = FileHelper.GetFileNames(WinformPath, "KentCraft启动器 v*.exe", false).Where(i =>
                    Convert.ToInt32(i.Substring((WinformPath + "KentCraft启动器 v").Length, 6)) <=
                    Convert.ToInt32(Application.ExecutablePath.Substring((WinformPath + "KentCraft启动器 v").Length, 6)));
                    foreach (var temp in exe)
                    {
                        if (!Application.ExecutablePath.Contains(FileHelper.GetFileName(temp)))
                            FileHelper.DeleteFile(temp);
                    }
                    newExe = FileHelper.GetFileNames(WinformPath, "KentCraft启动器 v*.exe", false);
                }
                //删除冗余文件
                FileHelper.DeleteFolder(WinformPath + "Temp\\");
            }
            catch (Exception) { }
            base.CheckUpdate_Load(sender, e, 1);
            try
            {
                //获取更新内容
                int updateVersion = 0;
                string newVersion = XMLHelper.GetItemValue(ConfigPath, "version", updateVersion).Trim();
                if (!FileHelper.IsExistFile(UpdateLogPath))
                    FileHelper.CreateFile(UpdateLogPath);
                var updateInfo = FileHelper.GetContent(UpdateLogPath);
                string localVersion = "0";
                for (int i = updateInfo.Length - 1; i >= 0; i--)
                {
                    if (updateInfo[i].IndexOf(TipString) != -1)
                    {
                        localVersion = updateInfo[i].Replace(TipString, "").Trim();
                        break;
                    }
                }
                if (newVersion != localVersion)
                {
                    int versionCount = XMLHelper.GetItemCount(ConfigPath, "version");
                    while (newVersion != localVersion && updateVersion + 1 < versionCount)
                    {
                        newVersion = XMLHelper.GetItemValue(ConfigPath, "version", ++updateVersion);
                    }
                    if (localVersion != "0")
                        updateVersion--;
                }
                //var tip = XMLHelper.GetItemsValue(ConfigPath, "version", updateVersion);
                //label2.Text = tip.Substring(0, tip.Length - 6);
                var tip = XMLHelper.GetItemsValue(ConfigPath, "version", updateVersion).Replace("\r\n\r\n", "\r\n").Replace("\r\n"," \r\n");
                label2.Text = tip.IndexOf("更新时间") != tip.LastIndexOf("更新时间") ? tip : tip.Replace("更新时间", "\r\n更新时间");
                //使整个窗体根据屏幕居中
                SetBounds((Screen.GetBounds(this).Width / 2) - Width / 2, (Screen.GetBounds(this).Height / 2) - (Height / 2), Width, Height, BoundsSpecified.Location);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
        }

        #region 使panel不包含横向滚动条

        void panel1_Paint(object sender, PaintEventArgs e)
        {
            Control _Control = (Control)sender;
            ShowScrollBar(_Control.Handle, 0, 0);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int ShowScrollBar(IntPtr hWnd, int bar, int show);

        #endregion
        
        /// <summary>
        /// 下次再说
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void btnCancel2_Click(object sender, EventArgs e)
        {
            base.btnCancel2_Click(sender, e);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void btnConfirm_Click(object sender, EventArgs e)
        {
            base.btnConfirm_Click(sender, e);
        }

        /// <summary>
        /// 链接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Process.Start("https://discord.gg/jMsptMy");

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.notion.so/johnnywoong/21-44c31c444f364dfeba39bb16c8f358cc");
        }
    }
}
