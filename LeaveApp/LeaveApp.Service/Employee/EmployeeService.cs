using LeaveApp.Core.Enums;
using LeaveApp.Core.Methords;
using LeaveApp.Core.ViewModel;
using LeaveApp.Data.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveApp.Service.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _db;
        private readonly DataHelper _dataHelper;
        public EmployeeService(ApplicationDbContext applicationDbContext, DataHelper dataHelper)
        {
            _db = applicationDbContext;
            _dataHelper = dataHelper;
        }
        public async Task<int> AddEmployeeDetailsAsync(EmployeeDetailsViewModel employeeDetailsViewModel)
        {
            if (employeeDetailsViewModel == null || employeeDetailsViewModel.EmployeeDetail == null || employeeDetailsViewModel.EmployeeEducations.Count < 1 || employeeDetailsViewModel.EmployeeExperiences.Count < 1)
            {
                return 0;
            }
            var EmployeeDetails = _dataHelper.AutoMap<EmployeeDetail, Data.DataModel.EmployeeDetail>(employeeDetailsViewModel.EmployeeDetail);
            _db.EmployeeDetails.Add(EmployeeDetails);

            foreach (var item in employeeDetailsViewModel.EmployeeEducations)
            {
                item.CreatedBy = EmployeeDetails.CreatedBy;
                item.CreatedDate = DateTime.Now;
                item.DeletedBy = "";
                item.IsDeleted = false;
                item.ModifiedBy = EmployeeDetails.ModifiedBy;
                item.ModifiedDate = DateTime.Now;
                
                var EmployeeEducations = _dataHelper.AutoMap<EmployeeEducation, Data.DataModel.EmployeeEducation>(item);

                _db.EmployeeEducations.Add(EmployeeEducations);
            }
            foreach (var item in employeeDetailsViewModel.EmployeeExperiences)
            {
                item.CreatedBy = EmployeeDetails.CreatedBy;
                item.CreatedDate = DateTime.Now;
                item.DeletedBy = "";
                item.IsDeleted = false;
                item.ModifiedBy = EmployeeDetails.ModifiedBy;
                item.ModifiedDate = DateTime.Now;

                var EmployeeExperiences = _dataHelper.AutoMap<EmployeeExperience, Data.DataModel.EmployeeExperience>(item);
                _db.EmployeeExperiences.Add(EmployeeExperiences);
            }
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            

            return EmployeeDetails.EmployeeDetailId;
        }

        public async Task<bool> DeleteEmployeeDetailsAsync(int EmployeeDetailId, string UserId, bool DoRemove = false)
        {
            if (EmployeeDetailId < 1)
            {
                return false;
            }
            var employeeDetail = await _db.EmployeeDetails.FindAsync(EmployeeDetailId);
            if (DoRemove)
            {
                _db.EmployeeDetails.Remove(employeeDetail);
                await _db.SaveChangesAsync();
                return true;
            }
            else
            {
                employeeDetail.IsDeleted = true;
                employeeDetail.DeletedDate = DateTime.Now;
                employeeDetail.DeletedBy = UserId;
                _db.EmployeeDetails.Attach(employeeDetail);
                _db.Entry(employeeDetail).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<EmployeeDetailsViewModel> GetEmployeeDetailsByIdAsync(int EmployeeDetailId)
        {
            EmployeeDetailsViewModel employeeDetailsViewModel = new EmployeeDetailsViewModel();
            if (EmployeeDetailId < 1)
            {
                return employeeDetailsViewModel;
            }
            var EmployeeDetails = await _db.EmployeeDetails.FindAsync(EmployeeDetailId);
            employeeDetailsViewModel.EmployeeDetail = _dataHelper.AutoMap<Data.DataModel.EmployeeDetail, EmployeeDetail>(EmployeeDetails);
            employeeDetailsViewModel.EmployeeDetail.ProfileImage = EmployeeDetails.ProfileImage;
            var user = _db.Users.Find(EmployeeDetails.ReportingEmployee);
            employeeDetailsViewModel.EmployeeDetail.ReportingEmployeeName = user.UserName;
            foreach (var item in EmployeeDetails.EmployeeEducations)
            {
                var employeeEducation  = _dataHelper.AutoMap<Data.DataModel.EmployeeEducation, EmployeeEducation>(item);
                employeeDetailsViewModel.EmployeeEducations.Add(employeeEducation);
            }
            foreach (var item in EmployeeDetails.EmployeeExperiences)
            {
                var employeeExperience = _dataHelper.AutoMap<Data.DataModel.EmployeeExperience, EmployeeExperience>(item);
                employeeDetailsViewModel.EmployeeExperiences.Add(employeeExperience);
            }
            foreach (var item in EmployeeDetails.EmployeeDocuments)
            {
                var employeeDocument = _dataHelper.AutoMap<Data.DataModel.EmployeeDocument, EmployeeDocument>(item);
                employeeDocument.FileContent = null;
                if (!item.IsDeleted)
                employeeDetailsViewModel.EmployeeDocuments.Add(employeeDocument);
            }
            foreach (var item in EmployeeDetails.EmployeeBonusLeaves)
            {
                var employeeBonusLeaves = _dataHelper.AutoMap<Data.DataModel.EmployeeBonusLeave, EmployeeBonusLeave>(item);
                employeeDetailsViewModel.EmployeeBonusLeaves.Add(employeeBonusLeaves);
            }
            return employeeDetailsViewModel;
        }
        public async Task<IList<EmployeeListModal>> GetAllEmployeeDetailsAsync()
        {
            IList<EmployeeListModal> employeeList = new List<EmployeeListModal>();

            var employeeDetails = await _db.EmployeeDetails.Where(x => !x.IsDeleted).ToListAsync();

            foreach (var item in employeeDetails)
            {
                var employeeData = new EmployeeListModal();
                employeeData.DateOfJoining = item.DateOfJoining;
                employeeData.Department = item.Department.ToString();
                employeeData.Designation = item.Designation.ToString();
                employeeData.Email = item.CompanyEmail;
                employeeData.EmployeeId = item.EmployeeDetailId;
                employeeData.EmployeeName = item.FirstName + " " + item.LastName;
                employeeData.EnrollmentId = item.EnrollmentId;
                employeeData.Phone = item.Phone;
                employeeData.ProfileImage = item.ProfileImage;
                //var leaveRules = new LeaveRulesViewModel(item.DateOfJoining, item.SickApplyed, item.PaidApplyed,0,0);
                //employeeData.UserLeaves = leaveRules.Sick + "/" + leaveRules.Paid; // + " Last Year: " + leaveRules.SickApplyed + "/" + leaveRules.PaidApplyed;
                var LastYearLeaves = await _db.EmployeeLeaves.Where(x => x.EmployeeId.Equals(item.EmployeeId) && x.ModifiedDate.Year == DateTime.Now.Year - 1 && !x.IsDeleted && x.LeaveStatus == LeaveStatus.Approved).ToListAsync();
                var ThisYearLeaves = await _db.EmployeeLeaves.Where(x => x.EmployeeId.Equals(item.EmployeeId) && x.ModifiedDate.Year == DateTime.Now.Year && !x.IsDeleted && x.LeaveStatus == LeaveStatus.Approved).ToListAsync();
                double bonusSL = 0;
                double bonusPL = 0;
                foreach (var subItem in item.EmployeeBonusLeaves)
                {
                    if (!subItem.IsDeleted && subItem.CreatedDate.Year == DateTime.Now.Year)
                    {
                        switch (subItem.LeaveType)
                        {
                            case LeaveType.Sick:
                                bonusSL += subItem.BonusLeave;
                                break;
                            case LeaveType.Paid:
                                bonusPL += subItem.BonusLeave;
                                break;
                            case LeaveType.NonPaid:
                                break;
                        }
                    }
                }
                var leaveSpan = new LeaveRulesViewModel(item.DateOfJoining, LastYearLeaves, ThisYearLeaves, bonusSL, bonusPL);
                //var bonusLeaves = await _db.EmployeeBonusLeaves.Where(x => x.EmployeeDetailId.Equals(item.EmployeeId) && !x.IsDeleted).ToListAsync();
                employeeData.UserLeaves = leaveSpan.Sick + "/" + leaveSpan.Paid;
                
                if (bonusSL > 0 || bonusPL > 0)
                {
                    employeeData.UserLeaves += "  Bonus(SL/PL): " + bonusSL + "/" + bonusPL;
                }
                if (item.DateOfJoining.Year < DateTime.Now.Year)
                {
                    employeeData.UserLeaves += "  Last Year(SL/PL): " + leaveSpan.LastYearSick + "/" + leaveSpan.LastYearPaid;
                }
                employeeList.Add(employeeData);
            }

            return employeeList;
        }

        public async Task<bool> EditEmployeeDetailsAsync(EmployeeDetailsViewModel employeeDetailsViewModel)
        {
            if (employeeDetailsViewModel.EmployeeDetail == null || employeeDetailsViewModel.EmployeeEducations.Count < 1 || employeeDetailsViewModel.EmployeeExperiences.Count < 1)
            {
                return false;
            }
            var EmployeeDetails = await _db.EmployeeDetails.FindAsync(employeeDetailsViewModel.EmployeeDetail.EmployeeDetailId);
            EmployeeDetails.ProfileImage = employeeDetailsViewModel.EmployeeDetail.ProfileImage;
            EmployeeDetails.City = employeeDetailsViewModel.EmployeeDetail.City;
            EmployeeDetails.CompanyEmail = employeeDetailsViewModel.EmployeeDetail.CompanyEmail;
            //also changing in account.
            var account = _db.Users.Find(EmployeeDetails.EmployeeId);
            if (account.Email != EmployeeDetails.CompanyEmail)
            {
                account.Email = EmployeeDetails.CompanyEmail;
                _db.Users.Attach(account);
                _db.Entry(account).State = EntityState.Modified;
                await _db.SaveChangesAsync();

            }
            EmployeeDetails.Country = employeeDetailsViewModel.EmployeeDetail.Country;
            EmployeeDetails.CurrentAddress = employeeDetailsViewModel.EmployeeDetail.CurrentAddress;
            EmployeeDetails.DateOfJoining = employeeDetailsViewModel.EmployeeDetail.DateOfJoining;
            EmployeeDetails.Department = employeeDetailsViewModel.EmployeeDetail.Department;
            EmployeeDetails.Description = employeeDetailsViewModel.EmployeeDetail.Description;
            EmployeeDetails.Designation = employeeDetailsViewModel.EmployeeDetail.Designation;
            EmployeeDetails.DOB = employeeDetailsViewModel.EmployeeDetail.DOB;
            EmployeeDetails.EmployeeType = employeeDetailsViewModel.EmployeeDetail.EmployeeType;
            EmployeeDetails.EnrollmentId = employeeDetailsViewModel.EmployeeDetail.EnrollmentId;
            EmployeeDetails.AnnualSalary = employeeDetailsViewModel.EmployeeDetail.AnnualSalary;
            EmployeeDetails.FirstName = employeeDetailsViewModel.EmployeeDetail.FirstName;
            EmployeeDetails.Gender = employeeDetailsViewModel.EmployeeDetail.Gender;
            EmployeeDetails.LastName = employeeDetailsViewModel.EmployeeDetail.LastName;
            EmployeeDetails.MaritalStatus = employeeDetailsViewModel.EmployeeDetail.MaritalStatus;
            //EmployeeDetails.MiddleName = employeeDetailsViewModel.EmployeeDetail.MiddleName;
            EmployeeDetails.ModifiedBy = employeeDetailsViewModel.EmployeeDetail.ModifiedBy;
            EmployeeDetails.ModifiedDate = employeeDetailsViewModel.EmployeeDetail.ModifiedDate;
            EmployeeDetails.PermanentAddress = employeeDetailsViewModel.EmployeeDetail.PermanentAddress;
            EmployeeDetails.PersonalEmail = employeeDetailsViewModel.EmployeeDetail.PersonalEmail;
            EmployeeDetails.Phone = employeeDetailsViewModel.EmployeeDetail.Phone;
            EmployeeDetails.PinCode = employeeDetailsViewModel.EmployeeDetail.PinCode;
            if(employeeDetailsViewModel.EmployeeDetail.ReportingEmployee.Trim() != "")
            EmployeeDetails.ReportingEmployee = employeeDetailsViewModel.EmployeeDetail.ReportingEmployee;
            EmployeeDetails.SocialMediaLinks = employeeDetailsViewModel.EmployeeDetail.SocialMediaLinks;
            EmployeeDetails.State = employeeDetailsViewModel.EmployeeDetail.State;

            _db.EmployeeEducations.RemoveRange(EmployeeDetails.EmployeeEducations);
            _db.EmployeeExperiences.RemoveRange(EmployeeDetails.EmployeeExperiences);
            foreach (var item in employeeDetailsViewModel.EmployeeEducations)
            {
                Data.DataModel.EmployeeEducation employeeEducation = new Data.DataModel.EmployeeEducation();
                employeeEducation.ModifiedDate = DateTime.Now;
                employeeEducation.CreatedBy = EmployeeDetails.CreatedBy;
                employeeEducation.CreatedDate = EmployeeDetails.CreatedDate;
                employeeEducation.ModifiedBy = EmployeeDetails.ModifiedBy;
                employeeEducation.Degree = item.Degree;
                employeeEducation.EmployeeDetailId = EmployeeDetails.EmployeeDetailId;
                employeeEducation.EndDate = item.EndDate;
                employeeEducation.Institute = item.Institute;
                employeeEducation.Score = item.Score;
                employeeEducation.Specialization = item.Specialization;
                employeeEducation.StartDate = item.StartDate;
                _db.EmployeeEducations.Add(employeeEducation);
            }
            foreach (var item in employeeDetailsViewModel.EmployeeExperiences)
            {
                Data.DataModel.EmployeeExperience employeeExperience = new Data.DataModel.EmployeeExperience();
                employeeExperience.ModifiedDate = DateTime.Now;
                employeeExperience.CreatedBy = EmployeeDetails.CreatedBy;
                employeeExperience.CreatedDate = EmployeeDetails.CreatedDate;
                employeeExperience.ModifiedBy = EmployeeDetails.ModifiedBy;
                employeeExperience.Designation = item.Designation;
                employeeExperience.EmployeeDetailId = EmployeeDetails.EmployeeDetailId;
                employeeExperience.EndDate = item.EndDate;
                employeeExperience.EndSalary = item.EndSalary;
                employeeExperience.Organization = item.Organization;
                employeeExperience.Reason = item.Reason;
                employeeExperience.StartDate = item.StartDate;
                employeeExperience.StartSalary = item.StartSalary;
                _db.EmployeeExperiences.Add(employeeExperience);
            }

            _db.EmployeeDetails.Attach(EmployeeDetails);
            _db.Entry(EmployeeDetails).State = EntityState.Modified;
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }
            
            return true;
        }

        public IList<KeyValuePair<string, string>> GetEmployeeDDLAsync()
        {
           var users = _db.Users.ToList();
            IList<KeyValuePair<string, string>> ddlData = new List<KeyValuePair<string, string>>();
            foreach (var item in users)
            {
               var isUserDetail = _db.EmployeeDetails.Any(x => x.EmployeeId.Equals(item.Id) && !x.IsDeleted);
                KeyValuePair<string, string> temp = new KeyValuePair<string, string>(item.Id + ":" +isUserDetail.ToString(), item.UserName);
                ddlData.Add(temp);
            }
            return ddlData;
        }

        public async Task<bool> AddEmployeeDocumentAsync(DocumentUploadModel documentUploadModel)
        {
            if (documentUploadModel == null || documentUploadModel.EmployeeDocuments.Count < 1)
            {
                return false;
            }
            foreach (var item in documentUploadModel.EmployeeDocuments)
            {
               var entityData = _dataHelper.AutoMap<EmployeeDocument, Data.DataModel.EmployeeDocument>(item);
                entityData.CreatedDate = DateTime.Now;
                entityData.DeletedBy = "";
                entityData.IsDeleted = false;
                entityData.ModifiedDate = DateTime.Now;
                entityData.EmployeeDetailId = documentUploadModel.EmployeeDetailId;
                _db.EmployeeDocuments.Add(entityData);
            }
            
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw;
            }


            return true;
        }
        public async Task<bool> DeleteEmployeeDocumentAsync(int DocumentId, string UserId, bool DoRemove = false)
        {
            if (DocumentId < 1)
            {
                return false;
            }
            var employeeDocument = await _db.EmployeeDocuments.FindAsync(DocumentId);
            if (DoRemove)
            {
                _db.EmployeeDocuments.Remove(employeeDocument);
                await _db.SaveChangesAsync();
                return true;
            }
            else
            {
                employeeDocument.IsDeleted = true;
                employeeDocument.DeletedDate = DateTime.Now;
                employeeDocument.DeletedBy = UserId;
                _db.EmployeeDocuments.Attach(employeeDocument);
                _db.Entry(employeeDocument).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return true;
            }
        }
        public async Task<Data.DataModel.EmployeeDocument> GetEmployeeDocumentByIdAsync(int DocumentId)
        {
           return await _db.EmployeeDocuments.FindAsync(DocumentId);
        }

        public async Task<ProfileViewModel> GetProfileData(string UserId)
        {
            ProfileViewModel profileData = new ProfileViewModel();
            Data.DataModel.EmployeeDetail entity = await _db.EmployeeDetails.Where(x => x.EmployeeId.Equals(UserId)).FirstOrDefaultAsync();
            if (entity != null)
            {
               profileData.ProfileImage = entity.ProfileImage;
                profileData.ProfileId = entity.EmployeeDetailId;
               profileData.FirstName = entity.FirstName;
               profileData.LastName = entity.LastName;
            }
            return profileData;
        }

        public async Task<List<CalendarViewModel>> GetLeavesForCalendarDataAsync(string UserId, bool IsAdmin)
        {
            List<CalendarViewModel> calendarViewModels = new List<CalendarViewModel>();
            if (IsAdmin)
            {
                var allEmpDetails = await _db.EmployeeDetails.Where(list => !list.IsDeleted).ToListAsync();
                foreach (var item in allEmpDetails)
                {
                   
                    var allLeaves = await _db.EmployeeLeaves.Where(list => list.EmployeeId.Equals(item.EmployeeId) && !list.IsDeleted).ToListAsync();
                    foreach (var subItem in allLeaves)
                    {
                        CalendarViewModel calendarViewModel = new CalendarViewModel();
                        calendarViewModel.id = subItem.EmployeeLeaveId;
                        calendarViewModel.leaveType = subItem.LeaveType;
                        calendarViewModel.start = subItem.LeaveStartDate;
                        calendarViewModel.end = subItem.LeaveEndDate;
                        calendarViewModel.title = item.FirstName;
                        calendarViewModel.description = subItem.LeaveReason;
                        //calendarViewModel.color = "#ff4000";
                        calendarViewModel.LeaveStatus = subItem.LeaveStatus;
                        foreach (var obj in subItem.EmployeeLeaveDetails)
                        {
                            calendarViewModel.LeaveCategorys.Add(obj.LeaveCategory);
                        }
                        calendarViewModels.Add(calendarViewModel);
                    }
                }
            }
            else
            {
                var empDetails = await _db.EmployeeDetails.Where(list => list.EmployeeId.Equals(UserId) && !list.IsDeleted).FirstOrDefaultAsync();
                var allLeaves = await _db.EmployeeLeaves.Where(list => list.EmployeeId.Equals(UserId) && !list.IsDeleted).ToListAsync();
                foreach (var subItem in allLeaves)
                {
                    CalendarViewModel calendarViewModel = new CalendarViewModel();
                    calendarViewModel.id = subItem.EmployeeLeaveId;
                    calendarViewModel.leaveType = subItem.LeaveType;
                    calendarViewModel.start = subItem.LeaveStartDate;
                    calendarViewModel.end = subItem.LeaveEndDate;
                    calendarViewModel.title = empDetails.FirstName;
                    calendarViewModel.description = subItem.LeaveReason;
                    //calendarViewModel.color = "#ff4000";
                    calendarViewModel.LeaveStatus = subItem.LeaveStatus;
                    foreach (var obj in subItem.EmployeeLeaveDetails)
                    {
                        calendarViewModel.LeaveCategorys.Add(obj.LeaveCategory);
                    }
                    calendarViewModels.Add(calendarViewModel);
                }
            }

            return calendarViewModels;
        }
    }
}
