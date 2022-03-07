using System;
using LeaveApp.Core.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeaveApp.Data.DataModel
{
    public class EmployeeLeave
    {
        public EmployeeLeave()
        {
            EmployeeLeaveDetails = new HashSet<EmployeeLeaveDetail>();
        }
        [Key]
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
        public Nullable<DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; } 
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<EmployeeLeaveDetail> EmployeeLeaveDetails { get; set; }
    }
}
