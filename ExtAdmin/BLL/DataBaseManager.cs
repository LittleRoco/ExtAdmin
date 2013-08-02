using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Data.SqlClient;

namespace ExtAdmin.BLL
{
    public class DataBaseManager
    {
        public static JObject Info(string conStr) {
            bool b =false;
            string server = "";
            string userName = "";
            if (conStr != "")
            {
                b = true;
                byte[] by = Convert.FromBase64String(conStr);
                conStr = Encoding.Default.GetString(by);
                SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(conStr);
                server = sb.DataSource;
                userName = sb.UserID;
            }
            return new JObject(new JProperty[] { new JProperty("success", b), new JProperty("server", server), new JProperty("userName", userName) });
        }
    }
}