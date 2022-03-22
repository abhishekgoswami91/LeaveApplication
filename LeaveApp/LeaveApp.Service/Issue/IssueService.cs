using LeaveApp.Core.Methords;
using LeaveApp.Core.ViewModel;
using LeaveApp.Data.DataModel;
using LeaveApp.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApp.Service.Issue
{
    public class IssueService : IIssueService
    {
        private readonly ApplicationDbContext _db;
        private readonly DataHelper _dataHelper;
        public IssueService(ApplicationDbContext applicationDbContext, DataHelper dataHelper)
        {
            _db = applicationDbContext;
            _dataHelper = dataHelper;
        }
        public async Task<int> AddIssueAsync(IssueViewModel issueDetail)
        {
            if (issueDetail == null)
                return 0;
            issueDetail.ModifiedBy = issueDetail.CreatedBy;
            issueDetail.CreatedDate = DateTime.Now;
            issueDetail.IsDeleted = false;
            issueDetail.ModifiedDate = DateTime.Now;
            IssueDetail entity =  _dataHelper.AutoMap<IssueViewModel, IssueDetail>(issueDetail);
            entity.IssueStatus = Core.Enums.IssueStatus.Created;
            _db.IssueDetails.Add(entity);
            try
            {
               return await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Task<List<IssueViewModel>> GetIssueListAsync()
        {
            throw new NotImplementedException();
        }
    }
}
