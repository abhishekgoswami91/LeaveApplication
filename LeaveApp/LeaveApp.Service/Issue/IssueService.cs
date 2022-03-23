using LeaveApp.Core.Enums;
using LeaveApp.Core.Methords;
using LeaveApp.Core.ViewModel;
using LeaveApp.Data.DataModel;
using LeaveApp.Data.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public Task<IssueViewModel> GetIssueByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public List<IssueViewModel> GetIssueList()
        {
            List<IssueDetail> issueDetail = new List<IssueDetail>();
            List<IssueViewModel> issueList = new List<IssueViewModel>();
            issueDetail =  _db.IssueDetails.ToList();
            foreach (var item in issueDetail)
            {
                var issue = _dataHelper.AutoMap<IssueDetail, IssueViewModel>(item);
                issueList.Add(issue);
            }
            return issueList;
        }

        public async Task<List<IssueViewModel>> GetIssueListAsync()
        {
            List<IssueDetail> issueDetail = new List<IssueDetail>();
            List<IssueViewModel> issueList = new List<IssueViewModel>();
            issueDetail = await _db.IssueDetails.Where(list => list.IssueStatus != IssueStatus.Resolved && !list.IsDeleted).ToListAsync();
            foreach (var item in issueDetail)
            {
                var issue = _dataHelper.AutoMap<IssueDetail, IssueViewModel>(item);
                issueList.Add(issue);
            }
            return issueList;
        }

        public async Task<bool> SetIssueStatusAsync(int IssueId, IssueStatus IssueStatus)
        {
            var entity = await _db.IssueDetails.FindAsync(IssueId);
            if (entity == null)
                return false;
            entity.IssueStatus = IssueStatus;
            _db.IssueDetails.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
            return true;
        }
    }
}
