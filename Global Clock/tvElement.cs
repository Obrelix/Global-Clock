using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global_Clock
{
    class tvElement
    {
        public TimeSpan BaseUTCOffset { get; set; }
        public string name { get; set; }
        public List<TimeZoneInfo> childList { get; set; }
        public tvElement(TimeSpan BaseUTCOffset, string name, List<TimeZoneInfo> childList)
        {
            this.BaseUTCOffset = BaseUTCOffset;
            this.childList = childList;
            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
