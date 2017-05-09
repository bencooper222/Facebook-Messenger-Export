using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Messenger_Export
{
     class IdLookupFactory
    {


        BiDictionaryOneToOne<string, string> idNames; // uid, name

        public IdLookupFactory()
        {
            idNames = new BiDictionaryOneToOne<string, string>();  
        }


        public void LoadFromFile(string path)
        {
            StreamReader reader = new StreamReader(path);

            while (reader.Peek() >= 0) // creds to http://www.sanfoundry.com/csharp-program-read-lines-until-end-file/
            {
                string[] line = reader.ReadLine().Split(',');
                idNames.Add(line[0], line[1]);
            }

        }
        
        public void CreateCSV(string path)
        {
            StreamWriter writer = new StreamWriter(path);
            List<string> uids = idNames.GetFirstKeys();
            List<string> names = idNames.GetSecondKeys();

            writer.WriteLine("UID,Name");

            for (int i=0; i < idNames.Count; i++)
            {
                writer.WriteLine(uids[i] + "," + names[i]);
            }
        }


        public string GetUID(string name)
        {
            return idNames.GetBySecond(name);
        }
        /// <summary>
        /// Use Facebook Graph API to get Name from UID
        /// </summary>
        /// <returns>The name. Can return null if not locateable</returns>
        public string GetName(string uid)
        {
            string rtn;
            try
            {
                rtn = idNames.GetByFirst(uid);
                return rtn; 
            }
            catch 
            {
                rtn =  FBApiRequest(uid);
                idNames.Add(uid, rtn); // prevent extraneous requests
                return rtn;
            }
        }

        private string FBApiRequest(string uid)
        {

            string logLocation = ConfigurationManager.AppSettings["logLocation"] + @"\graph-api-logs.txt";
            string result;
            try
            {
                var url = "https://graph.facebook.com/v2.9/";
                // Facebook OAuth is wonkily implemented. I got this information from a Postman request. I don't believe it should be reusable but it is.
                var client = new RestClient(url + uid + "?oauth_token=" + ConfigurationManager.AppSettings["fbAccessToken"] + "&oauth_signature_method=HMAC-SHA1 " + "&oauth_timestamp=1494281247&oauth_nonce=NQBv1w&oauth_version=1.0&oauth_signature=fBcO9HTiU4gG%20FEHjnTRM5bFAbc%3D");
                var request = new RestRequest(Method.GET);
                request.Timeout = 1000;
                request.AddHeader("cache-control", "no-cache");
                JObject response = JObject.Parse(client.Execute(request).Content);

                result = response["name"].ToString();
                
                File.AppendAllText(logLocation, DateTime.UtcNow + ": SUCCESS: " + uid + ": " + result + Environment.NewLine);

            }
            catch (Exception e)
            {
                File.AppendAllText(logLocation, DateTime.UtcNow + ": FAILURE: " + uid + ": " + e + Environment.NewLine);

                return null;

               
            }
            //logger.Close();


            return result;
        }
    }
}
