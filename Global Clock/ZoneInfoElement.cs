using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Clock
{
    public class ZoneInfoElement
    {
        public string name;
        public TimeZoneInfo zoneInfo;
        public ZoneInfoElement(string name, TimeZoneInfo zoneInfo)
        {
            this.name = name;
            this.zoneInfo = zoneInfo;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
