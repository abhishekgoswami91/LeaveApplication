using LeaveApp.Core.ViewModel;
using LeaveApp.Data.DataModel;
using LeaveApp.Service.Issue;
using LeaveApp.Service.Mail;
using System.Threading.Tasks;
using System.Web.Http;

namespace LeaveApp.API.Controllers
{
    [RoutePrefix("api/Utility")]
    public class UtilityController : ApiController
    {
        private IMailService _mailService;
        private readonly IIssueService _issueService;
        public UtilityController(IMailService mailService, IIssueService IssueService)
        {
            _mailService = mailService;
            _issueService = IssueService;
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        [Route("AddIssue")]
        public async Task<IHttpActionResult> AddIssue(IssueViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Ok(false);
            }
            int response = await _issueService.AddIssueAsync(model);
            IssueMailModel issueMailModel = new IssueMailModel();
            issueMailModel.IssueData = model;
            issueMailModel.Subject = "Issue By: " + model.CreatedBy;
            _mailService.SendIssueMail(issueMailModel);
            if (response > 0)
                return Ok(true);
            else
                return Ok(false);
        } 
    }
}
