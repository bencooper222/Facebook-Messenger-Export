using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Facebook_Messenger_Export
{
    class Person
    {

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string UID { get; }
        public int Count { // total messages sent
            get
            {
                return count;
            }
        }
        private int count;
        private string name;

        public Person(string uid,string name = null)
        {
            this.name = name;
            UID = uid;
        }

        /// <summary>
        /// Increases the total messages sent by this person
        /// </summary>
        public void AddMessageCount()
        {
            count++;
    //     DotNetOpenAuth.OAuth.ChannelElements.   
        }

        


    }
}
