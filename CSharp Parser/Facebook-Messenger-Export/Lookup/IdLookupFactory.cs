using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Facebook_Messenger_Export
{
    class IdLookupFactory
    {


        BiDictionaryOneToOne<string, LookupResult> idNames; // uid, name
        Random random;

        public IdLookupFactory(string importPath = null)
        {
            idNames = new BiDictionaryOneToOne<string, LookupResult>();
            if (importPath != null) LoadFromFile(importPath);
            random = new Random();
        }


        public void LoadFromFile(string path)
        {

            foreach (List<string> line in Utilities.ReadCSV(path))
            {
                bool isReal;
                bool.TryParse(line[2], out isReal);
                idNames.Add(line[0], new LookupResult(line[1], isReal));
            }

        }

        public void AddToCSV(string path)
        {

            List<string> uids = idNames.GetFirstKeys();
            List<LookupResult> names = idNames.GetSecondKeys();

            // super costly/shitty way of doing this
            IdLookupFactory duplicateCheck = new IdLookupFactory(path);



            for (int i = 0; i < idNames.Count; i++)
            {
                if (!duplicateCheck.ContainsUID(uids[i]))
                {
                    LookupResult name = names[i];
                    File.AppendAllText(path, uids[i] + "," + name.Name + "," + name.IsReal + Environment.NewLine);
                }

            }

        }

        private bool ContainsUID(string uid)
        {
            try
            {
                idNames.GetByFirst(uid);

            }
            catch (Exception e)
            {
                return false; // if it can't be found
            }
            return true;

        }

        private bool ContainsName(string name)
        {
            try
            {
                idNames.GetBySecond(new LookupResult(name, true)); // override means true/false shouldn't matter

            }
            catch (Exception e)
            {
                return false; // if it can't be found
            }
            return true;

        }


        public string GetUID(string name)
        {
            return idNames.GetBySecond(new LookupResult(name, true));
        }

        public string GetUID(LookupResult person)
        {
            return idNames.GetBySecond(person);
        }

        /// <summary>
        /// Change the UID by looking up by the name
        /// </summary>
        public void ChangeUID(string name,string newUid)
        {
            ChangeUID(new LookupResult(name, true),newUid);
        }

        /// <summary>
        /// Changes the UID by lookup up by a Lookup Result
        /// </summary>
        /// <param name="person"></param>
        public void ChangeUID(LookupResult person, string newUid)
        {
            idNames.ChangeFirstBySecond(person, newUid);
        }

        /// <summary>
        /// Changes the name associated with this UID
        /// </summary>
        /// <param name="uid"></param>
        public void ChangeName(string uid,LookupResult newName )
        {
            idNames.ChangeSecondByFirst(uid, newName);
        }

        /// <summary>
        /// Changes the name by looking up UID and creating a new LookupResultl
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="newName"></param>
        /// <param name="newIsReal"></param>
        public void ChangeName(string uid, string newName, bool newIsReal)
        {
            idNames.ChangeSecondByFirst(uid, new LookupResult(newName, newIsReal));
        }   

        /// <summary>
        /// Use Facebook Graph API to get Name from UID
        /// </summary>
        /// <returns>The name. Can return null if not locateable</returns>
        public LookupResult GetName(string uid)
        {
            LookupResult rtn;
            try
            {
                //   Console.WriteLine("No API request");
                rtn = idNames.GetByFirst(uid);
                return rtn;
            }
            catch
            {
                rtn = FBApiRequest(uid);

               try
                {
                    idNames.Add(uid, rtn); // prevent extraneous requests

                }
                catch
                {

                }
                    
               
               
                return rtn;
            }
        }

        private LookupResult FBApiRequest(string uid)
        {

            string logLocation = ConfigurationManager.AppSettings["private"] + @"\Logs\graph-api-logs.txt";
            string result;
            JObject response = new JObject();
            try
            {
                var url = "https://graph.facebook.com/v2.9/";
                // Facebook OAuth is wonkily implemented. I got this information from a Postman request. I don't believe it should be reusable but it is.
                var client = new RestClient(url + uid + "?oauth_token=" + ConfigurationManager.AppSettings["fbAccessToken"] +
                    "&oauth_signature_method=HMAC-SHA1 " +
                    "&oauth_timestamp=1494281247&oauth_nonce=NQBv1w&oauth_version=1.0&oauth_signature=fBcO9HTiU4gG%20FEHjnTRM5bFAbc%3D");
                var request = new RestRequest(Method.GET);
                request.Timeout = 1000;
                request.AddHeader("cache-control", "no-cache");
                response = JObject.Parse(client.Execute(request).Content);

                result = response["name"].ToString();

                File.AppendAllText(logLocation, DateTime.UtcNow + ": SUCCESS: " + uid + ": " + result + Environment.NewLine);

            }
            catch (Exception e)
            {
                File.AppendAllText(logLocation, DateTime.UtcNow + ": FAILURE: " + uid + ": " + e + ": " + response.ToString(Formatting.None) + Environment.NewLine);

                return new LookupResult("Unknown" + random.NextDouble(), false);


            }
            //logger.Close();


            return new LookupResult(result, true);
        }
    }
}
