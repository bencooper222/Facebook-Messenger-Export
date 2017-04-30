using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Globalization;
using Newtonsoft.Json;

namespace Facebook_Messenger_Export
{
    class Thread
    {
        public List<Message> Messages { get; } // is this the best data structure?
        public List<Person> Participants { get; }
        public int UID { get; }

        /// <summary>
        /// Default contstructor
        /// </summary>
        /// <param name="participants">The list of participants (parsed elsewhere)</param>
        public Thread(List<Person> participants)
        {
            Messages = new List<Message>();
            Participants = participants;
        }

        public Thread(HtmlDocument thread, int id)
        {
            UID = id;



            HtmlNode body = thread.DocumentNode.FirstChild.ChildNodes.Where(w => w.Name == "body").ToList()[0];
            List<HtmlNode> nodes = body.ChildNodes.Where(w => w.Name != "#text").ToList(); // eliminates whitespace text nodes

            Participants = CreateListOfParticipants(body.FirstChild.InnerText);

            Messages = new List<Message>();
            for (int i= 0; i< nodes.Count; i+=2){
                AddMessage(nodes[i].ChildNodes[1], nodes[i + 1]);
            }

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
        private void AddMessage(HtmlNode messageHeader, HtmlNode paragraphTag)
        {
            HtmlNodeCollection attributes = messageHeader.ChildNodes;

            // first tag
            string facebookEmail = attributes[0].InnerText;

            // process time (second tag)
            string rawTime = attributes[1].InnerText;
            Moment time = new Moment(rawTime.Remove(rawTime.Length - 4, 4), rawTime.Remove(0, rawTime.Length - 3));

            Message message = new Message(paragraphTag.InnerText,time,facebookEmail.Remove(facebookEmail.Length-13,13),UID);
            Messages.Add(message);                           
        }
        

        public string MessagesToJson()
        {
            List<MessageJsonWrapper> messageJsons = new List<MessageJsonWrapper>();

            // use Wrappers to advantage
            foreach(Message m  in Messages)
            {
                messageJsons.Add(new MessageJsonWrapper(m));
            }
            return JsonConvert.SerializeObject(messageJsons);
        }

       
    }
}
