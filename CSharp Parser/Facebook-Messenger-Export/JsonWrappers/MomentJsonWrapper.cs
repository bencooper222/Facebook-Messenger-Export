using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facebook_Messenger_Export
{
    class MomentJsonWrapper
    {

        public DateTime Time;
        public string ZoneId;
        public MomentJsonWrapper(Moment original)
        {
            Time = original.Time;
            ZoneId = original.Zone.Id;
        }
    }
}
