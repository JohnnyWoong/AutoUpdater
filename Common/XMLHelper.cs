using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Jnw.Common
{
    /// <summary>
    /// 创建人:Johnny Wong
    /// 创建时间:2013-03-28
    /// 修改时间:2016-07-09
    /// 功能:与XML文件进行交互(读写操作)
    /// </summary>
    public class XMLHelper
    {
        private static XmlDocument xd = new XmlDocument();

        /// <summary>
        /// 创建人:Johnny Wong
        /// 创建时间:2013-03-28
        /// 功能:根据item的key来获取相对应的值
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="name">item的key</param>
        /// <returns></returns>
        public static string GetItemValueByKey(string path, string name)
        {
            xd.Load(path);
            return xd.DocumentElement.SelectSingleNode("item[@key='" + name + "']").Attributes["value"].Value;
        }

        /// <summary>
        /// 创建人:Johnny Wong
        /// 创建时间:2016-07-09
        /// 功能:获取指定节点下的指定子节点的值
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="name">item的key</param>
        /// <param name="childname">item的key</param>
        /// <returns></returns>
        public static string GetItemValueByKey(string path, string name, string childname)
        {
            xd.Load(path);
            return xd.DocumentElement.SelectSingleNode(name).SelectSingleNode("item[@key='" + childname + "']").Attributes["value"].Value;
        }

        /// <summary>
        /// 创建人:Johnny Wong
        /// 创建时间:2016-07-09
        /// 功能:获取指定位置节点的值
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="name">节点</param>
        /// <param name="queue">位置</param>
        /// <returns></returns>
        public static string GetItemValue(string path, string name, int queue)
        {
            xd.Load(path);
            return xd.DocumentElement.SelectNodes(name)[queue].Attributes["value"].Value;
        }

        /// <summary>
        /// Author:Johnny Wong
        /// Time:2018-01-06
        /// EditTime:2019-06-15
        /// 获取指定数量的指定节点下的item值并进行拼接
        /// </summary>
        /// <param name="path">xml路径</param>
        /// <param name="name">节点</param>
        /// <param name="queue">位置</param>
        /// <returns></returns>
        public static string GetItemsValue(string path, string name, int queue)
        {
            xd.Load(path);
            string r = "";
            for (int i = queue; i >= 0; i--)
            {
                r += xd.DocumentElement.SelectNodes(name)[i].SelectSingleNode("item").Attributes["value"].Value + "\r\n\r\n\r\n";
            }
            return r.Substring(0, r.LastIndexOf("\r\n\r\n\r\n")).Replace("\r", "").Replace("\n", "\r\n"); //replace解决极少时候xml文件读取不到换行符的问题 (比如github?)
        }

        /// <summary>
        /// 创建人:Johnny Wong
        /// 创建时间:2016-07-09
        /// 功能:获取指定节点的个数
        /// <param name="path">路径</param>
        /// <param name="name">节点</param>
        /// </summary>
        public static int GetItemCount(string path, string name)
        {
            xd.Load(path);
            return xd.DocumentElement.SelectNodes(name).Count;
        }

        /// <summary>
        /// Author:Johnny
        /// Time:2013-8-26
        /// 得到所有的it项的value,返回二维数组([0,0]=key,[0,1]=value)
        /// </summary>
        /// <param name="path">xml路径</param>
        /// <returns></returns>
        public static string[,] GetIts(string path)
        {
            xd.Load(path);
            var xdNode = xd.DocumentElement.SelectSingleNode("items");
            var xdList = xdNode.SelectNodes("it");
            string[,] ss = new string[xdList.Count, 2];
            for (int i = 0; i < xdList.Count; i++)
            {
                ss[i, 0] = xdList[i].Attributes["key"].Value;
                ss[i, 1] = xdList[i].Attributes["value"].Value;
            }
            return ss;
        }

        /// <summary>
        /// Author:Johnny
        /// Time:2016-07-09
        /// 得到指定的it项的value,返回二维数组([0,0]=key,[0,1]=value)
        /// </summary>
        /// <param name="path">xml路径</param>
        /// <param name="queue">位置</param>
        /// <returns></returns>
        public static string[,] GetIts(string path, int queue)
        {
            xd.Load(path);
            XmlNode xdNode;
            XmlNodeList xdList;
            int length = 0;
            for (int j = 0; j <= queue; j++)
            {
                length += xd.DocumentElement.SelectNodes("version")[j].SelectSingleNode("items").SelectNodes("it").Count;
            }
            string[,] ss = new string[length, 2];
            for (int j = 0; j <= queue; j++)
            {
                xdNode = xd.DocumentElement.SelectNodes("version")[j].SelectSingleNode("items");
                xdList = xdNode.SelectNodes("it");
                for (int i = 0; i < xdList.Count; i++)
                {
                    ss[--length, 0] = xdList[i].Attributes["key"].Value;
                    ss[length, 1] = xdList[i].Attributes["value"].Value;
                }
            }
            return ss;
        }
    }
}
