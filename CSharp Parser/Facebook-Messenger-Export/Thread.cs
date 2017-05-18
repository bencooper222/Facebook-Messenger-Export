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
using System.Configuration;
using Facebook_Messenger_Export.JsonWrappers;

namespace Facebook_Messenger_Export
{
    class Thread
    {
        public List<Message> Messages { get; } // is this the best data structure?
        public List<Person> Participants { get; }
        public int UID { get; }
        private IdLookupFactory Lookup;
        private Random random;

        private int totalReal
        {
            get
            {
                int total = 0;
                foreach (Person p in Participants)
                {
                    if (p.RealName) total++;
                }

                return total; ;
            }
        }


        /// <summary>
        /// Default contstructor
        /// </summary>
        /// <param name="participants">The list of participants (parsed elsewhere)</param>
        public Thread(List<Person> participants, IdLookupFactory lookup)
        {
            Messages = new List<Message>();
            Participants = participants;
            Lookup = lookup;
        }

        public Thread(HtmlDocument thread, int id, IdLookupFactory lookup)
        {
            UID = id;
            Lookup = lookup;

            random = new Random();

            bool DoLookup = false; // should query for names?
            if (Lookup != null)
            {
                DoLookup = true;
            }



            HtmlNode body = thread.DocumentNode.FirstChild.ChildNodes.Where(w => w.Name == "body").ToList()[0];
            List<HtmlNode> nodes = body.ChildNodes.Where(w => w.Name != "#text").ToList(); // eliminates whitespace text nodes

            Participants = CreateListOfParticipants(body.FirstChild.InnerText, DoLookup);

            Messages = new List<Message>();
            for (int i = 0; i < nodes.Count; i += 2)
            {
                AddMessage(nodes[i].ChildNodes[0], nodes[i + 1]); // should be 1 in childnodes?
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
            foreach (string s in ids)
            {
                string uidNoWhitespace = Utilities.CleanEmailAddress(s.Trim());
                string uid = uidNoWhitespace.Remove(uidNoWhitespace.Length - 13, 13);
                Person person = new Person(uid);
                if (getNames)
                {
                    LookupResult search = Lookup.GetName(uid);
                    person.Name = search.Name;
                    person.RealName = search.IsReal;

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
            string facebookIdentifier = Utilities.CleanEmailAddress(attributes[0].InnerText); // this could be in the form of UID@facebook.com or just simple first last name

            // process time (second tag)
            string rawTime = attributes[1].InnerText;
            Moment time = new Moment(rawTime.Remove(rawTime.Length - 4, 4), rawTime.Remove(0, rawTime.Length - 3));

            string senderId;
            string senderName;
            if (facebookIdentifier.Contains("@"))
            {
                // if it's in the form of UID@facebook.com
                senderId = facebookIdentifier.Remove(facebookIdentifier.Length - 13, 13);
                senderName = Lookup.GetName(senderId).Name;
            }
            else
            {
                // if it's just first and last name

                try
                {
                    senderId = Lookup.GetUID(facebookIdentifier); // should have already been added at the beginning of the thread
                }
                catch (Exception e)
                {
                    if (Participants.Count - totalReal == 1)
                    {
                        // change participants list
                        Person unknown = Participants.Where(p => p.Name.Contains("Unknown")).ToList()[0];
                        unknown.Name = facebookIdentifier;
                        unknown.RealName = true;

                        // change Lookupfactory
                        Lookup.ChangeName(unknown.UID, new LookupResult(facebookIdentifier, true));
                        senderId = unknown.UID;

                    }
                    else
                    {
                        senderId = Math.Floor((random.NextDouble() * 100000)).ToString();
                    }

                }




                senderName = facebookIdentifier;
            }

            // fixes emojis coded in with hex values and those simply represented by plain text like :)
            string messageText = ReplacePlainTextEmojis(FixEmojiEncoding(paragraphTag.InnerText));

            Messages.Add(new Message(messageText, time, UID, senderId, senderName));
        }

        // win 1252 -> uni = r
        // win 1252 (r) -> uni = final
        private string FixEmojiEncoding(string s)
        {
            Encoding wind1252 = Encoding.GetEncoding(1252);
            Encoding utf8 = Encoding.UTF8;
            byte[] wind1252Bytes = wind1252.GetBytes(s);
            byte[] utf8Bytes = Encoding.Convert(utf8, wind1252, wind1252Bytes);

            return utf8.GetString(utf8Bytes);


        }

        private string ReplacePlainTextEmojis(string s)
        {
            List<List<string>> emojis = Utilities.ReadCSV(ConfigurationManager.AppSettings["private"] + "/emojimap.csv");

            foreach (List<string> pair in emojis)
            {
                s.Replace(pair[0], pair[1]);
            }
            return s;
        }


        public string MessagesToJson(bool prettify = false)
        {
            List<MessageJsonWrapper> messageJsons = new List<MessageJsonWrapper>();

            // use Wrappers to advantage
            foreach (Message m in Messages)
            {
                messageJsons.Add(new MessageJsonWrapper(m));
            }

            if (prettify)
            {
                return JsonConvert.SerializeObject(messageJsons, Formatting.Indented);
            }
            else
            {
                return JsonConvert.SerializeObject(messageJsons);
            }

        }

        public void WriteMessageJsonToFile(string path, bool prettify = false)
        {
            StreamWriter writer = new StreamWriter(path);
            writer.Write(MessagesToJson(prettify));
            writer.Close();
        }

        public void WriteToManifest(string path, bool prettify = false, bool last = false)
        {
            string json;
            if (prettify)
            {
                json = JsonConvert.SerializeObject(new ThreadJsonWrapper(this), Formatting.Indented);

            }
            else
            {
                json = JsonConvert.SerializeObject(new ThreadJsonWrapper(this), Formatting.None);
            }

            if (!last)
            {
                File.AppendAllText(path, json + "," + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(path, json);
            }
        }
    }


}

