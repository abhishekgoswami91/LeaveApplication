using LeaveApp.Core.Enums;
using System;

namespace LeaveApp.Data.DataModel
{
    public class EmployeeLeaveDetail
    {
        public int EmployeeLeaveDetailId { get; set; }
        public int EmployeeLeaveId { get; set; }
        public DateTime LeaveDate { get; set; }
        public LeaveCategorys LeaveCategory { get; set; } // FullDay/FirstHalf/SecondHalf
        public DateTime CreatedDate { get; set; } 
        public string CreatedBy { get; set; } 
        public DateTime ModifiedDate { get; set; } 
        public string ModifiedBy { get; set; } 
        public Nullable<DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; } 
        public bool IsDeleted { get; set; } = false;
    }
}
