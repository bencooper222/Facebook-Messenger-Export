using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Messenger_Export
{
    class MessageJsonWrapper
    {

        public string Text { get; }
        public MomentJsonWrapper SentTime { get; }
        public string SenderID { get; }
        public string SenderName { get; }
        public int ThreadId { get; }

        public MessageJsonWrapper(Message original)
        {
            Text = original.Text;
            SenderID = original.SenderID;
            SenderName = original.SenderName;
            ThreadId = original.ThreadId;

            SentTime = new MomentJsonWrapper(original.SentTime);
        }
    }
}
