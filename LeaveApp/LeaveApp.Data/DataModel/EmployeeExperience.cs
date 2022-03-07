using System;
using System.ComponentModel.DataAnnotations;

namespace LeaveApp.Data.DataModel
{
    public class EmployeeExperience
    {
        [Key]
        public int ExperienceId { get; set; }
        public int EmployeeDetailId { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double StartSalary { get; set; }
        public double EndSalary { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; } 
        public string CreatedBy { get; set; } 
        public DateTime ModifiedDate { get; set; } 
        public string ModifiedBy { get; set; } 
        public Nullable<DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; } 
        public bool IsDeleted { get; set; } = false;
    }
}
