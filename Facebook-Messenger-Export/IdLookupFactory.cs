using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Messenger_Export
{
    class IdLookupFactory
    {
        private string FbAPIKey;
        public IdLookupFactory(string fbApiKey)
        {
            FbAPIKey = fbApiKey;

        }

        /// <summary>
        /// Returns the names of people from Facebook
        /// </summary>
        /// <param name="ids">List of IDs of people to query</param>
        /// <returns></returns>
        public List<Person> LookupFbIds(List<String> ids)
        {
            return new List<Person>();
        }
    }
}
