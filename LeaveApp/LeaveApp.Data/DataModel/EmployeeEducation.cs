using LeaveApp.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace LeaveApp.Data.DataModel
{
    public class EmployeeEducation
    {
        [Key]
        public int EducationId { get; set; }
        public int EmployeeDetailId { get; set; }
        public Degrees Degree { get; set; }
        public Specializations Specialization { get; set; }
        public string Institute { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Score { get; set; } // 0-10
        public DateTime CreatedDate { get; set; } 
        public string CreatedBy { get; set; } 
        public DateTime ModifiedDate { get; set; } 
        public string ModifiedBy { get; set; } 
        public Nullable<DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; } 
        public bool IsDeleted { get; set; } = false;
    }
}
