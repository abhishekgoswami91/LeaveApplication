using LeaveApp.Core.Enums;
using LeaveApp.Core.Methords;
using LeaveApp.Core.ViewModel;
using LeaveApp.Data.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.Service.Leave
{
    public class LeaveService : ILeaveService
    {
        private readonly ApplicationDbContext _db;
        private readonly DataHelper _dataHelper;
        public LeaveService(ApplicationDbContext applicationDbContext, DataHelper dataHelper)
        {
            _db = applicationDbContext;
            _dataHelper = dataHelper;
        }
        public async Task<int> ApplyLeaveAsync(ApplyLeaveViewModel applyLeaveViewModel)
        {
            if (applyLeaveViewModel == null || applyLeaveViewModel.EmployeeLeave == null || applyLeaveViewModel.EmployeeLeaveDetails.Count < 1)
            {
                return 0;
            }

            var EmployeeLeaves = _dataHelper.AutoMap<EmployeeLeave, Data.DataModel.EmployeeLeave>(applyLeaveViewModel.EmployeeLeave);
            _db.EmployeeLeaves.Add(EmployeeLeaves);
            foreach (var item in applyLeaveViewModel.EmployeeLeaveDetails)
            {
                item.DeletedDate = DateTime.Now;
                var EmployeeLeaveDetails = _dataHelper.AutoMap<EmployeeLeaveDetail, Data.DataModel.EmployeeLeaveDetail>(item);
                EmployeeLeaveDetails.CreatedBy = applyLeaveViewModel.EmployeeLeave.CreatedBy;
                EmployeeLeaveDetails.ModifiedBy = applyLeaveViewModel.EmployeeLeave.ModifiedBy;
                EmployeeLeaveDetails.CreatedDate = EmployeeLeaves.CreatedDate;
                EmployeeLeaveDetails.ModifiedDate = EmployeeLeaves.ModifiedDate;
                EmployeeLeaveDetails.DeletedBy = "";
                EmployeeLeaveDetails.DeletedDate = null;
                _db.EmployeeLeaveDetails.Add(EmployeeLeaveDetails);
            }
            await _db.SaveChangesAsync();
            return EmployeeLeaves.EmployeeLeaveId;
        }

        public async Task<bool> DeleteLeaveAsync(int id)
        {
            if (id < 1)
            {
                return false;
            }
            var entity = await _db.EmployeeLeaves.FindAsync(id);
            _db.EmployeeLeaves.Remove(entity);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<int> EditLeaveAsync(ApplyLeaveViewModel editLeaveViewModel)
        {
            if (editLeaveViewModel.EmployeeLeave != null)
            {
                var entity = await _db.EmployeeLeaves.FindAsync(editLeaveViewModel.EmployeeLeave.EmployeeLeaveId);
                entity.LeaveType = editLeaveViewModel.EmployeeLeave.LeaveType;
                entity.LeaveStartDate = editLeaveViewModel.EmployeeLeave.LeaveStartDate;
                entity.LeaveEndDate = editLeaveViewModel.EmployeeLeave.LeaveEndDate;
                entity.ModifiedBy = editLeaveViewModel.EmployeeLeave.ModifiedBy;
                entity.ModifiedDate = DateTime.Now;
                entity.LeaveStatus = LeaveStatus.Submitted;
                entity.LeaveReason = editLeaveViewModel.EmployeeLeave.LeaveReason;
                entity.NumberOfDaysLeave = editLeaveViewModel.EmployeeLeave.NumberOfDaysLeave;

                _db.EmployeeLeaveDetails.RemoveRange(entity.EmployeeLeaveDetails);
                //await _db.SaveChangesAsync();

                foreach (var item in editLeaveViewModel.EmployeeLeaveDetails)
                {
                    Data.DataModel.EmployeeLeaveDetail employeeLeaveDetail = new Data.DataModel.EmployeeLeaveDetail();
                    employeeLeaveDetail.LeaveCategory = item.LeaveCategory;
                    employeeLeaveDetail.LeaveDate = item.LeaveDate;
                    employeeLeaveDetail.CreatedBy = entity.CreatedBy;
                    employeeLeaveDetail.CreatedDate = entity.CreatedDate;
                    employeeLeaveDetail.ModifiedBy = entity.ModifiedBy;
                    employeeLeaveDetail.ModifiedDate = DateTime.Now;
                    employeeLeaveDetail.DeletedBy = "";
                    employeeLeaveDetail.EmployeeLeaveId = entity.EmployeeLeaveId;
                    _db.EmployeeLeaveDetails.Add(employeeLeaveDetail);

                }

                _db.EmployeeLeaves.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return entity.EmployeeLeaveId;
            }

            return 0;
        }

        public async Task<ApplyLeaveViewModel> GetApplyedLeaveFormDataAsync(int LeaveId)
        {
            ApplyLeaveViewModel applyLeaveViewModel = new ApplyLeaveViewModel();
            if (LeaveId < 1)
            {
                return applyLeaveViewModel;
            }
            var EmployeeLeave = await _db.EmployeeLeaves.Where(x => x.EmployeeLeaveId.Equals(LeaveId)).FirstOrDefaultAsync();
            applyLeaveViewModel.EmployeeLeave = _dataHelper.AutoMap<Data.DataModel.EmployeeLeave, EmployeeLeave>(EmployeeLeave);
            var leaveList = await _db.EmployeeLeaveDetails.Where(x => x.EmployeeLeaveId.Equals(applyLeaveViewModel.EmployeeLeave.EmployeeLeaveId)).ToListAsync();
            foreach (var subItem in leaveList)
            {
                EmployeeLeaveDetail employeeLeaveDetail = new EmployeeLeaveDetail();
                employeeLeaveDetail.LeaveCategory = subItem.LeaveCategory;
                employeeLeaveDetail.LeaveDate = subItem.LeaveDate;
                applyLeaveViewModel.EmployeeLeaveDetails.Add(employeeLeaveDetail);
            }

            return applyLeaveViewModel;
        }

        public async Task<IList<LeaveListViewModel>> GetApplyedLeaveListAsync(string EmployeeId)
        {
            IList<LeaveListViewModel> leaveListViewModel = new List<LeaveListViewModel>();
            if (string.IsNullOrEmpty(EmployeeId))
            {
                return leaveListViewModel;
            }
            var EmployeeLeaves = await _db.EmployeeLeaves.Where(x => x.EmployeeId.Equals(EmployeeId)).ToListAsync();
            var user = await _db.Users.Where(x => x.Id.Equals(EmployeeId)).Select(y => y.UserName).FirstOrDefaultAsync();
            foreach (var item in EmployeeLeaves)
            {
                LeaveListViewModel leave = new LeaveListViewModel();
                leave.Leaves.LeaveId = item.EmployeeLeaveId;
                leave.Leaves.Employee = user;
                leave.Leaves.LeaveEndDate = item.LeaveEndDate;
                leave.Leaves.LeaveReason = item.LeaveReason;
                leave.Leaves.LeaveStartDate = item.LeaveStartDate;
                leave.Leaves.LeaveStatus = item.LeaveStatus.ToString();
                leave.Leaves.LeaveType = item.LeaveType.ToString();
                leave.Leaves.NumberOfDaysLeave = item.NumberOfDaysLeave;

                var leaveList = await _db.EmployeeLeaveDetails.Where(x => x.EmployeeLeaveId.Equals(item.EmployeeLeaveId)).ToListAsync();
                foreach (var subItem in leaveList)
                {
                    LeaveDetail leaveDetail = new LeaveDetail();
                    leaveDetail.LeaveCategory = subItem.LeaveCategory.ToString();
                    leaveDetail.LeaveDate = subItem.LeaveDate;
                    leave.LeaveDetails.Add(leaveDetail);
                }
                leaveListViewModel.Add(leave);
            }

            return leaveListViewModel;
        }
        public async Task<LeaveListViewModel> GetApplyedLeaveAsync(int LeaveId)
        {
            LeaveListViewModel leaveListViewModel = new LeaveListViewModel();
            if (LeaveId < 1)
            {
                return leaveListViewModel;
            }
            var employeeLeaves = await _db.EmployeeLeaves.Where(x => x.EmployeeLeaveId.Equals(LeaveId)).FirstOrDefaultAsync();
            var user = _db.Users.Where(x => x.Id.Equals(employeeLeaves.EmployeeId)).Select(y => y.UserName).FirstOrDefault();

            leaveListViewModel.Leaves.LeaveId = employeeLeaves.EmployeeLeaveId;
            leaveListViewModel.Leaves.Employee = user;
            leaveListViewModel.Leaves.LeaveEndDate = employeeLeaves.LeaveEndDate;
            leaveListViewModel.Leaves.LeaveReason = employeeLeaves.LeaveReason;
            leaveListViewModel.Leaves.LeaveStartDate = employeeLeaves.LeaveStartDate;
            leaveListViewModel.Leaves.LeaveStatus = employeeLeaves.LeaveStatus.ToString();
            leaveListViewModel.Leaves.LeaveType = employeeLeaves.LeaveType.ToString();
            leaveListViewModel.Leaves.NumberOfDaysLeave = employeeLeaves.NumberOfDaysLeave;

            foreach (var subItem in employeeLeaves.EmployeeLeaveDetails)
            {
                LeaveDetail leaveDetail = new LeaveDetail();
                leaveDetail.LeaveCategory = subItem.LeaveCategory.ToString();
                leaveDetail.LeaveDate = subItem.LeaveDate;
                leaveListViewModel.LeaveDetails.Add(leaveDetail);
            }

            return leaveListViewModel;
        }

        public async Task<IList<LeaveListViewModel>> GetApplyedLeaveListAsync()
        {
            IList<LeaveListViewModel> leaveListViewModel = new List<LeaveListViewModel>();
            var users = await _db.Users.ToListAsync();

            foreach (var user in users)
            {
                var EmployeeLeaves = await _db.EmployeeLeaves.Where(x => x.EmployeeId.Equals(user.Id)).ToListAsync();

                foreach (var item in EmployeeLeaves)
                {
                    LeaveListViewModel leave = new LeaveListViewModel();
                    leave.Leaves.LeaveId = item.EmployeeLeaveId;
                    leave.Leaves.Employee = user.UserName;
                    leave.Leaves.EmployeeId = user.Id;
                    leave.Leaves.LeaveEndDate = item.LeaveEndDate;
                    leave.Leaves.LeaveReason = item.LeaveReason;
                    leave.Leaves.LeaveStartDate = item.LeaveStartDate;
                    leave.Leaves.LeaveStatus = item.LeaveStatus.ToString();
                    leave.Leaves.LeaveType = item.LeaveType.ToString();
                    leave.Leaves.NumberOfDaysLeave = item.NumberOfDaysLeave;

                    var leaveList = await _db.EmployeeLeaveDetails.Where(x => x.EmployeeLeaveId.Equals(item.EmployeeLeaveId)).ToListAsync();
                    foreach (var subItem in leaveList)
                    {
                        LeaveDetail leaveDetail = new LeaveDetail();
                        leaveDetail.LeaveCategory = subItem.LeaveCategory.ToString();
                        leaveDetail.LeaveDate = subItem.LeaveDate;
                        leave.LeaveDetails.Add(leaveDetail);
                    }
                    leaveListViewModel.Add(leave);
                }

            }

            return leaveListViewModel;
        }

        public async Task<bool> ResetLeaveStatus(int Id, LeaveStatus Option)
        {
            if (Id < 1)
            {
                return false;
            }

            var entity = await _db.EmployeeLeaves.FindAsync(Id);
            entity.LeaveStatus = Option;
            _db.EmployeeLeaves.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();

            if (Option == LeaveStatus.Approved)
            {
                double Sick = 0;
                double Paid = 0;
                var data = await _db.EmployeeLeaves.Where(x => x.EmployeeId.Equals(entity.EmployeeId) && x.LeaveStatus == LeaveStatus.Approved && !x.IsDeleted).ToListAsync();
                (Sick, Paid) = await calculateLeaves(entity.EmployeeId, LeaveStatus.Approved);
                var empDetail = await _db.EmployeeDetails.Where(y => y.EmployeeId.Equals(entity.EmployeeId) && !y.IsDeleted).FirstOrDefaultAsync();
                empDetail.SickApplyed = Sick;
                empDetail.PaidApplyed = Paid;
                _db.EmployeeDetails.Attach(empDetail);
                _db.Entry(empDetail).State = EntityState.Modified;
                _db.SaveChanges();
            }

            return true;
        }

        public async Task<LeaveRulesViewModel> GetLeaveRulesAsync(string EmployeeId)
        {
            double Sick = 0;
            double Paid = 0;
            var empDetail = await _db.EmployeeDetails.Where(y => y.EmployeeId.Equals(EmployeeId) && !y.IsDeleted).FirstOrDefaultAsync();
            if (empDetail == null)
            {
                return new LeaveRulesViewModel();
            }
            (Sick, Paid) = await calculateLeaves(empDetail.EmployeeId, LeaveStatus.Submitted);
            return new LeaveRulesViewModel(empDetail.DateOfJoining, empDetail.SickApplyed, empDetail.PaidApplyed, Sick + Paid, 0);
        }

        private async Task<(double Sick, double Paid)> calculateLeaves(string EmployeeId, LeaveStatus LeaveStatus)
        {
            double Sick = 0;
            double Paid = 0;
            var data = await _db.EmployeeLeaves.Where(x => x.EmployeeId.Equals(EmployeeId) && x.LeaveStatus == LeaveStatus && !x.IsDeleted).ToListAsync();
            foreach (var item in data)
            {
                switch (item.LeaveType)
                {
                    case LeaveType.Sick:
                        foreach (var subItem in item.EmployeeLeaveDetails)
                        {
                            switch (subItem.LeaveCategory)
                            {
                                case LeaveCategorys.FullDay:
                                    Sick += 1;
                                    break;
                                case LeaveCategorys.FirstHalfDay:
                                    Sick += .5;
                                    break;
                                case LeaveCategorys.SecondHalfDay:
                                    Sick += .5;
                                    break;
                            }
                        }
                        break;
                    case LeaveType.Paid:
                        foreach (var subItem in item.EmployeeLeaveDetails)
                        {
                            switch (subItem.LeaveCategory)
                            {
                                case LeaveCategorys.FullDay:
                                    Paid += 1;
                                    break;
                                case LeaveCategorys.FirstHalfDay:
                                    Paid += .5;
                                    break;
                                case LeaveCategorys.SecondHalfDay:
                                    Paid += .5;
                                    break;
                            }
                        }
                        break;
                }
            }

            return (Sick, Paid);
        }

        public async Task<bool> AddBonusLeavesAsync(string UserId, int EmployeeId, int Leaves)
        {
            try
            {
                Data.DataModel.EmployeeBonusLeave employeeBonusLeave = new Data.DataModel.EmployeeBonusLeave();
                employeeBonusLeave.BonusLeave = Leaves;
                employeeBonusLeave.CreatedBy = UserId;
                employeeBonusLeave.CreatedDate = DateTime.Now;
                employeeBonusLeave.DeletedBy = null;
                employeeBonusLeave.DeletedDate = null;
                employeeBonusLeave.IsDeleted = false;
                employeeBonusLeave.ModifiedBy = UserId;
                employeeBonusLeave.ModifiedDate = DateTime.Now;
                employeeBonusLeave.EmployeeDetailId = EmployeeId;
                _db.EmployeeBonusLeaves.Add(employeeBonusLeave);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
