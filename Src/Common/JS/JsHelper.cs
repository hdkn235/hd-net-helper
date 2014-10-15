using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HD.Common
{
    /// <summary>
    /// 客户端脚本输出
    /// </summary>
    public class JsHelper
    {
        /// <summary>
        /// 弹出信息,并跳转指定页面。
        /// </summary>
        public static void AlertAndRedirect(string message, string toURL)
        {
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, message, toURL));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出信息,并返回历史页面
        /// </summary>
        public static void AlertAndGoHistory(string message, int value)
        {
            string js = @"<Script language='JavaScript'>alert('{0}');history.go({1});</Script>";
            HttpContext.Current.Response.Write(string.Format(js, message, value));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 直接跳转到指定的页面
        /// </summary>
        public static void Redirect(string toUrl)
        {
            string js = @"<script language=javascript>window.location.replace('{0}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, toUrl));
        }

        /// <summary>
        /// 弹出信息 并指定到父窗口
        /// </summary>
        public static void AlertAndParentUrl(string message, string toURL)
        {
            string js = "<script language=javascript>alert('{0}');window.top.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, message, toURL));
        }

        /// <summary>
        /// 返回到父窗口
        /// </summary>
        public static void ParentRedirect(string ToUrl)
        {
            string js = "<script language=javascript>window.top.location.replace('{0}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, ToUrl));
        }

        /// <summary>
        /// 返回历史页面
        /// </summary>
        public static void BackHistory(int value)
        {
            string js = @"<Script language='JavaScript'>history.go({0});</Script>";
            HttpContext.Current.Response.Write(string.Format(js, value));
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 弹出信息
        /// </summary>
        public static void Alert(string message)
        {
            string js = "<script language=javascript>alert('{0}');</script>";
            HttpContext.Current.Response.Write(string.Format(js, message));
        }

        /// <summary>
        /// 注册脚本块
        /// </summary>
        public static void RegisterScriptBlock(System.Web.UI.Page page, string _ScriptString)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "scriptblock", "<script type='text/javascript'>" + _ScriptString + "</script>");
        }

        #region 显示模态窗口

        /// <summary>
        /// 返回把指定链接地址显示模态窗口的脚本
        /// </summary>
        /// <param name="wid"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="url"></param>
        public static string GetShowModalWindowScript(string wid, string title, int width, int height, string url)
        {
            return string.Format("setTimeout(\"showModalWindow('{0}','{1}',{2},{3},'{4}')\",100);", wid, title, width, height, url);
        }

        /// <summary>
        /// 把指定链接地址显示模态窗口
        /// </summary>
        /// <param name="wid">窗口ID</param>
        /// <param name="title">标题</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="url">链接地址</param>
        public static void ShowModalWindow(string wid, string title, int width, int height, string url)
        {
            WriteScript(GetShowModalWindowScript(wid, title, width, height, url));
        }

        /// <summary>
        /// 为指定控件绑定前台脚本：显示模态窗口
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventName"></param>
        /// <param name="wid"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="url"></param>
        /// <param name="isScriptEnd"></param>
        public static void ShowCilentModalWindow(string wid, WebControl control, string eventName, string title, int width, int height, string url, bool isScriptEnd)
        {
            string script = isScriptEnd ? "return false;" : "";
            control.Attributes[eventName] = string.Format("showModalWindow('{0}','{1}',{2},{3},'{4}');" + script, wid, title, width, height, url);
        }

        /// <summary>
        /// 为指定控件绑定前台脚本：显示模态窗口
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="eventName"></param>
        /// <param name="wid"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="url"></param>
        /// <param name="isScriptEnd"></param>
        public static void ShowCilentModalWindow(string wid, TableCell cell, string eventName, string title, int width, int height, string url, bool isScriptEnd)
        {
            string script = isScriptEnd ? "return false;" : "";
            cell.Attributes[eventName] = string.Format("showModalWindow('{0}','{1}',{2},{3},'{4}');" + script, wid, title, width, height, url);
        }
        #endregion

        #region 显示客户端确认窗口
        /// <summary>
        /// 显示客户端确认窗口
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventName"></param>
        /// <param name="message"></param>
        public static void ShowCilentConfirm(WebControl control, string eventName, string message)
        {
            ShowCilentConfirm(control, eventName, "系统提示", 210, 125, message);
        }

        /// <summary>
        /// 显示客户端确认窗口
        /// </summary>
        /// <param name="control"></param>
        /// <param name="eventName"></param>
        /// <param name="title"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="message"></param>
        public static void ShowCilentConfirm(WebControl control, string eventName, string title, int width, int height, string message)
        {
            control.Attributes[eventName] = string.Format("return showConfirm('{0}',{1},{2},'{3}','{4}');", title, width, height, message, control.ClientID);
        }


        #endregion

        /// <summary>
        /// 写javascript脚本
        /// </summary>
        /// <param name="script">脚本内容</param>
        public static void WriteScript(string script)
        {
            Page page = GetCurrentPage();

            // NDGridViewScriptFirst(page.Form.Controls, page);

            page.ClientScript.RegisterStartupScript(page.GetType(), System.Guid.NewGuid().ToString(), script, true);

        }

        /// <summary>
        /// 得到当前页对象实例
        /// </summary>
        /// <returns></returns>
        public static Page GetCurrentPage()
        {
            return (Page)HttpContext.Current.Handler;
        }
    }
}