using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Messenger_Export
{
    class Person
    {

        string Name { get; }
        string UID { get; }
        public int Count { // total messages sent
            get
            {
                return count;
            }
        }
        private int count;

        public Person(string uid,string name = null)
        {
            Name = name;
            UID = uid;
        }

        /// <summary>
        /// Increases the total messages sent by this person
        /// </summary>
        public void AddMessageCount()
        {
            count++;
            
        }

        
    }
}
