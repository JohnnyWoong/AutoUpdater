using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Zip;
using SharpCompress.Common;
using SharpCompress.Readers;

//需添加 SharpCompress 的引用
//By Johnny Wong
namespace Jnw.Common
{
    /// <summary>
    /// CompressHelper 的摘要说明
    /// Author: Johnny Wong
    /// Time: 2017.01.08
    /// 压缩解压辅助类
    /// </summary>
    public class CompressHelper
    {
        private static ReaderOptions options = new ReaderOptions();

        /// <summary>
        /// Author: Johnny Wong
        /// Time: 2018.01.08
        /// EditTime:2021-12-22
        /// 解压指定文件到目录
        /// </summary>
        /// <param name="file">压缩文件</param>
        /// <param name="folder">目录</param>
        /// <param name="encode">解码</param>
        public static void Uncompress(string file, string folder, string encode = "gb2312")
        {
            options.ArchiveEncoding.Default = Encoding.GetEncoding(encode);

            switch (FileHelper.GetExtension(file))
            {
                case ".7z":
                    SevenZUncompress(file, folder);
                    break;
                case ".rar":
                    RarUncompress(file, folder);
                    break;
                case ".zip":
                    ZipUncompress(file, folder);
                    break;
            }
        }

        /// <summary>
        /// Author: Johnny Wong
        /// Time: 2018.01.08
        /// 解压指定文件到目录(带进度条)
        /// </summary>
        /// <param name="file">压缩文件</param>
        /// <param name="folder">目录</param>
        /// <param name="pb">进度条</param>
        public static void Uncompress(string file, string folder, ProgressBar pb)
        {
            switch (FileHelper.GetExtension(file))
            {
                case ".7z":
                    SevenZUncompress(file, folder, pb);
                    break;
                case ".rar":
                    RarUncompress(file, folder, pb);
                    break;
                case ".zip":
                    ZipUncompress(file, folder, pb);
                    break;
            }
        }

        /// <summary>
        /// Author: Johnny Wong
        /// Time: 2018.01.08
        /// EditTime:2021-12-22
        /// 解压zip文件到指定目录
        /// </summary>
        /// <param name="file">zip文件</param>
        /// <param name="folder">目录</param>
        protected static void ZipUncompress(string file, string folder)
        {
            using (var archive = ZipArchive.Open(file, options))
            {
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(folder, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
        }

        /// <summary>
        /// Author: Johnny Wong
        /// Time: 2018.01.08
        /// EditTime:2021-12-22
        /// 解压zip文件到指定目录(带进度条)
        /// </summary>
        /// <param name="file">zip文件</param>
        /// <param name="folder">目录</param>
        /// <param name="pb">进度条</param>
        protected static void ZipUncompress(string file, string folder, ProgressBar pb)
        {
            using (var archive = ZipArchive.Open(file, options))
            {
                pb.Invoke(new Action(() => { pb.Maximum = archive.Entries.Count(entry => !entry.IsDirectory); }));
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(folder, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                    pb.Invoke(new Action(() =>
                    {
                        try
                        {
                            pb.Value++;
                        }
                        catch (Exception) { }
                    }));
                }
            }
        }

        /// <summary>
        /// Author: Johnny Wong
        /// Time: 2018.01.08
        /// EditTime:2021-12-22
        /// 解压rar文件到指定目录
        /// </summary>
        /// <param name="file">rar文件</param>
        /// <param name="folder">目录</param>
        protected static void RarUncompress(string file, string folder)
        {
            using (var archive = RarArchive.Open(file, options))
            {
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(folder, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
        }

        /// <summary>
        /// Author: Johnny Wong
        /// Time: 2018.01.08
        /// EditTime:2021-12-22
        /// 解压rar文件到指定目录(带进度条)
        /// </summary>
        /// <param name="file">rar文件</param>
        /// <param name="folder">目录</param>
        /// <param name="pb">进度条</param>
        protected static void RarUncompress(string file, string folder, ProgressBar pb)
        {
            using (var archive = RarArchive.Open(file, options))
            {
                pb.Invoke(new Action(() => { pb.Maximum = archive.Entries.Count(entry => !entry.IsDirectory); }));
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(folder, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                    pb.Invoke(new Action(() =>
                    {
                        try
                        {
                            pb.Value++;
                        }
                        catch (Exception) { }
                    }));
                }
            }
        }

        /// <summary>
        /// Author: Johnny Wong
        /// Time: 2018.01.08
        /// EditTime:2021-12-22
        /// 解压7z文件到指定目录
        /// </summary>
        /// <param name="file">7z文件</param>
        /// <param name="folder">目录</param>
        protected static void SevenZUncompress(string file, string folder)
        {
            using (var archive = SevenZipArchive.Open(file, options))
            {
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(folder, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                }
            }
        }

        /// <summary>
        /// Author: Johnny Wong
        /// Time: 2018.01.08
        /// EditTime:2021-12-22
        /// 解压7z文件到指定目录(带进度条)
        /// </summary>
        /// <param name="file">7z文件</param>
        /// <param name="folder">目录</param>
        /// <param name="pb">进度条</param>
        protected static void SevenZUncompress(string file, string folder, ProgressBar pb)
        {
            using (var archive = SevenZipArchive.Open(file, options))
            {
                pb.Invoke(new Action(() => { pb.Maximum = archive.Entries.Count(entry => !entry.IsDirectory); }));
                foreach (var entry in archive.Entries.Where(entry => !entry.IsDirectory))
                {
                    entry.WriteToDirectory(folder, new ExtractionOptions()
                    {
                        ExtractFullPath = true,
                        Overwrite = true
                    });
                    pb.Invoke(new Action(() =>
                    {
                        try
                        {
                            pb.Value++;
                        }
                        catch (Exception) { }
                    }));
                }
            }
        }
    }
}
