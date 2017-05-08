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


        Dictionary<string, string> idNames;

        public IdLookupFactory()
        {
            idNames = new Dictionary<string, string>(); 
        }

        /// <summary>
        /// Use Facebook Graph ID to get Name
        /// </summary>
        /// <returns>The name. Is empty string if it can't locate it</returns>
        public string GetName(string uid)
        {
            string rtn;
            try
            {
                rtn = idNames[uid];
                return rtn; 
            }
            catch 
            {
                rtn =  FBApiRequest(uid);
                idNames.Add(uid, rtn);
                return rtn;
            }
        }

        private string FBApiRequest(string uid)
        {

            string logLocation = ConfigurationManager.AppSettings["logLocation"] + @"\graph-api-logs.txt";
            string name;
            try
            {
                var url = "https://graph.facebook.com/v2.9/";
                // Facebook OAuth is wonkily implemented. I got this information from a Postman request. I don't believe it should be reusable but it is.
                var client = new RestClient(url + uid + "?oauth_token=" + ConfigurationManager.AppSettings["fbAccessToken"] + "&oauth_signature_method=HMAC-SHA1 " + "&oauth_timestamp=1494281247&oauth_nonce=NQBv1w&oauth_version=1.0&oauth_signature=fBcO9HTiU4gG%20FEHjnTRM5bFAbc%3D");
                var request = new RestRequest(Method.GET);
                request.Timeout = 1000;
                request.AddHeader("cache-control", "no-cache");
                JObject response = JObject.Parse(client.Execute(request).Content);

                name = response["name"].ToString();
                File.AppendAllText(logLocation, DateTime.UtcNow + ": SUCCESS: " + uid + ": " + Name + Environment.NewLine);

            }
            catch (Exception e)
            {
                File.AppendAllText(logLocation, DateTime.UtcNow + ": FAILURE: " + uid + ": " + e + Environment.NewLine);

                return "";

               
            }
            //logger.Close();


            return name;
        }
    }
}
