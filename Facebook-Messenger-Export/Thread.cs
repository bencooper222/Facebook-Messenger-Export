using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        /// <summary>
        /// Adds a message to the thread
        /// </summary>
        /// <param name="msg"></param>
        public void AddMessage(Message msg)
        {

        }
    }
}
