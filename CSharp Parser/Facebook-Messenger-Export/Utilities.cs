using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Messenger_Export
{
    class Utilities
    {


        public static void AddJsonToWebRequest(HttpWebRequest req, JObject param)
        {
            IList<string> keys = param.Properties().Select(p => p.Name).ToList();

          foreach(string s in keys)
            {
                req.Headers.Add(s, param[s].ToString());
           //     Console.WriteLine(s + " " + param[s]);
            }
        }
    }
}
