using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Messenger_Export
{
    class Message
    {
        public string Text { get; }
        public DateTime Time { get; }
        public string SenderID { get;}
        public string SenderName { get; }
        public string ThreadId { get; } 
        

        /// <summary>
        /// Default constructor
        /// </summary>
        public Message()
        {
            Text = "";
            Time = new DateTime();
            SenderID = "";
            SenderName = "";
            ThreadId = "";
        }


       

        public Message(string text, string date, string senderID,string threadId)
        {
            Text = text;
            Time = ParseDate(date);
            SenderID = senderID;
            SenderName = null; // worry about later
            ThreadId = threadId;
        }

        /// <summary>
        /// Takes the date string FB provides and turns it into a DateTime object
        /// </summary>
        /// <param name="date">The (bad) DateTime string FB provides</param>
        /// <returns></returns>
        private DateTime ParseDate(string date)
        {
            return new DateTime();
        }


    }
}
