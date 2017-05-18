using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Messenger_Export.JsonWrappers
{
    class ThreadJsonWrapper
    {

        public List<Person> Participants;
        public int UID;

        public MomentJsonWrapper Start;
        public MomentJsonWrapper End;

        public ThreadJsonWrapper(Thread initial)
        {
            Participants = initial.Participants;
            UID = initial.UID;

            Start = new MomentJsonWrapper(initial.Messages[0].SentTime);
            End = new MomentJsonWrapper(initial.Messages[initial.Messages.Count-1].SentTime);
        }
    }   
}
