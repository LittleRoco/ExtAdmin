using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Text;
using ExtAdmin.App_Code;

namespace ExtAdmin.handler
{
    /// <summary>
    /// database 的摘要说明
    /// </summary>
    public class database : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request.HttpMethod.ToLower();
            
            if (method == "info")
            {
                #region INFO
                string ConStr = context.Request.Cookies["ConnectionString"].Value;
                string b = "true";
                string server = "";
                string username = "";
                if (ConStr != "")
                {
                    b = "true";   
                    byte[] by = Convert.FromBase64String(ConStr);
                    ConStr = Encoding.Default.GetString(by);
                    SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(ConStr);
                    server = sb.DataSource;
                    username = sb.UserID;
                }

                context.Response.ContentType = "text/json";
                context.Response.StatusCode = 200;
                string s = "{success:" + b + ",server:\'" + server + "\',user:\'" + username + "\'}";
                context.Response.Write(s);
                #endregion
            }
            else if(method=="get")
            {
                #region LIST
                if (context.Request.QueryString["node"] == "root")
                {
                    #region root
                    context.Response.ContentType = "text/json";
                    context.Response.StatusCode = 200;
                    JSONObject db = new JSONObject();
                    db.id = "databases";
                    db.parent = "root";
                    db.text = "数据库";
                    db.expanded = "false";
                    db.leaf = "false";
                    JSONObject un = new JSONObject();
                    un.id = "users";
                    un.parent = "root";
                    un.text = "登录名";
                    un.expanded = "false";
                    un.leaf = "false";
                    string s = "[" + db.ToString() + "," + un.ToString() + "]";
                    context.Response.Write(s);
                    #endregion
                }
                else if (context.Request.QueryString["node"] == "users")
                {
                    #region users
                    string ConStr = context.Request.Cookies["ConnectionString"].Value;
                    if (ConStr != "")
                    {
                        byte[] by = Convert.FromBase64String(ConStr);
                        ConStr = Encoding.Default.GetString(by);
                        SqlConnection con = new SqlConnection(ConStr);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("select [name] from syslogins;",con);
                        SqlDataReader dr=cmd.ExecuteReader();
                        string jsonl = "[";
                        string jsonr = "]";
                        string jsonm = "";
                        while (dr.Read())
                        {
                            JSONObject db = new JSONObject();
                            db.id = "user_"+dr[0].ToString();
                            db.parent = "users";
                            db.text = dr[0].ToString();
                            db.expanded = "false";
                            db.leaf = "true";
                            jsonm += db.ToString();
                            jsonm += ",";
                        } 
                        jsonm.TrimEnd(',');
                        string json = jsonl + jsonm + jsonr;
                        context.Response.ContentType = "text/json";
                        context.Response.StatusCode = 200;
                        context.Response.Write(json);
                    }
                    #endregion
                }
                else if (context.Request.QueryString["node"] == "databases")
                {
                    #region Databases
                    string ConStr = context.Request.Cookies["ConnectionString"].Value;
                    if (ConStr != "")
                    {
                        byte[] by = Convert.FromBase64String(ConStr);
                        ConStr = Encoding.Default.GetString(by);
                        SqlConnection con = new SqlConnection(ConStr);
                        con.Open();
                        SqlCommand cmd = new SqlCommand("select [name] from sysdatabases;", con);
                        SqlDataReader dr = cmd.ExecuteReader();
                        string jsonl = "[";
                        string jsonr = "]";
                        string jsonm = "";
                        while (dr.Read())
                        {
                            JSONObject db = new JSONObject();
                            db.id = "db_" + dr[0].ToString();
                            db.parent = "databases";
                            db.text = dr[0].ToString();
                            db.expanded = "false";
                            db.leaf = "false";
                            jsonm += db.ToString();
                            jsonm += ",";
                        }
                        jsonm.TrimEnd(',');
                        string json = jsonl + jsonm + jsonr;
                        context.Response.ContentType = "text/json";
                        context.Response.StatusCode = 200;
                        context.Response.Write(json);
                    }
                    #endregion
                }
                #endregion
                #region TREE
                else if(context.Request.QueryString["node"].IndexOf("db_")==0&&
                    context.Request.QueryString["node"].IndexOf("_table")==-1&&
                    context.Request.QueryString["node"].IndexOf("_view")==-1&&
                context.Request.QueryString["node"].IndexOf("_procedure")==-1)
                {
                    #region DBTREE
                    string node = context.Request.QueryString["node"];
                    context.Response.ContentType = "text/json";
                    context.Response.StatusCode = 200;
                    JSONObject t = new JSONObject();
                    t.id = node+"_table";
                    t.parent = node;
                    t.text = "表";
                    t.expanded = "false";
                    t.leaf = "false";
                    JSONObject v = new JSONObject();
                    v.id = node+"_view";
                    v.parent = node;
                    v.text = "视图";
                    v.expanded = "false";
                    v.leaf = "false";
                    JSONObject p = new JSONObject();
                    p.id = node+"_procedure";
                    p.parent = node;
                    p.text = "存储过程";
                    p.expanded = "false";
                    p.leaf = "false";
                    string s = "[" + t.ToString() + "," + v.ToString() + "," + p.ToString() + "]";
                    context.Response.Write(s);
                    #endregion
                }
                else if (context.Request.QueryString["node"].IndexOf("db_") == 0 &&
                    context.Request.QueryString["node"].IndexOf("_table") > -1)
                {
                    #region tabletree
                    string node = context.Request.QueryString["node"];
                    string ConStr = context.Request.Cookies["ConnectionString"].Value;
                    if (ConStr != "")
                    {
                        byte[] by = Convert.FromBase64String(ConStr);
                        ConStr = Encoding.Default.GetString(by);
                        SqlConnection con = new SqlConnection(ConStr);
                        con.Open();
                        string db1 = context.Request.QueryString["node"].Split('_')[1];
                        SqlCommand cmd = new SqlCommand("select [name] from " + db1 + ".sys.objects where type='U' order by [name] asc;", con);
                        SqlDataReader dr = cmd.ExecuteReader();
                        string jsonl = "[";
                        string jsonr = "]";
                        string jsonm = "";
                        while (dr.Read())
                        {
                            JSONObject db = new JSONObject();
                            db.id = node.Replace("_table","_tlist") + dr[0].ToString();
                            db.parent = node;
                            db.text = dr[0].ToString();
                            db.expanded = "false";
                            db.leaf = "true";
                            jsonm += db.ToString();
                            jsonm += ",";
                        }
                        jsonm.TrimEnd(',');
                        string json = jsonl + jsonm + jsonr;
                        context.Response.ContentType = "text/json";
                        context.Response.StatusCode = 200;
                        context.Response.Write(json);
                    }
                    #endregion
                }
                else if (context.Request.QueryString["node"].IndexOf("db_") == 0 &&
                context.Request.QueryString["node"].IndexOf("_view") > -1)
                {
                    #region viewtree
                    string node = context.Request.QueryString["node"];
                    string ConStr = context.Request.Cookies["ConnectionString"].Value;
                    if (ConStr != "")
                    {
                        byte[] by = Convert.FromBase64String(ConStr);
                        ConStr = Encoding.Default.GetString(by);
                        SqlConnection con = new SqlConnection(ConStr);
                        con.Open();
                        string db1 = context.Request.QueryString["node"].Split('_')[1];
                        SqlCommand cmd = new SqlCommand("select [name] from " + db1 + ".sys.objects where type='V' order by [name] asc;", con);
                        SqlDataReader dr = cmd.ExecuteReader();
                        string jsonl = "[";
                        string jsonr = "]";
                        string jsonm = "";
                        while (dr.Read())
                        {
                            JSONObject db = new JSONObject();
                            db.id = node.Replace("_view", "_vlist") + dr[0].ToString();
                            db.parent = node;
                            db.text = dr[0].ToString();
                            db.expanded = "false";
                            db.leaf = "true";
                            jsonm += db.ToString();
                            jsonm += ",";
                        }
                        jsonm.TrimEnd(',');
                        string json = jsonl + jsonm + jsonr;
                        context.Response.ContentType = "text/json";
                        context.Response.StatusCode = 200;
                        context.Response.Write(json);
                    }
                    #endregion
                }
                else if (context.Request.QueryString["node"].IndexOf("db_") == 0 &&
                context.Request.QueryString["node"].IndexOf("_procedure") > -1)
                {
                    #region proceduretree
                    string node = context.Request.QueryString["node"];
                    string ConStr = context.Request.Cookies["ConnectionString"].Value;
                    if (ConStr != "")
                    {
                        byte[] by = Convert.FromBase64String(ConStr);
                        ConStr = Encoding.Default.GetString(by);
                        SqlConnection con = new SqlConnection(ConStr);
                        con.Open();
                        string db1 = context.Request.QueryString["node"].Split('_')[1];
                        SqlCommand cmd = new SqlCommand("select [name] from " + db1 + ".sys.objects where type='P' order by [name] asc;", con);
                        SqlDataReader dr = cmd.ExecuteReader();
                        string jsonl = "[";
                        string jsonr = "]";
                        string jsonm = "";
                        while (dr.Read())
                        {
                            JSONObject db = new JSONObject();
                            db.id = node.Replace("_procedure", "_plist") + dr[0].ToString();
                            db.parent = node;
                            db.text = dr[0].ToString();
                            db.expanded = "false";
                            db.leaf = "true";
                            jsonm += db.ToString();
                            jsonm += ",";
                        }
                        jsonm.TrimEnd(',');
                        string json = jsonl + jsonm + jsonr;
                        context.Response.ContentType = "text/json";
                        context.Response.StatusCode = 200;
                        context.Response.Write(json);
                    }
                    #endregion
                }
                #endregion
                #region NONE
                else
                {
                    context.Response.ContentType = "text/json";
                    context.Response.StatusCode = 200;
                    string s = "[]";
                    context.Response.Write(s);
                }
                #endregion
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