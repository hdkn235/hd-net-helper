using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace HD.Helper
{
    public static class XmlHelper
    {
        /// <summary>
        /// 将一个对象序列化为流中
        /// </summary>
        /// <param name="stream">存放对象序列化后的流</param>
        /// <param name="o">要序列化的对象</param>
        /// <param name="encoding">编码格式</param>
        private static void XmlSerializeInternal(Stream stream, object o, Encoding encoding)
        {
            if (o == null)
                throw new ArgumentNullException("o");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer serializer = new XmlSerializer(o.GetType());

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineChars = "\r\n";
            settings.Encoding = encoding;
            settings.IndentChars = "    ";

            using (XmlWriter writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, o);
                writer.Close();
            }
        }

        /// <summary>
        /// 将一个对象序列化为XML字符串
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>序列化产生的XML字符串</returns>
        public static string XmlSerialize(object o, Encoding encoding)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializeInternal(stream, o, encoding);

                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// 将一个对象按XML序列化的方式写入到一个文件
        /// </summary>
        /// <param name="o">要序列化的对象</param>
        /// <param name="path">保存文件路径</param>
        /// <param name="encoding">编码方式</param>
        public static void XmlSerializeToFile(object o, string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                XmlSerializeInternal(file, o, encoding);
            }
        }

        /// <summary>
        /// 从XML字符串中反序列化对象
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="s">包含对象的XML字符串</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserialize<T>(string s, Encoding encoding)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException("s");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(encoding.GetBytes(s)))
            {
                using (StreamReader sr = new StreamReader(ms, encoding))
                {
                    return (T)mySerializer.Deserialize(sr);
                }
            }
        }

        /// <summary>
        /// 读入一个文件，并按XML的方式反序列化对象。
        /// </summary>
        /// <typeparam name="T">结果对象类型</typeparam>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>反序列化得到的对象</returns>
        public static T XmlDeserializeFromFile<T>(string path, Encoding encoding)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (encoding == null)
                throw new ArgumentNullException("encoding");

            string xml = File.ReadAllText(path, encoding);
            return XmlDeserialize<T>(xml, encoding);
        }

        #region 得到XML节点的文本
        /// <summary>
        /// 得到XML节点的文本
        /// </summary>
        /// <param name="p_xnParent">父节点</param>
        /// <param name="p_strNodeName">节点名称</param>
        /// <returns>XML节点的文本</returns>
        public static string GetXmlNodeText(XmlNode p_xnParent, string p_strNodeName)
        {
            XmlNode xnChild = p_xnParent.SelectSingleNode(p_strNodeName);

            if (xnChild != null)//节点有数据
            {
                return xnChild.InnerText;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 得到XML节点的属性值
        /// <summary>
        /// 得到XML节点的属性值
        /// </summary>
        /// <param name="p_xnParent">父节点</param>
        /// <param name="p_strNodeName">节点名称</param>
        /// <param name="p_strAttributeName">属性名称</param>
        /// <returns>XML节点的属性值</returns>
        public static string GetXmlNodeAttribute(XmlNode p_xnParent, string p_strNodeName, string p_strAttributeName)
        {
            //判断该节点是否存在该属性
            if (p_xnParent.SelectSingleNode(p_strNodeName + "[@name='" + p_strAttributeName + "']") != null)
            {
                return p_xnParent.SelectSingleNode(p_strNodeName).Attributes[p_strAttributeName].Value;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 得到XML节点的属性值
        /// </summary>
        /// <param name="p_xnNode">节点对象</param>
        /// <param name="p_strAttributeName">属性名称</param>
        /// <returns>XML节点的属性值</returns>
        public static string GetXmlNodeAttribute(XmlNode p_xnNode, string p_strAttributeName)
        {
            //判断该节点是否存在该属性
            if (p_xnNode.Attributes[p_strAttributeName] != null)
            {
                return p_xnNode.Attributes[p_strAttributeName].Value;
            }
            else
            {
                return "";
            }
        }
        #endregion
    }
}
