using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using HtmlAgilityPack;

namespace Facebook_Messenger_Export
{
    class Program
    { //http://www.nudoq.org/#!/Packages/HtmlAgilityPack/HtmlAgilityPack/HtmlNode
        static void Main(string[] args)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(ConfigurationManager.AppSettings["messageLocation"]);
            Console.WriteLine("pause");
         
            doc = null;
            GC.Collect();

            Console.WriteLine("Hello");
            Console.Read();
        }
    }
}   
