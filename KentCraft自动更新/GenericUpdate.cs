using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace winform自动更新
{
    public class GenericUpdate : Form
    {
        private readonly string _serverAddress;
        private readonly string _configPath;
        private readonly string _updateLogPath;
        private readonly string _winformPath;
        private readonly string _tipString;


        protected bool IsSuccess;

        protected BackgroundWorker BgwUpdate;
        protected ProgressBar PbUpdate;
        protected ProgressBar PbUpdateAll;

        protected void SetControls(BackgroundWorker bgwUpdate, ProgressBar pbUpdate, ProgressBar pbUpdateAll)
        {
            BgwUpdate = bgwUpdate;
            PbUpdate = pbUpdate;
            PbUpdateAll = pbUpdateAll;
        }

        public GenericUpdate()
        {

        }

        protected GenericUpdate(string serverAddress, string configPath, string updateLogPath, string winformPath)
        {
            //_serverAddress = serverAddress.Replace("1710",Jnw.Common.FileHelper.GetContent(Environment.CurrentDirectory + "\\KentCraft.config")[0]);
            //_configPath = configPath.Replace("1710", Jnw.Common.FileHelper.GetContent(Environment.CurrentDirectory + "\\KentCraft.config")[0]);
            _serverAddress = serverAddress;
            _configPath = configPath;
            _updateLogPath = updateLogPath;
            _winformPath = winformPath;
        }

        protected GenericUpdate(string serverAddress, string configPath, string updateLogPath, string tipString, string winformPath)
        {
            //_serverAddress = serverAddress.Replace("1710",Jnw.Common.FileHelper.GetContent(Environment.CurrentDirectory + "\\KentCraft.config")[0]);
            //_configPath = configPath.Replace("1710", Jnw.Common.FileHelper.GetContent(Environment.CurrentDirectory + "\\KentCraft.config")[0]);
            _serverAddress = serverAddress;
            _configPath = configPath;
            _updateLogPath = updateLogPath;
            _tipString = tipString;
            _winformPath = winformPath;
        }


        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected virtual void AutoUpdate_Load(object sender, EventArgs e)
        {
            //激发DoWork事件
            BgwUpdate.RunWorkerAsync();
        }

        /// <summary>
        /// 执行线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        protected virtual void bgwUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgw = sender as BackgroundWorker;
            //允许报告进度
            bgw.WorkerReportsProgress = true;
            Jnw.Common.AutoUpdate au = GetJnWCommonAutoUpdateInstance();
            IsSuccess = au.UpdateFile(bgw, PbUpdateAll);
        }

        /// <summary>
        /// 进度改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void bgwUpdate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //更改进度
            PbUpdate.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// 线程结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void bgwUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                PromptErrorMessageBox(e);
            }
            else if (e.Cancelled)
            {
                PromptCancelMessageBox();
            }
            else
            {
                if (IsSuccess)
                {
                    PromptUpdateRootNotification();
                }
                else
                {
                    PromptUpdateFailMessageBox();
                }
                Application.Exit();
            }
        }

        private delegate void CancelCallback();


        /// <summary>
        /// 取消更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void btnCancel_Click(object sender, EventArgs e)
        {
            PromptCancelMessageBox(delegate ()
            {
                Jnw.Common.AutoUpdate au = GetJnWCommonAutoUpdateInstance();
                au.OpenUpdate("KentCraft.exe");//程序文件名
                Application.Exit();
            });
        }

        private Jnw.Common.AutoUpdate GetJnWCommonAutoUpdateInstance()
        {
            return new Jnw.Common.AutoUpdate(_serverAddress, _configPath, _updateLogPath, _winformPath);
        }

        private void PromptCancelMessageBox(CancelCallback cancelCallback)
        {
            DialogResult dr = MessageBox.Show("取消更新后可能造成无法正常进行游戏,您确定要取消更新吗", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                cancelCallback();
            }
        }

        private void PromptErrorMessageBox(RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(e.Error.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void PromptCancelMessageBox()
        {
            MessageBox.Show("您取消了更新", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void PromptUpdateRootNotification()
        {
            TopMost = false;
            DialogResult dr = MessageBox.Show("更新成功,是否立即查看更新日志?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            Jnw.Common.AutoUpdate au = GetJnWCommonAutoUpdateInstance();
            if (dr == DialogResult.Yes)
            {
                au.OpenUpdateLog();
            }
            //如果有多个启动器,则打开最新的启动器
            var exe = Jnw.Common.FileHelper.GetFileNames(_winformPath, "KentCraft启动器 v*.exe", false);
            if (exe.Length > 1)
            {
                string newExe = "";
                foreach (var temp in exe)
                {
                    if (newExe == "" ||
                        Convert.ToInt32(temp.Substring((_winformPath + "KentCraft启动器 v").Length, 6)) >
                        Convert.ToInt32(newExe.Substring((_winformPath + "KentCraft启动器 v").Length, 6)))
                        newExe = temp;
                }
                au.OpenUpdate(newExe);
            }
            else
            {
                au.OpenUpdate("KentCraft.exe");//程序文件名   
            }
        }

        private void PromptUpdateFailMessageBox()
        {
            MessageBox.Show("更新失败,请稍后再试", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
        }


        private delegate void NewestCallback();
        private delegate void CancelCallback2();


        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void CheckUpdate_Load(object sender, EventArgs e, int type)
        {
            Jnw.Common.AutoUpdate au = GetJnWCommonAutoUpdateInstance();
            au.KillProcess("KentCraft", true, false);
            //au.KillProcess("java", true);
            //au.KillProcess("javaw", true);
            //判断是否需要更新
            if (type == 1)
            {
                if (!au.CheckUpdate(_tipString))
                {
                    NewestCallback newestCallback = new NewestCallback(delegate ()
                    {
                        au.OpenUpdate("KentCraft.exe");//程序文件名
                        Application.Exit();
                    });
                    newestCallback();
                }
            }
            else if (type == 2)
            {
                if (!au.CheckUpdate("192.168.1.1", "FileShare", "LoginAccount", "LoginPassword", _tipString))
                {
                    NewestCallback newestCallback = new NewestCallback(() => Application.Exit());
                    newestCallback();
                }
            }
        }

        private void PromptCancelMessageBox2(CancelCallback2 cancelCallback)
        {
            var rs = MessageBox.Show("确定取消更新?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (rs == DialogResult.Yes)
            {
                cancelCallback();
            }
        }

        protected virtual void btnCancel2_Click(object sender, EventArgs e)
        {
            PromptCancelMessageBox2(delegate ()
            {
                Jnw.Common.AutoUpdate au = GetJnWCommonAutoUpdateInstance();
                au.OpenUpdate("KentCraft.exe");//程序文件名
                Application.Exit();
            });
        }

        private delegate void ConfirmCallback();

        private void PromptConfirmMessageBox(ConfirmCallback confirmCallback)
        {
            var rs = MessageBox.Show("自动更新将关闭当前游戏进程,是否继续?", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (rs == DialogResult.Yes)
            {
                confirmCallback();
            }
            else
            {
                return;
            }
        }

        protected virtual void btnConfirm_Click(object sender, EventArgs e)
        {
            Jnw.Common.AutoUpdate au = GetJnWCommonAutoUpdateInstance();
            //提示
            PromptConfirmMessageBox(delegate
            ()
            {
                //测试
                //au.KillProcess("FileName.vshost");
                au.KillProcess("KentCraft", true, false);
                au.KillProcess("java", true);
                au.KillProcess("javaw", true);

                this.Hide();
                AutoUpdate2 winFormau = new AutoUpdate2();
                winFormau.Show();
            });
        }
    }
}
