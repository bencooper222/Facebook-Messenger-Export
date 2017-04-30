using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using TimeZoneNames;

namespace Facebook_Messenger_Export
{
    class Moment
    {
        public DateTime Time { get; }
        public TimeZoneInfo Zone {get;}


        /// <summary>
        /// Creates Moment object contianing time and timezone
        /// Date format resource below
        /// https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx
        /// </summary>
        /// <param name="timeString">String with time. Must exactly match "dddd, MMMM d, yyyy 'at' h:mmtt" (see above link)</param>
        /// <param name="zoneString">Simple abbreviation like "EST"</param>
        public Moment(string timeString, string zoneString)
        {
            IDictionary<string, string> something = TZNames.GetFixedTimeZoneAbbreviations("en-US");
            Time = DateTime.ParseExact(timeString, "dddd, MMMM d, yyyy 'at' h:mmtt", CultureInfo.InvariantCulture); 
            Zone = TimeZoneInfo.FindSystemTimeZoneById(zoneString);
            
        }

        public Moment()
        {
            Time = new DateTime();
            Zone = TimeZoneInfo.FindSystemTimeZoneById("America/Chicago"); // my timezone so eh?
            
        }
   
    }
}
