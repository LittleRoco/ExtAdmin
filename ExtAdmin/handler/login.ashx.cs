using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;

namespace ExtAdmin.handler
{
    /// <summary>
    /// login 的摘要说明
    /// </summary>
    public class login : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.HttpMethod.ToLower();
            if (method == "login")
            {
                string server = context.Request.Form["server"];
                string name = context.Request.Form["username"];
                string password = context.Request.Form["password"];
                SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
                sb.InitialCatalog = "master";
                sb.DataSource = server;
                sb.UserID = name;
                sb.Password = password;
                sb.ConnectTimeout = 3;
                SqlConnection con=new SqlConnection(sb.ToString());
                string msg = "";
                string b = "";
                string constr="";
                try
                {
                    con.Open();
                    msg = "登陆成功!";
                    b = "true";
                    string constr1 = sb.ToString();
                    byte[] by = Encoding.Default.GetBytes(constr1);
                    constr = Convert.ToBase64String(by);
                }
                catch(Exception ex)
                {
                    msg = ex.Message;
                    b = "false";
                }
                finally
                {
                    con.Close();
                }
                context.Response.ContentType = "text/json";
                context.Response.StatusCode = 200;
                string s = "{success:"+b+",msg:\""+msg+"\",constr:\""+constr+"\"}";
                context.Response.Write(s);
            }
            else if (method == "logout")
            {
                //string name = context.Request.Form["username"];
                context.Response.ContentType = "text/json";
                context.Response.StatusCode = 200;
                string s = "{success:true,msg:\'注销成功!'}";
                context.Response.Write(s);
            }
            else
            {
                context.Response.ContentType = "text/json";
                context.Response.StatusCode = 405;
                context.Response.Write("{success:failed,msg:'Method Not Supported'}");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}