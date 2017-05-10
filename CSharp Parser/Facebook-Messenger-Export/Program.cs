using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using HtmlAgilityPack;
using System.Globalization;

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
            string threadId = "10";
            string privateLocation = ConfigurationManager.AppSettings["private"];
            doc.Load(privateLocation + @"\threads\" + threadId + ".html");

            IdLookupFactory factory = new IdLookupFactory(privateLocation+@"\idNames.csv");

            Thread test = new Thread(doc,0,factory);
            test.WriteJsonToFile(privateLocation + @"\jsons\" + threadId + ".json",true);
            factory.AddToCSV(privateLocation + @"\idNames.csv");
            
            /*
            
            Encoding win52 = Encoding.GetEncoding("windows-1252");
            Encoding unicode = Encoding.UTF8;

           // string s = "Ã°Å¸ÂÂ»";
          string s = "Ã°Å¸Â\u008dÂ»";
            string normal = "Hello!";
            
            byte[] messageBytes = unicode.GetBytes(s);
            byte[] beer = unicode.GetBytes("🍻");

           
            */

          

            

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

        
    }

    
}   
