using System;
using System.Collections.Generic;
using System.Text;

namespace HD.Common
{
    public class StringPlus
    {

        #region list与字符串互转
        /// <summary>
        /// 将字符串按指定分隔符分割转换为list
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="speater">分割符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns>list</returns>
        public static List<string> GetStrArray(string str, char speater)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    list.Add(s);
                }
            }
            return list;
        }

        /// <summary>
        /// 将list按分隔符组合为字符串
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="speater">分隔符</param>
        /// <returns>字符串</returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        } 
        #endregion
        
        #region 删除最后一个字符之后的字符

        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str,string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        #endregion

        #region 将字符串样式转换为纯字符串
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            string RetrunValue = "";
            //如果为空，返回空值
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                string NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }
            return RetrunValue;
        }
        #endregion

        #region 将字符串转换为新样式
        public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
        {
            string ReturnValue = "";
            //如果输入空值，返回空，并给出错误提示
            if (StrList == null)
            {
                ReturnValue = "";
                Error = "请输入需要划分格式的字符串";
            }
            else
            {
                //检查传入的字符串长度和样式是否匹配,如果不匹配，则说明使用错误。给出错误信息并返回空值
                int strListLength = StrList.Length;
                int NewStyleLength = GetCleanStyle(NewStyle, SplitString).Length;
                if (strListLength != NewStyleLength)
                {
                    ReturnValue = "";
                    Error = "样式格式的长度与输入的字符长度不符，请重新输入";
                }
                else
                {
                    //检查新样式中分隔符的位置
                    string Lengstr = "";
                    for (int i = 0; i < NewStyle.Length; i++)
                    {
                        if (NewStyle.Substring(i, 1) == SplitString)
                        {
                            Lengstr = Lengstr + "," + i;
                        }
                    }
                    if (Lengstr != "")
                    {
                        Lengstr = Lengstr.Substring(1);
                    }
                    //将分隔符放在新样式中的位置
                    string[] str = Lengstr.Split(',');
                    foreach (string bb in str)
                    {
                        StrList = StrList.Insert(int.Parse(bb), SplitString);
                    }
                    //给出最后的结果
                    ReturnValue = StrList;
                    //因为是正常的输出，没有错误
                    Error = "";
                }
            }
            return ReturnValue;
        }
        #endregion

        #region 字符串转换\数字转换 转换为安全格式
        /// <summary>
        /// 返回安全整数类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <returns>int16 整形数字</returns>
        public static string safeInt(string str)
        {
            string constr = "0";
            try
            {
                constr = (Convert.ToInt16(str)).ToString();
                return constr;
            }
            catch
            {
                return constr;
            }
        }
        ///////////////////////////////////////////////
        //
        //  功能：安全处理用户提供的字符串内容
        //
        //  作者：依依秋寒
        //  时间：2008-10-27
        //
        ///////////////////////////////////////////////
        /// <summary>
        /// 转化为安全字符串 去除其中的英文单引号
        /// </summary>
        /// <param name="str">字符串内容</param>
        /// <returns>string 处理后的</returns>
        public static string safeStr(string str)
        {
            try
            {
                str = str.Replace("'", "'");
                str = str.Replace("\"", "&quot;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                return str;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 把字符串转化为安全的text内容
        /// </summary>
        /// <param name="str">传入的字符串内容</param>
        /// <returns>转化后结果 string</returns>
        public static string safeText(string str)
        {
            try
            {
                str = str.Replace("'", "'");
                str = str.Replace("\"", "&quot;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("\n", "<br />");
                return str;
            }
            catch
            {
                return "";
            }
        }
        ///////////////////////////////////////////////
        //
        //  功能：把经过安全处理的字符串返原成用户提交的内容
        //
        //  作者：依依秋寒
        //  时间：2008-10-27
        //
        ///////////////////////////////////////////////
        /// <summary>
        /// 把'还原成单引号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string safeStrToSource(string str)
        {
            try
            {
                str = str.Replace("&lt;", "<");
                str = str.Replace("&gt;", ">");
                str = str.Replace("&quot;", "\"");
                str = str.Replace("'", "'");
                str = str.Replace("&nbsp;", " ");
                return str;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 把经过安全的text内容转化为源字符串
        /// </summary>
        /// <param name="str">传入的字符串内容</param>
        /// <returns></returns>
        public static string safeTextToSource(string str)
        {
            try
            {
                str = str.Replace("<br />", "\n");
                str = str.Replace("&lt;", "<");
                str = str.Replace("&gt;", ">");
                str = str.Replace("&quot;", "\"");
                str = str.Replace("'", "'");
                str = str.Replace("&nbsp;", " ");
                return str;
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 是否安全
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsSafeSql(string str)
        {
            string sql = str.ToLower();
            if (sql.Contains("delete"))
            {
                return false;
            }
            else if (sql.Contains("select"))
            {
                return false;
            }
            else if (sql.Contains("update"))
            {
                return false;
            }
            else if (sql.Contains("drop"))
            {
                return false;
            }
            else if (sql.Contains("execute"))
            {
                return false;
            }
            else if (sql.Contains("exec"))
            {
                return false;
            }
            else if (sql.Contains("'"))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 替换不安全sql
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SafeSql(string str)
        {
            System.Text.StringBuilder sb = new StringBuilder(str.ToLower());
            sb.Replace("delete", "");
            sb.Replace("select", "");
            sb.Replace("update", "");
            sb.Replace("drop", "");
            sb.Replace("execute", "");
            sb.Replace("exec", "");
            sb.Replace("'", "");
            return sb.ToString();
        }
        #endregion

        #region 全角\半角转换
        /// <summary>
        /// 全角转半角的函数(DBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        /// <summary>
        /// 半角转全角的函数(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>        
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }
        #endregion

        #region 截取字符串
        /// <summary>
        /// 从字符串左边起给定的位置截取字符串
        /// </summary>
        /// <param name="str">给定要截取的字符串内容</param>
        /// <param name="len">截取的起始位置</param>
        /// <returns>截取后的字符串 string</returns>
        public static string leftstr(string str, int len)
        {
            if (str.Length > len && len > 0)
            {
                return str.Substring(0, len - 1) + "…";
            }
            else
            {
                return str;
            }
        }

        /// <summary>
        /// 截取相应数目的字符
        /// </summary>
        public static string MyLeftFunction(string mText, int byteCount)
        {
            if (byteCount < 1) return mText;
            if (System.Text.Encoding.Default.GetByteCount(mText) <= byteCount)
            {
                return mText;
            }
            else
            {
                byte[] txtBytes = System.Text.Encoding.Default.GetBytes(mText);
                byte[] newBytes = new byte[byteCount - 4];
                for (int i = 0; i < byteCount - 4; i++)
                    newBytes[i] = txtBytes[i];
                return System.Text.Encoding.Default.GetString(newBytes) + "...";
            }
        }
        #endregion

        #region 从字符串里随机得到，规定个数的字符串.

        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="allChar"></param>
        /// <param name="CodeCount"></param>
        /// <returns></returns>
        private string GetRandomCode(string allChar, int CodeCount)
        {
            //string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z"; 
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }
            return RandomCode;
        }

        #endregion
    }
}
