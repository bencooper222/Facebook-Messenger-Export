using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Messenger_Export
{
    class LookupResult
    {

        public string Name;
        public bool IsReal { get; }
        


        public LookupResult(string name, bool isReal)
        {
            
            Name = name;
            
            IsReal = isReal;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode(); // this is a great idea
        }



        public override bool Equals(object obj)
        {

            if (!(obj is LookupResult))
            {
                return false;
            }

            if (this == (LookupResult)obj)
            {
                return true;
            }

            return false;
        }

        public static bool operator ==(LookupResult one, LookupResult two)
        {
            if (one.Name == two.Name)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool operator !=(LookupResult one, LookupResult two)
        {
            return !(one == two);
        }
    }

}

