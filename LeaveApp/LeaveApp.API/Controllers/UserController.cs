using LeaveApp.Core.Enums;
using LeaveApp.Core.ViewModel;
using LeaveApp.Service.Leave;
using LeaveApp.Service.Employee;
using LeaveApp.Service.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using LeaveApp.Data.DataModel;

namespace LeaveApp.API.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly ILeaveService _leaveService;
        private readonly IEmployeeService _employeeService;
        private ResponseModel _responseModel;
        private IMailService _mailService;
        public UserController(ILeaveService leaveService,
            ResponseModel responseModel,
            IMailService mailService,
            IEmployeeService employeeService
            )
        {
            _leaveService = leaveService;
            _responseModel = responseModel;
            _mailService = mailService;
            _employeeService = employeeService;
        }

        [HttpPost]
        [Route("ApplyLeave")]
        public async Task<IHttpActionResult> ApplyLeave(ApplyLeaveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //_responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return Ok(false);
            }
            var leaveId = await _leaveService.ApplyLeaveAsync(model);
            MailModel mailModel = new MailModel();
            mailModel.LeaveId = leaveId;
            var response = _mailService.SendApplyLeaveMail(mailModel, true);
            return Ok(response);
        }

        [HttpPost]
        [Route("EditLeave")]
        public async Task<IHttpActionResult> EditLeave(ApplyLeaveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //_responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return Ok(false);
            }
            var leaveId = await _leaveService.EditLeaveAsync(model);
            MailModel mailModel = new MailModel();
            mailModel.LeaveId = leaveId;
            var response = _mailService.SendApplyLeaveMail(mailModel, true);
            return Ok(response);
        }

        [HttpGet]
        [Route("GetApplyLeaveList/{employeeId}")]
        public async Task<IHttpActionResult> GetApplyLeaveList(string employeeId)
        {
            IList<LeaveListViewModel> response = new List<LeaveListViewModel>();
            if (string.IsNullOrEmpty(employeeId))
            {
                //_responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return Ok(response);
            }

             response = await _leaveService.GetApplyedLeaveListAsync(employeeId);
            return Ok(response);
        }
        [HttpGet]
        [Route("GetApplyLeave/{leaveId}")]
        public async Task<IHttpActionResult> GetApplyLeave(int leaveId)
        {
            LeaveListViewModel response = new LeaveListViewModel();
            if (leaveId < 1)
            {
                //_responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return Ok(response);
            }

            response = await _leaveService.GetApplyedLeaveAsync(leaveId);
            return Ok(response);
        }
        [HttpGet]
        [Route("GetApplyLeaveList")]
        public async Task<IHttpActionResult> GetApplyLeaveList()
        {
            IList<LeaveListViewModel> response = await _leaveService.GetApplyedLeaveListAsync();
            return Ok(response);
        }

        [HttpGet]
        [Route("DeleteLeave/{id}")]
        public async Task<IHttpActionResult> DeleteLeave(int id)
        {
            if (id < 1)
            {
                //_responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return Ok(false);
            }
            var response = await _leaveService.DeleteLeaveAsync(id);

            return Ok(response);
        }
        
        [Authorize(Roles="Admin")]
        [HttpGet]
        [Route("ResetLeaveStatus/{Id}/{Option}")]
        public async Task<IHttpActionResult> ResetLeaveStatus(int Id, int Option = 1)
        {

            if (Id < 1)
            {
                return Ok(false);
            }

            var response = await _leaveService.ResetLeaveStatus(Id, (LeaveStatus)Option);
            MailModel mailModel = new MailModel();
            mailModel.LeaveId = Id;
            _mailService.SendApplyLeaveMail(mailModel, false);

            return Ok(response);
        }

        [HttpGet]
        [Route("GetLeaveFormData/{LeaveId}")]
        public async Task<IHttpActionResult> GetLeaveFormData(int LeaveId)
        {
            ApplyLeaveViewModel response = new ApplyLeaveViewModel();
            if (LeaveId < 1)
            {
                //_responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return Ok(response);
            }

            response = await _leaveService.GetApplyedLeaveFormDataAsync(LeaveId);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddEmployeeDetails")]
        public async Task<IHttpActionResult> AddEmployeeDetails(EmployeeDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(0);
            }

            var response = await _employeeService.AddEmployeeDetailsAsync(model);

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetEmployeeDDL")]
        public  IHttpActionResult GetEmployeeDDL()
        {
            var response =  _employeeService.GetEmployeeDDLAsync();
            return Ok(response);
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        [Route("GetEmployeeDetails/{Id}")]
        public async Task<IHttpActionResult> GetEmployeeDetails(int Id)
        {
            EmployeeDetailsViewModel employeeDetailsViewModel = new EmployeeDetailsViewModel();
            if (Id < 1)
            {
                return Ok(employeeDetailsViewModel);
            }

             employeeDetailsViewModel = await _employeeService.GetEmployeeDetailsByIdAsync(Id);

            return Ok(employeeDetailsViewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("EditEmployeeDetails")]
        public async Task<IHttpActionResult> EditEmployeeDetails(EmployeeDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(false);
            }

            var response = await _employeeService.EditEmployeeDetailsAsync(model);
            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAllEmployeeDetails")]
        public async Task<IHttpActionResult> GetAllEmployeeDetails()
        {
            var response = await _employeeService.GetAllEmployeeDetailsAsync();

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("DeleteEmployeeDetails/{Id}/{UserId}/{DoRemove}")]
        public async Task<IHttpActionResult> DeleteEmployeeDetails(int Id, string UserId, bool DoRemove = false)
        {
            var response = await _employeeService.DeleteEmployeeDetailsAsync(Id, UserId, DoRemove);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddEmployeeDocument")]
        public async Task<IHttpActionResult> AddEmployeeDocument(DocumentUploadModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(false);
            }

            var response = await _employeeService.AddEmployeeDocumentAsync(model);

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("DeleteEmployeeDocument/{Id}/{UserId}/{DoRemove}")]
        public async Task<IHttpActionResult> DeleteEmployeeDocument(int Id, string UserId, bool DoRemove = false)
        {
            var response = await _employeeService.DeleteEmployeeDocumentAsync(Id, UserId, DoRemove);
            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetEmployeeDocument/{Id}")]
        public async Task<IHttpActionResult> GetEmployeeDocument(int Id)
        {
            var response = await _employeeService.GetEmployeeDocumentByIdAsync(Id);
            return Ok(response);
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        [Route("GetLeaveRules/{EmployeeId}")]
        public async Task<IHttpActionResult> GetLeaveRules(string EmployeeId)
        {
            var response = await _leaveService.GetLeaveRulesAsync(EmployeeId);
            return Ok(response);
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        [Route("GetProfileData/{UserId}")]
        public async Task<IHttpActionResult> GetProfileData(string UserId)
        {
            var response = await _employeeService.GetProfileData(UserId);
            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("AddBonusLeaves/{UserId}/{EmployeeId}/{Leaves}/{LeaveType}")]
        public async Task<IHttpActionResult> AddBonusLeaves(string UserId, int EmployeeId, int Leaves, LeaveType LeaveType)
        {
            var response = await _leaveService.AddBonusLeavesAsync(UserId, EmployeeId, Leaves, LeaveType);
            return Ok(response);
        }
        //[HttpGet]
        //[AllowAnonymous]
        //[Route("SendMail")]
        //public async Task<IHttpActionResult> SendMail()
        //{
        //    bool result = false;
        //    //if (!ModelState.IsValid)
        //    //{
        //    //    _responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
        //    //    return Ok(_responseModel);
        //    //}
        //    //var response = _mailService.SendApplyLeaveMail(model, true);
        //    MailModel mailModel = new MailModel();
        //    mailModel.LeaveId = 2;
        //    try
        //    {
        //        result = _mailService.SendApplyLeaveMail(mailModel, true);
        //    }
        //    catch (System.Exception ex)
        //    {

        //        return Ok(ex);
        //    }

        //    return Ok(result);
        //}

    }
}