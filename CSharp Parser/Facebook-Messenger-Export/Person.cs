using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Facebook_Messenger_Export
{
    class Person
    {

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string UID { get; }
        public int Count { // total messages sent
            get
            {
                return count;
            }
        }
        private int count;
        private string name;

        public Person(string uid,string name = null)
        {
            this.name = name;
            UID = uid;
        }

        /// <summary>
        /// Increases the total messages sent by this person
        /// </summary>
        public void AddMessageCount()
        {
            count++;
    //     DotNetOpenAuth.OAuth.ChannelElements.   
        }

        /// <summary>
        /// Use Facebook Graph ID to get Name
        /// </summary>
        /// <returns>Boolen for success or failure</returns>
        public bool GetName()
        {
          //  StreamWriter logger = new StreamWriter(ConfigurationManager.AppSettings["logLocation"]+ @"\graph-api-logs.txt");
            try
            {
                var url = "https://graph.facebook.com/v2.9/";
                var client = new RestClient(url + UID + "?oauth_token=" + ConfigurationManager.AppSettings["fbAccessToken"] + "&oauth_signature_method=HMAC-SHA1 " + "&oauth_timestamp=1494281247&oauth_nonce=NQBv1w&oauth_version=1.0&oauth_signature=fBcO9HTiU4gG%20FEHjnTRM5bFAbc%3D");
                var request = new RestRequest(Method.GET);
                request.Timeout = 1000;
                request.AddHeader("cache-control", "no-cache");
                JObject response = JObject.Parse(client.Execute(request).Content);

                name = response["name"].ToString();
                File.AppendAllText(ConfigurationManager.AppSettings["logLocation"] + @"\graph-api-logs.txt", DateTime.UtcNow + ": SUCCESS: " + UID + ": " + Name + Environment.NewLine);
              
            }
            catch (Exception e)
            {
                File.AppendAllText(ConfigurationManager.AppSettings["logLocation"] + @"\graph-api-logs.txt", DateTime.UtcNow + ": FAILURE: " + UID + ": " + e + Environment.NewLine);

   
           
                return false;   
            }
            //logger.Close();
            

            return true;

         

        }


    }
}
