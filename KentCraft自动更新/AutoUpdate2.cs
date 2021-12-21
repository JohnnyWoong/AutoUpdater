using System;
using System.ComponentModel;

namespace KentCraftAutoUpdater
{
    public partial class AutoUpdate2 : GenericUpdate
    {
        private readonly static string ServerAddress = Jnw.Common.FileHelper.GetContent("KCLconfig.ini")[2];
        private readonly static string ConfigPath = Jnw.Common.FileHelper.GetContent("KCLconfig.ini")[2] + "updateconfig.xml";
        //private const string ConfigPath = @"E:\编程\C#\制作\我的世界\updateconfig.xml";
        private readonly static string UpdateLogPath = Jnw.Common.FileHelper.GetContent("KCLconfig.ini")[3];
        private readonly static string WinformPath = Environment.CurrentDirectory + "\\";

        public AutoUpdate2()
            : base(ServerAddress, ConfigPath, UpdateLogPath, WinformPath)
        {
            InitializeComponent();
            SetControls(bgwUpdate, pbUpdate, pbUpdateAll);
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
