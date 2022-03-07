using System;

namespace LeaveApp.Core.ViewModel
{
    public class EmployeeListModal
    {
        public int EmployeeId { get; set; }
        public string ProfileImage { get; set; }
        public string EmployeeName { get; set; }
        public string EnrollmentId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string UserLeaves { get; set; }
        public DateTime DateOfJoining { get; set; }
    }
}
