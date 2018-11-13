using System;

namespace winform自动更新
{
    public partial class CheckUpdate : GenericUpdate
    {
        private const string ServerAddress = "http://192.168.1.1:1000/FileShare/";
        private const string ConfigPath = "http://192.168.1.1:1000/UpdateConfig.xml";
        private const string UpdateLogPath = "更新日志.txt";
        private const string TipString = "版本号:";
        private readonly static string WinformPath = Environment.CurrentDirectory + "\\";

        public CheckUpdate()
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
            base.CheckUpdate_Load(sender, e, 2);
        }

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
    }
}
