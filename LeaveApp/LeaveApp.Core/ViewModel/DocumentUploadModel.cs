using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeaveApp.Core.ViewModel
{
    public class DocumentUploadModel
    {
        public DocumentUploadModel()
        {
            EmployeeDocuments = new List<EmployeeDocument>();
        }
        public int EmployeeDetailId { get; set; }
        public IList<EmployeeDocument> EmployeeDocuments { get; set; }
    }
    //public class EmployeeDocument
    //{
    //    public int DocumentId { get; set; }
    //    public int EmployeeDetailId { get; set; }
    //    [Required]
    //    public String FileName { get; set; }
    //    [Required]
    //    public byte[] FileContent { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public string CreatedBy { get; set; }
    //    public DateTime ModifiedDate { get; set; }
    //    public string ModifiedBy { get; set; }
    //    public Nullable<DateTime> DeletedDate { get; set; }
    //    public string DeletedBy { get; set; }
    //    public bool IsDeleted { get; set; } = false;
    //}
}
