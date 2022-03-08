using LeaveApp.Core.Enums;
using System;
using System.Collections.Generic;

namespace LeaveApp.Core.ViewModel
{
    public class EmployeeDetailsViewModel
    {
        public EmployeeDetailsViewModel()
        {
            EmployeeEducations = new List<EmployeeEducation>();
            EmployeeExperiences = new List<EmployeeExperience>();
            EmployeeDocuments = new List<EmployeeDocument>();
            EmployeeBonusLeaves = new List<EmployeeBonusLeave>();
        }
        public EmployeeDetail EmployeeDetail { get; set; }
        //Education
        public IList<EmployeeEducation> EmployeeEducations { get; set; }
        //Experience
        public IList<EmployeeExperience> EmployeeExperiences { get; set; }
        public IList<EmployeeDocument> EmployeeDocuments { get; set; }
        public List<EmployeeBonusLeave> EmployeeBonusLeaves { get; set; }
    }

    public class EmployeeDetail
    {
        public int EmployeeDetailId { get; set; }
        public string EmployeeId { get; set; }
        //Basic Information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ProfileImage { get; set; }
        public double AnnualSalary { get; set; }
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
        public string ReportingEmployeeName { get; set; }
        public EmployeeTypes EmployeeType { get; set; }
        //Contact Information
        public string CurrentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string City { get; set; }
        public States State { get; set; }
        public int PinCode { get; set; } // 6 digit.
        public Country Country { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "";
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "";
        public Nullable<DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; } = "";
        public bool IsDeleted { get; set; } = false;
    }

    public class EmployeeEducation
    {
        public int EducationId { get; set; }
        public Degrees Degree { get; set; }
        public Specializations Specialization { get; set; }
        public string Institute { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Score { get; set; } // 0-10
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "";
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "";
        public Nullable<DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; } = "";
        public bool IsDeleted { get; set; } = false;
    }
    public class EmployeeExperience
    {
        public int ExperienceId { get; set; }
        public string Organization { get; set; }
        public string Designation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double StartSalary { get; set; }
        public double EndSalary { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string CreatedBy { get; set; } = "";
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        public string ModifiedBy { get; set; } = "";
        public Nullable<DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; } = "";
        public bool IsDeleted { get; set; } = false;
    }
    public class EmployeeDocument
    {
        public int DocumentId { get; set; }
        public int EmployeeDetailId { get; set; }
        public String FileName { get; set; }
        public byte[] FileContent { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
    public class EmployeeBonusLeave
    {
        public int BonusLeaveId { get; set; }
        public int EmployeeDetailId { get; set; }
        public int BonusLeave { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
