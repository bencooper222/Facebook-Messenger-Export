﻿using RestSharp;
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
            set
            {
                
                name = value;
            }
        }

        public string UID { get; }
        public bool RealName { get; set; } // whether or not the name was actually queried or just randomly generated
    
        private int count;
        private string name;
     

        public Person(string uid,string name = null)
        {
            
            this.name = name;
            UID = uid;
           
        }

   

        


    }
}
