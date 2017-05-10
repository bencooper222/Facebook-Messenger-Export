using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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

        public static List<List<string>> ReadCSV(string path)
        {
            StreamReader reader = new StreamReader(path);
            List<List<string>> rtn = new List<List<string>>();

            bool isFirst = true;
            while (reader.Peek() >= 0) // creds to http://www.sanfoundry.com/csharp-program-read-lines-until-end-file/
            {
                string[] line = reader.ReadLine().Split(',');
                if (!isFirst) // first line should be the header
                {

                    rtn.Add(line.ToList());
                }
                else
                {
                    isFirst = false;
                }

            }
            reader.Close();
            return rtn;
        }
    }
}
