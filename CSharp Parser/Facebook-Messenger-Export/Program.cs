using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using HtmlAgilityPack;
using System.Globalization;
using OAuth;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Facebook_Messenger_Export
{
    class Program
    { //http://www.nudoq.org/#!/Packages/HtmlAgilityPack/HtmlAgilityPack
        static void Main(string[] args)
        {


            
            
            HtmlDocument doc = new HtmlDocument();
            string threadId = "29";
            doc.Load(@"C:\Users\benco\Documents\Visual Studio 2015\Projects\Facebook-Messenger-Export\CSharp Parser\threads\" + threadId + ".html");

            IdLookupFactory factory = new IdLookupFactory();

            Thread test = new Thread(doc,0,factory);
            Console.WriteLine(test.MessagesToJson());
            
            
           

            /*
            doc.Load(ConfigurationManager.AppSettings["messageLocation"]);

            SeparateThreads(doc);
         
            */
            Console.Read();
        }

        /// <summary>
        /// Takes a bunch of thread div objects and create HTML documents with them inside it
        /// </summary>
        /// <param name="doc">The HTML document formatted like Facebook's export</param>
        private static void SeparateThreads(HtmlDocument doc)
        {
            HtmlNode documentNode = doc.DocumentNode;
            HtmlNode contents = doc.DocumentNode.Descendants("div").Where(d => d.Attributes["class"].Value == "contents").First();
            HtmlNode threadContainer = contents.Descendants("div").First();

            ThreadSeparator separate = new ThreadSeparator(threadContainer);
            separate.WriteThreads();
        }

        private static void WeirdGetOauth()
        {
            //https://stackoverflow.com/questions/4002847/oauth-with-verification-in-net
            var oauth = new OAuth.Manager();
            oauth["consumer_key"] = "1891815467765937";
            oauth["consumer_secret"] = "9b4f44d6f6a3f9369bcfd36bf3a407cb";
            oauth["token_secret"] = ConfigurationManager.AppSettings["fbAccessToken"];


            // now, update twitter status using that access token
            var appUrl = "https://graph.facebook.com/v2.9/4";
            var authzHeader = oauth.GenerateAuthzHeader(appUrl, "POST");
            var request = (HttpWebRequest)WebRequest.Create(appUrl);
            request.Method = "GET";
            request.PreAuthenticate = true;
            request.AllowWriteStreamBuffering = true;
            //        request.Headers.Add("Authorization", authzHeader);

            string editedAuthHeader = ("{" + authzHeader.Remove(0, 6) + "}").Replace("=", ":");
            JObject o = JObject.Parse(editedAuthHeader);

            Console.WriteLine(" ");
            Console.WriteLine(o.ToString());

            Utilities.AddJsonToWebRequest(request, o);


            request.Headers.Add("oauth_token", ConfigurationManager.AppSettings["fbAccessToken"]);
            Console.WriteLine(JsonConvert.SerializeObject(request.Headers));

            //    request.Headers.Remove("Host");
            //  request.Headers.Remove("Connection");
            Console.WriteLine(request);
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine("There's been a problem trying to tweet:" + response.StatusDescription);
                    }
                    else
                    {
                        Console.Write(response.ToString());
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }

    
}   
