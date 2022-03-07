using LeaveApp.Core.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace LeaveApp.Web.Models
{
    public class FileUploadModel
    {
        public HttpPostedFileBase files { get; set; }
        public EmployeeDetailsViewModel EmployeeDetails { get; set; }
        
    }
}