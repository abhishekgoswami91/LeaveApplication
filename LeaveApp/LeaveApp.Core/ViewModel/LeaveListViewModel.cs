using System;
using System.Collections.Generic;

namespace LeaveApp.Core.ViewModel
{
    public class LeaveListViewModel
    {
        public LeaveListViewModel()
        {
            LeaveDetails = new List<LeaveDetail>();
            Leaves = new Leave();
        }
        public Leave Leaves { get; set; }
        public IList<LeaveDetail> LeaveDetails { get; set; }
    }

    public class Leave
    {
        public int LeaveId { get; set; }
        public string Employee { get; set; }
        public string EmployeeId { get; set; }
        public string LeaveType { get; set; } //Sick = 1, Paid.
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public string LeaveReason { get; set; }
        public double NumberOfDaysLeave { get; set; }
        public string LeaveStatus { get; set; }
    }
    public class LeaveDetail
    {
        public string LeaveCategory { get; set; } //FullDay = 1,FirstHalfDay,SecondHalfDay.
        public DateTime LeaveDate { get; set; }
    }
}
