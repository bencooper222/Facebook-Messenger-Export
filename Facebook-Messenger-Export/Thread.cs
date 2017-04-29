using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Facebook_Messenger_Export
{
    class Thread
    {
        List<Message> Messages { get; } // is this the best data structure?
        List<Person> Participants { get; }

        /// <summary>
        /// Default contstructor
        /// </summary>
        /// <param name="participants">The list of participants (parsed elsewhere)</param>
        public Thread(List<Person> participants)
        {
            Messages = new List<Message>();
            Participants = participants;
        }

        public Thread(HtmlDocument thread)
        {
            HtmlNode body = thread.DocumentNode.ChildNodes.First().ChildNodes.ToList()[0]; // annoying to extract
            Participants = CreateListOfParticipants(body.FirstChild.InnerText);
            

        }

        /// <summary>
        /// Helper method to construct Participants object
        /// </summary>
        /// <param name="text">Plain text from Facebook</param>
        /// <returns>List of Person objects for each ID</returns>
        private List<Person> CreateListOfParticipants(string text)
        {
            string[] ids = Regex.Split(text, ", ");
            //DateTime time = new DateTime();
           // time.   
            List<Person> rtn = new List<Person>();
            foreach(string s in ids)
            {
                rtn.Add(new Person(s.Trim()));
            }
            return rtn;
        }


        /// <summary>
        /// Creates a Message object from the two HTML tags necessary to do so
        /// See the README for an explanation so you know what I'm talking about
        /// </summary>
        /// <param name="messageHeader">The tag with class message_header</param>
        /// <param name="pStuff">Stuff in the p (the actual message)</param>
        /// <returns>A nice message object with the information</returns>
        private Message AddMessage(HtmlNode messageHeader, HtmlNode paragraphTag)
        {
            //   Message message = new Message(paragraphTag.InnerHtml,)
            return new Message();
        }


        /// <summary>
        /// Adds a message to the thread
        /// </summary>
        /// <param name="msg"></param>
        public void AddMessage(Message msg)
        {

        }
    }
}
