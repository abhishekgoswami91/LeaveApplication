using System;
using System.Collections.Generic;
using System.Text;
using LeaveApp.Core.Enums;

namespace LeaveApp.Core.ViewModel
{
    public class CalendarViewModel
    {
        public CalendarViewModel()
        {
            LeaveCategorys = new List<LeaveCategorys>();
        }
        public int id { get; set; }
        public LeaveType leaveType { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool allDay { get; set; }
        public bool free { get; set; }
        public string color { get; set; }
        public LeaveStatus LeaveStatus {
            set {
                switch (value)
                {
                    case LeaveStatus.Submitted:
                        this.color = "#ffff00";
                        break;
                    case LeaveStatus.Approved:
                        this.color = "#00ff00";
                        break;
                    case LeaveStatus.Rejected:
                        this.color = "#ff0000";
                        break;
                    case LeaveStatus.NeedProperReason:
                        this.color = "#808080";
                        break;
                    default:
                        this.color = "#808080";
                        break;
                }
            }
        }
        public List<LeaveCategorys> LeaveCategorys { get; set; }
    }
}
