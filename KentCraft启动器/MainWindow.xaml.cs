using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KentCraft启动器
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //调试需注释
            if (Assembly.GetEntryAssembly().Location.IndexOf("KentCraft启动器.exe") > -1)
            {
                Process.Start("kentcraft.exe");
                Application.Current.Shutdown();
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discord.gg/jMsptMy");
        }

        //19周目
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (true == cbk.IsChecked)
            {
                Process.Start("https://discord.gg/mZrsN3E");
            }
            if (IsExistFile("KentCraft_19.json"))
            {
                MoveFile("hmcl.json", "KentCraft_Sade.json");
                MoveFile("KentCraft_19.json", "hmcl.json");
                //MoveFile("hmcl.log", "KentCraft_Sade.log");
                //MoveFile("KentCraft_19.log", "hmcl.log");
            }
            Process.Start(Environment.CurrentDirectory + "\\KentCraft_19\\kentcraft.exe"); //调试需注释
            //Process.Start(Environment.CurrentDirectory + "\\KentCraft_19\\kentcraft.exe");
            Application.Current.Shutdown();
        }

        //Sade特别版
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (true == cbk.IsChecked)
            {
                Process.Start("https://discord.gg/AaMsA7w");
            }
            if (IsExistFile("KentCraft_Sade.json"))
            {
                MoveFile("hmcl.json", "KentCraft_19.json");
                MoveFile("KentCraft_Sade.json", "hmcl.json");
                //MoveFile("hmcl.log", "KentCraft_19.log");
                //MoveFile("KentCraft_Sade.log", "hmcl.log");
            }
            Process.Start(Environment.CurrentDirectory + "\\KentCraft_Sade\\KentCraft.exe"); //调试需注释
            //Process.Start(Environment.CurrentDirectory + "\\KentCraft_Sade\\kentcraft.exe");
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 移动文件(剪贴--粘贴)
        /// Author: Johnny Wong
        /// Date: 2018.05.23
        /// </summary>
        /// <param name="dir1">要移动的文件的路径及全名(包括后缀)</param>
        /// <param name="dir2">文件移动到新的位置,并指定新的文件名</param>
        public static void MoveFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(dir1))
            {
                if (File.Exists(dir2))
                {
                    DeleteFile(dir2);
                }
                File.Move(dir1, dir2);
            }
        }

        /// <summary>
        /// 检测指定文件是否存在,如果存在则返回true。
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="file">要删除的文件路径和名称</param>
        public static void DeleteFile(string file)
        {
            if (File.Exists(file))
                File.Delete(file);
        }
    }
}
