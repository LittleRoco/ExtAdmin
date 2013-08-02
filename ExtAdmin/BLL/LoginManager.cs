﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
using System.Text;

namespace ExtAdmin.BLL
{
    public class LoginManager
    {
        public JObject Login(string server, string userName, string passWord)
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.InitialCatalog = "master";
            sb.DataSource = server;
            sb.UserID = userName;
            sb.Password = passWord;
            sb.ConnectTimeout = 3;
            SqlConnection con = new SqlConnection(sb.ToString());
            JObject result = new JObject(new JProperty[] { new JProperty("success", false), new JProperty("msg", ""), new JProperty("constr", "") });
            string constr = "";
            try
            {
                con.Open();
                result.Property("msg").Value = "登陆成功!";
                result.Property("success").Value = true;
                string constr1 = sb.ToString();
                byte[] by = Encoding.Default.GetBytes(constr1);
                constr = Convert.ToBase64String(by);
                result.Property("constr").Value = constr;
            }
            catch (Exception ex)
            {
                result.Property("msg").Value = ex.Message;
                result.Property("success").Value = false;
            }
            finally
            {
                con.Close();
            }
            return result;
        }
    }
}
