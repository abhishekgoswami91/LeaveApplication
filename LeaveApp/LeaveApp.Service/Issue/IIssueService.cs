using LeaveApp.Core.Enums;
using LeaveApp.Core.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveApp.Service.Issue
{
    public interface IIssueService
    {
        Task<List<IssueViewModel>> GetIssueListAsync();
        List<IssueViewModel> GetIssueList();
        Task<IssueViewModel> GetIssueByIdAsync(int Id);
        Task<int> AddIssueAsync(IssueViewModel issueViewModel);
        Task<bool> SetIssueStatusAsync(int IssueId, IssueStatus IssueStatus);

    }
}
