using LeaveApp.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApp.Service.Issue
{
    public interface IIssueService
    {
        Task<List<IssueViewModel>> GetIssueListAsync();
        Task<int> AddIssueAsync(IssueViewModel issueViewModel);


    }
}
