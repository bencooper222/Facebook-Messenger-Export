﻿using Newtonsoft.Json;
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
        public Moment SentTime { get; }
        public string SenderID { get;}
        public string SenderName { get; set; }
        public int ThreadId { get; } 
        

        /// <summary>
        /// Default constructor
        /// </summary>
        public Message()
        {
            Text = "";
            SentTime = new Moment();
            SenderID = "";
            SenderName = "";
            ThreadId = -1; // not sure what the default should be
        }


       

        public Message(string text, Moment time, int threadId, string senderID, string senderName = null)
        {
            Text = text;
            SentTime = time;
            SenderID = senderID;
            SenderName = senderName; // worry about later
            ThreadId = threadId;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(new MessageJsonWrapper(this));
        }

    //    public void GetName



    }
}
