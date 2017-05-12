using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Configuration;

namespace Facebook_Messenger_Export
{
    class ThreadSeparator
    {
        public HtmlNode ThreadContainer { get; set; }
        private int internalIndex;
    
        public ThreadSeparator(HtmlNode parentDiv)
        {
            ThreadContainer = parentDiv;
            internalIndex = 0;
            
        }

        /// <summary>
        /// Creates a new plaintext HTML document from a single HTML node
        /// </summary>
        /// <param name="node">The node that will be the document</param>
        /// <param name="id">The name of the file (minus the file extension)</param>
        private void WriteThread(HtmlNode node, int id)
        {
            using (StreamWriter writer = new StreamWriter(ConfigurationManager.AppSettings["private"] + @"\threads\"+ id + ".html"))
            {
                writer.Write("<html><body>");                
                writer.Write(node.InnerHtml);
                writer.Write("</body></html>");
            }
        }
        
        /// <summary>
        /// Itereates through ThreadContainer and writes each Thread in it to a separate document
        /// Returns total threads
        /// </summary>
        public int WriteThreads()
        {
            int index = 0; // watch out!
            
            HtmlNodeCollection nodes = ThreadContainer.ChildNodes;
            int count = nodes.Count;
            foreach (HtmlNode node in nodes)
            {
                if (index % 2 != 0)
                {
                    WriteThread(node, (internalIndex-1)/2); // every other one in here will be invalid so this adjusts for that
                }
                
                index++;
                internalIndex++;
                if (index % 10 == 0) // just to document progress
                {
                    Console.WriteLine((double)index / count * 100 + " % complete");
                }

               
            }
         
            return (index-1)/2;
        }

    }
}
