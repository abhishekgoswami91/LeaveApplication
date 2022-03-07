using LeaveApp.Core.Enums;
using System;
using System.Collections.Generic;

namespace LeaveApp.Data.DataModel
{
    public class EmployeeDetail
    {
        public EmployeeDetail()
        {
            EmployeeEducations = new HashSet<EmployeeEducation>();
            EmployeeExperiences = new HashSet<EmployeeExperience>();
            EmployeeDocuments = new HashSet<EmployeeDocument>();
            EmployeeBonusLeaves = new HashSet<EmployeeBonusLeave>();
        }
        public int EmployeeDetailId { get; set; }
        public string EmployeeId { get; set; }
        //Basic Information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public double AnnualSalary { get; set; }
        //public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string PersonalEmail { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; } //
        public string MaritalStatus { get; set; } //
        public string SocialMediaLinks { get; set; } // add list of social media links with ",".
        public string Description { get; set; }
        //Employee Information
        public string EnrollmentId { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string CompanyEmail { get; set; }
        public Departments Department { get; set; }
        public Designations Designation { get; set; }
        public string ReportingEmployee { get; set; }
        public EmployeeTypes EmployeeType { get; set; }
        //Contact Information
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string City { get; set; }
        public States State { get; set; }
        public int PinCode { get; set; } // 6 digit.
        public Country Country { get; set; }
        //leave calculations.
        public double SickApplyed { get; set; } = 0;
        public double PaidApplyed { get; set; } = 0;
        //
        public DateTime CreatedDate { get; set; } 
        public string CreatedBy { get; set; } 
        public DateTime ModifiedDate { get; set; } 
        public string ModifiedBy { get; set; } 
        public Nullable<DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; } 
        public bool IsDeleted { get; set; } = false;
        //Education
        public virtual ICollection<EmployeeEducation> EmployeeEducations { get; set; }
        //Experience
        public virtual ICollection<EmployeeExperience> EmployeeExperiences { get; set; }
        public virtual ICollection<EmployeeDocument> EmployeeDocuments { get; set; }
        public virtual ICollection<EmployeeBonusLeave> EmployeeBonusLeaves { get; set; }

    }
}
