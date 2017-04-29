using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using HtmlAgilityPack;
using System.Globalization;

namespace Facebook_Messenger_Export
{
    class Program
    { //http://www.nudoq.org/#!/Packages/HtmlAgilityPack/HtmlAgilityPack
        static void Main(string[] args)
        {
            /*
            HtmlDocument doc = new HtmlDocument();
            doc.Load(@"C:\Users\benco\Documents\Visual Studio 2015\Projects\Facebook-Messenger-Export\threads\0.html");


            Thread test = new Thread(doc);
            
            */
            DateTimeOffset time = DateTime.ParseExact("Monday, January 2, 2017 at 11:56pm", "dddd, MMMM d, yyyy 'at' h:mmtt", CultureInfo.InvariantCulture);
             Console.WriteLine(time);
            DateTimeOffset date1 = new DateTime(2017, 1, 2, 23, 56,0);
      
            Console.WriteLine(date1.ToString("dddd, MMMM d, yyyy 'at' h:mmtt K"));
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
