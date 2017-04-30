using Newtonsoft.Json;
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
        public Moment Time { get; }
        public string SenderID { get;}
        public string SenderName { get; }
        public int ThreadId { get; } 
        

        /// <summary>
        /// Default constructor
        /// </summary>
        public Message()
        {
            Text = "";
            Time = new Moment();
            SenderID = "";
            SenderName = "";
            ThreadId = -1; // not sure what the default should be
        }


       

        public Message(string text, Moment time, string senderID,int threadId)
        {
            Text = text;
            Time = time;
            SenderID = senderID;
            SenderName = null; // worry about later
            ThreadId = threadId;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void GetName



    }
}
