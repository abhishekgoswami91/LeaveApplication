using LeaveApp.Core.Enums;
using LeaveApp.Core.ViewModel;
using LeaveApp.Service.API;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LeaveApp.Web.Controllers
{
    [Authorize]
    [ExceptionHandler]
    [RoutePrefix("Utility")]
    public class UtilityController : Controller
    {
        private readonly IApiService _apiService;
        private ResponseModel _responseModel;
        private string _token
        {
            get
            {
                HttpCookie cookie = Request.Cookies["API_ACCESS"];
                if (cookie == null)
                {
                    return "";
                }
                return cookie["AccessToken"];
            }
            set
            {
                _token = value;
            }
        }
        private string _userId
        {
            get
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return "";
                }
                return User.Identity.GetUserId();
            }
            set
            {
                _userId = value;
            }
        }
        public UtilityController(IApiService apiService, ResponseModel responseModel)
        {
            _apiService = apiService;
            _responseModel = responseModel;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult> AddIssue(IssueViewModel model)
        {
            model.CreatedBy = _userId;
            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<bool>("api/Utility/AddIssue", HttpMethod.Post, _token, model);
            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Issue(int Id = 0)
        {
            List<IssueViewModel> response = await _apiService.MakePrivateApiCallAsync<List<IssueViewModel>>("api/Utility/GetIssueList/" + Id, HttpMethod.Get, _token);
            return View(response);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeIssueStatus(int IssueId, IssueStatus IssueStatus)
        {
            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<bool>("api/Utility/ChangeIssueStatus/" + IssueId + "/" + IssueStatus, HttpMethod.Post, _token);
            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> DeleteIssue(int Id)
        {
            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<List<CalendarViewModel>>("api/Utility/GetCalendarData/" + _userId, HttpMethod.Get, _token);
            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DisplayMedia(string image)
        {
            ViewBag.WordHtml = image;
            return View("_media");
        }
    }
}