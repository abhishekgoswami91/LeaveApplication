using LeaveApp.Core.ViewModel;
using LeaveApp.Data.DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeaveApp.Service.Employee
{
    public interface IEmployeeService
    {
       Task<int> AddEmployeeDetailsAsync(EmployeeDetailsViewModel employeeDetailsViewModel);
       Task<bool> EditEmployeeDetailsAsync(EmployeeDetailsViewModel employeeDetailsViewModel);
       Task<bool> DeleteEmployeeDetailsAsync(int EmployeeDetailId, string UserId, bool DoRemove = false);
       Task<EmployeeDetailsViewModel> GetEmployeeDetailsByIdAsync(int EmployeeDetailId);
       Task<IList<EmployeeListModal>> GetAllEmployeeDetailsAsync();
       IList<KeyValuePair<string, string>> GetEmployeeDDLAsync();
       Task<bool> AddEmployeeDocumentAsync(DocumentUploadModel documentUploadModel);
       Task<bool> DeleteEmployeeDocumentAsync(int DocumentId, string UserId, bool DoRemove = false);
       Task<Data.DataModel.EmployeeDocument> GetEmployeeDocumentByIdAsync(int DocumentId);
       Task<ProfileViewModel> GetProfileData(string UserId);
    }
}
