using System;
using System.ComponentModel.DataAnnotations;

namespace LeaveApp.Data.DataModel
{
    public class EmployeeDocument
    {
        [Key]
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
}
