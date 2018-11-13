using System;
using System.Windows.Forms;

namespace KentCraft自动更新生成器
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KentCraft自动更新生成器());
        }
    }
}
