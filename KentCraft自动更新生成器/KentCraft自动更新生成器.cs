using Jnw.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace KentCraft自动更新生成器
{
    public partial class KentCraft自动更新生成器 : Form
    {
        public KentCraft自动更新生成器()
        {
            InitializeComponent();
            textBox2.Text = DateTime.Now.ToString("yyMMdd");
            richTextBox1.Text = richTextBox1.Text.Replace("yyyy-MM-dd", DateTime.Now.ToString("yyyy-MM-dd"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder result = new StringBuilder();
                string tempStr = "";
                //更新头
                result.Append("<version value=\"" + textBox1.Text + "\">");
                result.Append("\n<item key=\"updateInfo\" value=\"版本号: " + textBox1.Text);
                result.Append("\n" + richTextBox1.Text + "\">\n</item>");
                result.Append("\n<items>");

                //更新文件
                tempStr = "";
                var updateFile =
                    FileHelper.GetFileNames(Environment.CurrentDirectory + "\\" + textBox2.Text + "\\更新",
                        "*", true);
                foreach (var temp in updateFile)
                {
                    tempStr = "\n<it key=\"\" value=\"" + temp.Substring(Environment.CurrentDirectory.Length + 11) +
                              "@" + textBox2.Text + "/" +
                              "更新/" + temp.Substring(Environment.CurrentDirectory.Length + 11).Replace("\\", "/") + "\"></it>" + tempStr;
                }
                result.Append(tempStr);

                ////新增文件
                //tempStr = "";
                //var addFile = FileHelper.GetFileNames(Environment.CurrentDirectory + "\\新增", "*", true);
                //foreach (var temp in addFile)
                //{
                //    tempStr = "\n<it key=\"\" value=\"" + temp.Substring(Environment.CurrentDirectory.Length + 4) + "@" + textBox2.Text + "/" + HttpUtility.UrlPathEncode(FileHelper.GetFileName(temp)) + "\"></it>" + tempStr;
                //}
                //result.Append(tempStr);

                //添加目录
                tempStr = "";
                var updateFolder =
                    FileHelper.GetDirectories(Environment.CurrentDirectory + "\\" + textBox2.Text + "\\更新",
                        "*", true);
                //var addFolder = FileHelper.GetDirectories(Environment.CurrentDirectory + "\\新增", "*", true);
                var folder = new List<string>();
                foreach (var temp in updateFolder)
                {
                    if (!folder.Contains(temp.Substring(Environment.CurrentDirectory.Length + 11)) && temp.Substring(Environment.CurrentDirectory.Length + 11) != "Backup")
                        folder.Add(temp.Substring(Environment.CurrentDirectory.Length + 11));
                }
                //foreach (var temp in addFolder)
                //{
                //    if (!folder.Contains(temp.Substring(Environment.CurrentDirectory.Length + 4)))
                //        folder.Add(temp.Substring(Environment.CurrentDirectory.Length + 4));
                //}
                foreach (var temp in folder)
                {
                    tempStr = "\n<it key=\"" + temp + "@add\" value=\"\"></it>" + tempStr;
                }
                result.Append(tempStr);

                //移除目录
                tempStr = "";
                var removeFolder =
                    FileHelper.GetDirectories(Environment.CurrentDirectory + "\\" + textBox2.Text + "\\移除",
                        "*", true);
                foreach (var temp in removeFolder)
                {
                    if (FileHelper.IsEmptyDirectory(temp))
                        tempStr = "\n<it key=\"" + temp.Substring(Environment.CurrentDirectory.Length + 11) +
                                  "@remove\" value=\"\"></it>" + tempStr;
                }
                result.Append(tempStr);

                //移除文件
                tempStr = "";
                var removeFile =
                    FileHelper.GetFileNames(Environment.CurrentDirectory + "\\" + textBox2.Text + "\\移除",
                        "*", true);
                if (cbkMd5.Checked)
                {
                    string tt;
                    foreach (var temp in removeFile)
                    {
                        if (FileHelper.GetDirectoryName(temp).Length < Environment.CurrentDirectory.Length + 11)
                        {
                            tt = FileHelper.GetDirectoryName(temp).Substring(Environment.CurrentDirectory.Length + 10);
                        }
                        else
                        {
                            tt = FileHelper.GetDirectoryName(temp).Substring(Environment.CurrentDirectory.Length + 11);
                        }
                        tempStr = string.Format("\n<it key=\"\" value=\"{0}{1}@remove\"></it>{2}", tt, tt != "" ? "\\" + FileHelper.GetMd5(temp) : FileHelper.GetMd5(temp), tempStr);
                    }
                }
                else
                {
                    foreach (var temp in removeFile)
                    {
                        tempStr = "\n<it key=\"\" value=\"" + temp.Substring(Environment.CurrentDirectory.Length + 11) +
                                  "@remove\"></it>" + tempStr;
                    }
                }
                result.Append(tempStr);

                //备份文件
                tempStr = "";
                var backupFile =
                    FileHelper.GetFileNames(Environment.CurrentDirectory + "\\" + textBox2.Text + "\\备份",
                        "*", true);
                foreach (var temp in backupFile)
                {
                    tempStr = "\n<it key=\"\" value=\"" + temp.Substring(Environment.CurrentDirectory.Length + 11) +
                              "@backup\"></it>" + tempStr;
                }
                result.Append(tempStr);

                //备份目录
                tempStr = "";
                var backupFolder =
                    FileHelper.GetDirectories(Environment.CurrentDirectory + "\\" + textBox2.Text + "\\备份",
                        "*", true);
                foreach (var temp in backupFolder)
                {
                    if (FileHelper.IsEmptyDirectory(temp))
                        tempStr = "\n<it key=\"" + temp.Substring(Environment.CurrentDirectory.Length + 11) +
                              "@backup\" value=\"\"></it>" + tempStr;
                    else
                        tempStr = "\n<it key=\"Backup\\" + temp.Substring(Environment.CurrentDirectory.Length + 11) + "@add\" value=\"\"></it>" + tempStr;
                }
                result.Append(tempStr);

                //是否添加备份目录
                tempStr = "";
                if (!FileHelper.IsEmptyDirectory(Environment.CurrentDirectory + "\\" + textBox2.Text + "\\备份"))
                {
                    tempStr = "\n<it key=\"Backup@add\" value=\"\"></it>";
                }
                result.Append(tempStr);

                //还原文件
                tempStr = "";
                var restoreFile =
                    FileHelper.GetFileNames(Environment.CurrentDirectory + "\\" + textBox2.Text + "\\还原",
                        "*", true);
                foreach (var temp in restoreFile)
                {
                    tempStr = "\n<it key=\"\" value=\"" + temp.Substring(Environment.CurrentDirectory.Length + 11) +
                              "@restore\"></it>" + tempStr;
                }
                result.Append(tempStr);

                //还原目录
                tempStr = "";
                var restoreFolder =
                    FileHelper.GetDirectories(Environment.CurrentDirectory + "\\" + textBox2.Text + "\\还原",
                        "*", true);
                foreach (var temp in restoreFolder)
                {
                    if (FileHelper.IsEmptyDirectory(temp))
                        tempStr = "\n<it key=\"" + temp.Substring(Environment.CurrentDirectory.Length + 11) +
                              "@restore\" value=\"\"></it>" + tempStr;
                    else
                        tempStr = "\n<it key=\"" + temp.Substring(Environment.CurrentDirectory.Length + 11) + "@add\" value=\"\"></it>" + tempStr;
                }
                result.Append(tempStr);

                //更新尾
                result.Append("\n</items>");
                result.Append("\n</version>");
                richTextBox2.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
