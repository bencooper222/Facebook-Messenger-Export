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

        public IdLookupFactory(string importPath = null)
        {
            idNames = new BiDictionaryOneToOne<string, string>();
            if(importPath!=null) LoadFromFile(importPath); 
        }


        public void LoadFromFile(string path)
        {

            foreach(List<string> line in Utilities.ReadCSV(path))
            {
                idNames.Add(line[0], line[1]);
            }
            
        }
        
        public void AddToCSV(string path)
        {
            
            List<string> uids = idNames.GetFirstKeys();
            List<string> names = idNames.GetSecondKeys();

            // super costly/shitty way of doing this
            IdLookupFactory duplicateCheck = new IdLookupFactory(path);
           
         

            for (int i=0; i < idNames.Count; i++)
            {
                if (!duplicateCheck.ContainsUID(uids[i]))
                {
                    File.AppendAllText(path, uids[i] + "," + names[i] + Environment.NewLine);
                }
                
            }
           
        }

        private bool ContainsUID(string uid)
        {
            try
            {
                idNames.GetByFirst(uid);
                
            }
            catch(Exception e)
            {
                return false; // if it can't be found
            }
            return true;
            
        }

        private bool ContainsName(string name)
        {
            try
            {
                idNames.GetBySecond(name);

            }
            catch (Exception e)
            {
                return false; // if it can't be found
            }
            return true;

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
                Console.WriteLine("No API request");
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

            string logLocation = ConfigurationManager.AppSettings["private"] + @"\Logs\graph-api-logs.txt";
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
