using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Globalization;
using Newtonsoft.Json;
using System.IO;

namespace Facebook_Messenger_Export
{
    class Thread
    {
        public List<Message> Messages { get; } // is this the best data structure?
        public List<Person> Participants { get; }
        public int UID { get; }
        private IdLookupFactory Lookup;

        /// <summary>
        /// Default contstructor
        /// </summary>
        /// <param name="participants">The list of participants (parsed elsewhere)</param>
        public Thread(List<Person> participants,IdLookupFactory lookup )
        {
            Messages = new List<Message>();
            Participants = participants;
            Lookup = lookup;
        }

        public Thread(HtmlDocument thread, int id, IdLookupFactory lookup)
        {
            UID = id;
            Lookup = lookup;

            bool DoLookup = false; // should query for names?
            if(Lookup != null)
            {
                DoLookup = true;
            }



            HtmlNode body = thread.DocumentNode.FirstChild.ChildNodes.Where(w => w.Name == "body").ToList()[0];
            List<HtmlNode> nodes = body.ChildNodes.Where(w => w.Name != "#text").ToList(); // eliminates whitespace text nodes

            Participants = CreateListOfParticipants(body.FirstChild.InnerText,DoLookup);

            Messages = new List<Message>();
            for (int i= 0; i< nodes.Count; i+=2){
                AddMessage(nodes[i].ChildNodes[1], nodes[i + 1]);
            }
            Messages.Reverse(); // because FB gives it most recent to least recent
        }

        /// <summary>
        /// Helper method to construct Participants object
        /// </summary>
        /// <param name="text">Plain text from Facebook</param>
        ///  <param name="getNames">Whether or not to make requests to Facebook for all the names</param>
        /// <returns>List of Person objects for each ID</returns>
        private List<Person> CreateListOfParticipants(string text, bool getNames = false)
        {
            string[] ids = Regex.Split(text, ", ");
            //DateTime time = new DateTime();
           // time.   
            List<Person> rtn = new List<Person>();
            foreach(string s in ids)
            {
                string uidNoWhitespace = s.Trim();
                string uid = uidNoWhitespace.Remove(uidNoWhitespace.Length - 13, 13);
                Person person = new Person(uid);
                if (getNames)
                {
                    person.Name = Lookup.GetName(uid);
                }
                
                rtn.Add(person);
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
            string facebookIdentifier = attributes[0].InnerText; // this could be in the form of UID@facebook.com or just simple first last name

            // process time (second tag)
            string rawTime = attributes[1].InnerText;
            Moment time = new Moment(rawTime.Remove(rawTime.Length - 4, 4), rawTime.Remove(0, rawTime.Length - 3));

            string senderId;
            string senderName;
            if (facebookIdentifier.Contains("@"))
            {
                // if it's in the form of UID@facebook.com
                 senderId = facebookIdentifier.Remove(facebookIdentifier.Length - 13, 13);
                senderName = Lookup.GetName(senderId); 
            }
            else
            {
                // if it's just first and last name
                senderId = Lookup.GetUID(facebookIdentifier); // should have already been added at the beginning of the thread
                senderName = facebookIdentifier;
            }
            
           
           
            Messages.Add(new Message(paragraphTag.InnerText, time, UID, senderId, senderName));                           
        }
        

        public string MessagesToJson(bool prettify =false)
        {
            List<MessageJsonWrapper> messageJsons = new List<MessageJsonWrapper>();

            // use Wrappers to advantage
            foreach(Message m  in Messages)
            {
                messageJsons.Add(new MessageJsonWrapper(m));
            }

            if (prettify)
            {
                return JsonConvert.SerializeObject(messageJsons,Formatting.Indented);
            }
            else
            {
                return JsonConvert.SerializeObject(messageJsons);
            }
           
        }

        public void WriteJsonToFile(string path,bool prettify = false)
        {
            StreamWriter writer = new StreamWriter(path);
            writer.Write(MessagesToJson(prettify));
            writer.Close();
        }

       
    }
}
