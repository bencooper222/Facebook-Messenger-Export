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

        /// <summary>
        /// Constructs the class from the plaintext of the div
        /// </summary>
        /// <param name="divText">The plaintext of the div</param>
        public Message(string divText)
        {

        }


    }
}
