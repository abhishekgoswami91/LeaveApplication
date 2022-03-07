using LeaveApp.Core.Enums;
using System;
using System.Collections.Generic;

namespace LeaveApp.Core.ViewModel
{
    public class ApplyLeaveViewModel
    {
        public ApplyLeaveViewModel()
        {
            EmployeeLeaveDetails = new List<EmployeeLeaveDetail>();
        }
    
        public EmployeeLeave EmployeeLeave { get; set; }
        public IList<EmployeeLeaveDetail> EmployeeLeaveDetails { get; set; }
    }

    public class EmployeeLeave
    {
        public int EmployeeLeaveId { get; set; }
        public string EmployeeId { get; set; }
        public LeaveType LeaveType { get; set; } // Sick, Paid
        public LeaveStatus LeaveStatus { get; set; }
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public double NumberOfDaysLeave { get; set; }
        public string LeaveReason { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public class EmployeeLeaveDetail
    {
        public int EmployeeLeaveDetailId { get; set; }
        public int EmployeeLeaveId { get; set; }
        public DateTime LeaveDate { get; set; }
        public LeaveCategorys LeaveCategory { get; set; } // FullDay/HalfDay/FirstHalf/SecondHalf
        public DateTime DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
