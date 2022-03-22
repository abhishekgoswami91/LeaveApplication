using LeaveApp.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApp.Core.ViewModel
{
    public class IssueViewModel
    {
        public int IssueId { get; set; }
        public IssueStatus IssueStatus { get; set; }
        public string Description { get; set; }
        public string IssueImage { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> DeletedDate { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
