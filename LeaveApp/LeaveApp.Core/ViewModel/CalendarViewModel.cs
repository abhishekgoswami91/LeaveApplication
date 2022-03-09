using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApp.Core.ViewModel
{
    public class CalendarViewModel
    {
        public int id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool allDay { get; set; }
        public bool free { get; set; }
        public string color { get; set; }
    }
}
