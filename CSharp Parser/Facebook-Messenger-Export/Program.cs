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
using System.IO;

namespace Facebook_Messenger_Export
{
    class Program
    { 
        static void Main(string[] args)
        {


            
            
            HtmlDocument all = new HtmlDocument();
            string privateLocation = ConfigurationManager.AppSettings["private"];
            /*

            all.Load(privateLocation + "/messages.htm");
            Console.WriteLine("Doc loaded");

            int totalThreads = SeparateThreads(all);
            Console.WriteLine("Separation complete: " + totalThreads);

            */

            
            IdLookupFactory factory = new IdLookupFactory(privateLocation + @"\idNames.csv");
            factory.GetUID("Ben Cooper");
            
            int totalThreads = Directory.GetFiles(privateLocation + @"\threads").Length;
            
            /*
            int threadId = 2;
            HtmlDocument thread = new HtmlDocument();
            thread.Load(privateLocation + @"\threads\" + threadId.ToString() + ".html");
            Thread current = new Thread(thread, threadId, factory);
            current.WriteJsonToFile(privateLocation + @"\jsonTests\" + threadId.ToString() + ".json", true);
            */

            Console.WriteLine("Begin thread to JSON");
            for (int i=0; i<totalThreads; i++)
            {
                HtmlDocument thread = new HtmlDocument();
             //   Console.WriteLine(i + " 1");
                thread.Load(privateLocation + @"\threads\" + i.ToString() + ".html");
               // Console.WriteLine(i + " 2");
                Thread current = new Thread(thread, i, factory);
                //Console.WriteLine(i + " 3");
                current.WriteJsonToFile(privateLocation + @"\jsons\" + i + ".json", true);
                //Console.WriteLine(i + " 4");

                if (i % 20 == 0) // just to document progress
                {
                    Console.WriteLine((double)i / totalThreads * 100 + " % complete");
                }
            }
            
            
            factory.AddToCSV(privateLocation + @"\idNames.csv");
            


            
            Console.Read();
        }

        /// <summary>
        /// Takes a bunch of thread div objects and create HTML documents with them inside it
        /// Returns total threads
        /// </summary>
        /// <param name="doc">The HTML document formatted like Facebook's export</param>
        private static int SeparateThreads(HtmlDocument doc)
        {
            HtmlNode documentNode = doc.DocumentNode;
            HtmlNode contents = doc.DocumentNode.Descendants("div").Where(d => d.Attributes["class"].Value == "contents").First();

           
            ThreadSeparator separate = new ThreadSeparator(null);


            bool isFirst = true; // first node is an h1
            int total = 0;
            foreach(HtmlNode node in contents.ChildNodes)
            {
                if (!isFirst)
                {
                    // worry about later - iterate through each node
                    separate.ThreadContainer = node;
                    total += separate.WriteThreads();
                }
                else
                {
                    isFirst = false;
                }
            }

            return total;
        }

        
    }

    
}   
