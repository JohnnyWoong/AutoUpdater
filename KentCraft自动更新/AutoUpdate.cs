using System;
using System.ComponentModel;

namespace KentCraftAutoUpdater
{
    public partial class AutoUpdate : GenericUpdate
    {
        private const string ServerAddress = "192.168.1.1";
        private const string ConfigPath = "\\\\192.168.1.1\\FileShare\\UpdateConfig.xml";
        private const string UpdateLogPath = "更新日志.txt";
        private readonly static string WinformPath = Environment.CurrentDirectory + "\\";

        public AutoUpdate()
            : base(ServerAddress, ConfigPath, UpdateLogPath, WinformPath)
        {
            InitializeComponent();
            SetControls(bgwUpdate, pbUpdate, PbUpdateAll);
        }

        protected override void AutoUpdate_Load(object sender, EventArgs e)
        {
            base.AutoUpdate_Load(sender, e);
        }


        protected override void bgwUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            base.bgwUpdate_DoWork(sender, e);

        }

        protected override void bgwUpdate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            base.bgwUpdate_ProgressChanged(sender, e);

        }

        protected override void bgwUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            base.bgwUpdate_RunWorkerCompleted(sender, e);

        }

        protected override void btnCancel_Click(object sender, EventArgs e)
        {
            base.btnCancel_Click(sender, e);
        }
    }
}
