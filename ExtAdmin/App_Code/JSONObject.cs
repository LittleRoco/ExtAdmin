using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtAdmin.App_Code
{
    public class JSONObject
    {
        public string text;
        public string id;
        public string parent;
        public string leaf;
        public string expanded;
        public override string ToString()
        {
            return "{text:\'" + text + "\',id:\'" + id + "\',parent:\'" + parent + "\',leaf:" +
               leaf + ",expanded:" + expanded + "}";
        }
    }
}