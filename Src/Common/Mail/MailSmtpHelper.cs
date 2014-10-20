using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace HD.Common
{
    public class MailSmtpHelper
    {
        #region 默认配置信息
        public static readonly string smtpServer = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"];
        public static readonly string userName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
        public static readonly string pwd = System.Configuration.ConfigurationManager.AppSettings["Pwd"];
        public static readonly int smtpPort = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SmtpPort"]);
        public static readonly string authorName = System.Configuration.ConfigurationManager.AppSettings["AuthorName"];
        #endregion

        #region 使用配置文件的配置信息发送邮件
        /// <summary>
        /// 使用配置文件的配置信息发送邮件
        /// </summary>
        /// <param name="server">包含用于 SMTP 事务的主机的名称或 IP 地址</param>
        /// <param name="sender">发件人</param>
        /// <param name="recipient">收件人</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="isBodyHtml">内容是否是Html格式</param>
        /// <param name="encoding">编码</param>
        /// <param name="isAuthentication">是否认证</param>
        /// <param name="files">附件列表</param>
        public static void SendByConfig(
            string server,
            string sender,
            string recipient,
            string subject,
            string body,
            bool isBodyHtml,
            Encoding encoding,
            bool isAuthentication,
            params string[] files)
        {
            SmtpClient smtpClient = new SmtpClient(server);
            MailMessage message = new MailMessage(sender, recipient);
            message.IsBodyHtml = isBodyHtml;

            message.SubjectEncoding = encoding;
            message.BodyEncoding = encoding;

            message.Subject = subject;
            message.Body = body;

            message.Attachments.Clear();
            if (files != null && files.Length != 0)
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    Attachment attach = new Attachment(files[i]);
                    message.Attachments.Add(attach);
                }
            }

            if (isAuthentication == true)
            {
                smtpClient.Credentials = new NetworkCredential(SmtpConfig.Create().SmtpSetting.User,
                    SmtpConfig.Create().SmtpSetting.Password);
            }
            smtpClient.Send(message);


        }

        public static void SendByConfig(string recipient, string subject, string body)
        {
            SendByConfig(
                SmtpConfig.Create().SmtpSetting.Server,
                SmtpConfig.Create().SmtpSetting.Sender,
                recipient,
                subject,
                body,
                true,
                Encoding.Default,
                true,
                null);
        }

        public static void SendByConfig(string Recipient, string Sender, string Subject, string Body)
        {
            SendByConfig(
                SmtpConfig.Create().SmtpSetting.Server,
                Sender,
                Recipient,
                Subject,
                Body,
                true,
                Encoding.Default,
                true,
                null);
        }
        #endregion

        #region 读取web.config中的配置信息发送邮件
        /// <summary>
        /// 群发邮件
        /// </summary>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="to">收件人列表</param>
        public void Send(string subject, string body, string to)
        {
            List<string> toList = StringHelper.GetStrList(StringHelper.ToDBC(to), ',');
            OpenSmtp.Mail.Smtp smtp = new OpenSmtp.Mail.Smtp(smtpServer, userName, pwd, smtpPort);
            foreach (string s in toList)
            {
                OpenSmtp.Mail.MailMessage msg = new OpenSmtp.Mail.MailMessage();
                msg.From = new OpenSmtp.Mail.EmailAddress(userName, authorName);

                msg.AddRecipient(s, OpenSmtp.Mail.AddressType.To);

                //设置邮件正文,并指定格式为 html 格式
                msg.HtmlBody = body;
                //设置邮件标题
                msg.Subject = subject;
                //指定邮件正文的编码
                msg.Charset = "gb2312";
                //发送邮件
                smtp.SendMail(msg);
            }
        }
        #endregion
    }

    /// <summary>
    /// smtp设置类
    /// </summary>
    public class SmtpSetting
    {
        private string _server;
        public string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        private bool _authentication;
        public bool Authentication
        {
            get { return _authentication; }
            set { _authentication = value; }
        }

        private string _user;
        public string User
        {
            get { return _user; }
            set { _user = value; }
        }

        private string _sender;
        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }

    /// <summary>
    /// smtp配置类
    /// </summary>
    public class SmtpConfig
    {
        private static SmtpConfig _smtpConfig;
        private string ConfigFile
        {
            get
            {
                string configPath = ConfigurationManager.AppSettings["SmtpConfigPath"];
                if (string.IsNullOrEmpty(configPath) || configPath.Trim().Length == 0)
                {
                    configPath = HttpContext.Current.Request.MapPath("/Config/SmtpSetting.config");
                }
                else
                {
                    if (!Path.IsPathRooted(configPath))
                        configPath = HttpContext.Current.Request.MapPath(Path.Combine(configPath, "SmtpSetting.config"));
                    else
                        configPath = Path.Combine(configPath, "SmtpSetting.config");
                }
                return configPath;
            }
        }
        public SmtpSetting SmtpSetting
        {
            get
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(this.ConfigFile);
                SmtpSetting smtpSetting = new SmtpSetting();
                smtpSetting.Server = doc.DocumentElement.SelectSingleNode("Server").InnerText;
                smtpSetting.Authentication = Convert.ToBoolean(doc.DocumentElement.SelectSingleNode("Authentication").InnerText);
                smtpSetting.User = doc.DocumentElement.SelectSingleNode("User").InnerText;
                smtpSetting.Password = doc.DocumentElement.SelectSingleNode("Password").InnerText;
                smtpSetting.Sender = doc.DocumentElement.SelectSingleNode("Sender").InnerText;

                return smtpSetting;
            }
        }
        private SmtpConfig()
        {

        }
        public static SmtpConfig Create()
        {
            if (_smtpConfig == null)
            {
                _smtpConfig = new SmtpConfig();
            }
            return _smtpConfig;
        }
    }
}
