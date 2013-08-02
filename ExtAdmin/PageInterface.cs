﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExtDirectHandler.Configuration;
using Newtonsoft.Json.Linq;
using System.IO;
using ExtAdmin.BLL;
using System.Web.SessionState;

namespace ExtAdmin
{
    public class PageInterface
    {
        public JObject Login(JObject obj)
        {
            string server = obj.GetValue("server").ToString();
            string name = obj.GetValue("username").ToString();
            string password = obj.GetValue("password").ToString();
            LoginManager lm = new LoginManager();
            return lm.Login(server, name, password);
        }

        public JObject Logout() {
            return new JObject(new JProperty[] { new JProperty("success", true), new JProperty("msg", "注销成功!") });
        }

        public JObject Info() {
            HttpContext contextManager = HttpContext.Current;
            string conStr = contextManager.Request.Cookies["ConnectionString"].Value;
            return DataBaseManager.Info(conStr);
        }

        /// <summary>Retrieve an object from the session.</summary>
        /// 
        /// <param name="name">the name in of the object in the session.</param>
        /// 
        /// <returns>the session object</returns>
        /// 
        protected Object getSessionObject(String name)
        {
            Object sessionObject = null;
            HttpContext contextManager = HttpContext.Current;
            if (contextManager != null)
            {
                HttpSessionState session = contextManager.Session;
                if (session != null)
                {
                    sessionObject = session[name];
                }
            }
            return sessionObject;
        }

        /// <summary>Store an object in the session</summary>
        /// 
        /// <param name="name">the name in which to store the object under.</param>
        /// <param name="anObject">the object to store.</param>
        /// 
        protected void storeSessionObject(String name, Object anObject)
        {
            HttpContext contextManager = HttpContext.Current;
            if (contextManager != null)
            {
                HttpSessionState session = contextManager.Session;
                if (session != null)
                {
                    session.Add(name, anObject);
                }
            }
        }
    }
}
