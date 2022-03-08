using LeaveApp.Core.Enums;
using LeaveApp.Core.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveApp.Service.Leave
{
    public interface ILeaveService
    {
        Task<int> ApplyLeaveAsync(ApplyLeaveViewModel applyLeaveViewModel);
        Task<int> EditLeaveAsync(ApplyLeaveViewModel editLeaveViewModel);
        Task<IList<LeaveListViewModel>> GetApplyedLeaveListAsync(string EmployeeId);
        Task<IList<LeaveListViewModel>> GetApplyedLeaveListAsync();
        Task<ApplyLeaveViewModel> GetApplyedLeaveFormDataAsync(int EmployeeId);
        Task<bool> DeleteLeaveAsync(int id);
        Task<bool> ResetLeaveStatus(int Id, LeaveStatus Option);
        Task<LeaveListViewModel> GetApplyedLeaveAsync(int LeaveId);
        Task<LeaveRulesViewModel> GetLeaveRulesAsync(string EmployeeId);
        Task<bool> AddBonusLeavesAsync(string UserId, int EmployeeId, int Leaves, LeaveType LeaveType);
    }
}
