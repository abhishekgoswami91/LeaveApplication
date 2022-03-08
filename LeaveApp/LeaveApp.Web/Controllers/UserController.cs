using LeaveApp.Core.Enums;
using LeaveApp.Core.ViewModel;
using LeaveApp.Service.API;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Linq;
using System.Web;
using System.IO;
using LeaveApp.Data.DataModel;
using LeaveApp.Web.Models;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;
using System.Text;
using System.Drawing.Imaging;

namespace LeaveApp.Web.Controllers
{
    [Authorize]
    [ExceptionHandler]
    [RoutePrefix("User")]
    public class UserController : Controller
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
                    LogOff();
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
                    LogOff();
                    return "";
                }
                return User.Identity.GetUserId();
            }
            set
            {
                _userId = value;
            }
        }
        public UserController(IApiService apiService, ResponseModel responseModel)
        {
            _apiService = apiService;
            _responseModel = responseModel;
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public async Task<ActionResult> Leave(int LeaveId = 0)
        {
            IList<LeaveListViewModel> resultList = new List<LeaveListViewModel>();

            if (LeaveId > 0)
            {
                var result = await _apiService.MakePrivateApiCallAsync<LeaveListViewModel>("api/User/GetApplyLeave/" + LeaveId, HttpMethod.Get, _token);
                resultList.Add(result);
                return View(resultList);
            }

            var IsAdmin = ((ClaimsIdentity)User.Identity).Claims
                .Any(x => x.Value.Equals("Admin"));

            if (IsAdmin)
            {
                resultList = await _apiService.MakePrivateApiCallAsync<IList<LeaveListViewModel>>("api/User/GetApplyLeaveList", HttpMethod.Get, _token);
            }
            else
            {
                resultList = await _apiService.MakePrivateApiCallAsync<IList<LeaveListViewModel>>("api/User/GetApplyLeaveList/" + _userId, HttpMethod.Get, _token);
            }

            return View(resultList);
        }
        public ActionResult ApplyLeave()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ApplyLeave(ApplyLeaveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return Json(_responseModel);
            }
            model.EmployeeLeave.EmployeeId = _userId;
            model.EmployeeLeave.CreatedBy = _userId;
            model.EmployeeLeave.CreatedDate = DateTime.Now;
            model.EmployeeLeave.DeletedBy = "";
            model.EmployeeLeave.DeletedDate = DateTime.Now;
            model.EmployeeLeave.IsDeleted = false;
            model.EmployeeLeave.ModifiedBy = _userId;
            model.EmployeeLeave.ModifiedDate = DateTime.Now;

            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<bool>("api/User/ApplyLeave", HttpMethod.Post, _token, model);

            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<ActionResult> EditLeave(ApplyLeaveViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return Json(_responseModel);
            }
            model.EmployeeLeave.EmployeeId = _userId;
            //model.EmployeeLeave.CreatedBy = _userId;
            //model.EmployeeLeave.CreatedDate = DateTime.Now;
            //model.EmployeeLeave.DeletedBy = "";
            //model.EmployeeLeave.DeletedDate = DateTime.Now;
            //model.EmployeeLeave.IsDeleted = false;
            model.EmployeeLeave.ModifiedBy = _userId;
            model.EmployeeLeave.ModifiedDate = DateTime.Now;

            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<bool>("api/User/EditLeave", HttpMethod.Post, _token, model);

            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> GetLeaveFormData(int LeaveId)
        {
            if (LeaveId < 1)
            {
                _responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return Json(_responseModel);
            }
            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<ApplyLeaveViewModel>("api/User/GetLeaveFormData/" + LeaveId, HttpMethod.Get, _token);

            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> DeleteLeave(int id)
        {
            if (id < 1)
            {
                _responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return Json(_responseModel);
            }

            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<bool>("api/User/DeleteLeave/" + id, HttpMethod.Get, _token);

            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin")]
        [Route("ResetLeaveStatus/{Id}/{Option}")]
        public async Task<ActionResult> ResetLeaveStatus(int Id, int Option = 1)
        {

            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<bool>("api/User/ResetLeaveStatus/" + Id + "/" + Option, HttpMethod.Get, _token);

            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        //[Authorize(Roles ="Admin")]
        //public ActionResult EmployeeList()
        //{

        //    return View();
        //}
        //[HttpGet]
        //[Authorize(Roles = "Admin")]
        //public  ActionResult AddEmployee()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public async Task<ActionResult> AddEmployee(EmployeeDetailsViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        _responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
        //        return View(_responseModel);
        //    }
        //    model.EmployeeDetail.CreatedBy = _userId;
        //    model.EmployeeDetail.ModifiedBy = _userId;

        //    _responseModel.Data = await _apiService.MakePrivateApiCallAsync<bool>("api/User/AddEmployeeDetails/", HttpMethod.Post, _token, model);
        //    return View(_responseModel);
        //}
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> EmployeeDetails(int Id)
        {
            if (Id < 1)
            {
                _responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return View(_responseModel);
            }

            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<EmployeeDetailsViewModel>("api/User/GetEmployeeDetails/" + Id, HttpMethod.Get, _token);
            return View(_responseModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> GetEditEmployee(int Id = 0)
        {
            if (Id < 1)
            {
                _responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return Json(_responseModel);
            }

            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<EmployeeDetailsViewModel>("api/User/GetEmployeeDetails/" + Id, HttpMethod.Get, _token);
            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult EditEmployee()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditEmployee(EmployeeDetailsViewModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    _responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
            //    return View(_responseModel);
            //}
            model.EmployeeDetail.ModifiedBy = _userId;

            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<bool>("api/User/EditEmployeeDetails/", HttpMethod.Post, _token, model);
            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EmployeeList()
        {
            var Data = await _apiService.MakePrivateApiCallAsync<List<EmployeeListModal>>("api/User/GetAllEmployeeDetails/", HttpMethod.Get, _token);
            return View(Data);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult> EmployeeDetail(int Id = 0)
        {
            var Data = await _apiService.MakePrivateApiCallAsync<EmployeeDetailsViewModel>("api/User/GetEmployeeDetails/" + Id, HttpMethod.Get, _token);
            return View(Data);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteEmployee(int Id, bool DoRemove = false)
        {
            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<bool>("api/User/DeleteEmployeeDetails/" + Id + "/" + _userId + "/" + DoRemove, HttpMethod.Get, _token);
            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteEmployeeDocument(int Id, bool DoRemove = false)
        {
            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<bool>("api/User/DeleteEmployeeDocument/" + Id + "/" + _userId + "/" + DoRemove, HttpMethod.Get, _token);
            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetEmployeeDDL()
        {
            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<List<KeyValuePair<string, string>>>("api/User/GetEmployeeDDL/", HttpMethod.Get, _token);
            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }

        #region Upload / Download file  
        [HttpPost]
        public async Task<ActionResult> UploadEmployeeDocuments()
        {
            if (HttpContext.Request.Files.Count < 1)
            {
                _responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                return Json(_responseModel);
            }
            DocumentUploadModel FdList = new DocumentUploadModel();
            for (int i = 0; i < HttpContext.Request.Files.Count; i++)
            {
                var file = HttpContext.Request.Files[i];
                String FileExt = Path.GetExtension(file.FileName).ToUpper();
                List<string> acceptExtList = new List<string> { ".PDF", ".DOC", ".DOCX", ".JPG", ".JPEG", ".PNG" };
                var doUpload = acceptExtList.Any(x => x.Equals(FileExt));
                if (!doUpload)
                {
                    _responseModel.Error = ResponseMessages.FormDataNotValid.ToString();
                    return Json(_responseModel);
                }
                Stream str = file.InputStream;
                BinaryReader Br = new BinaryReader(str);
                Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                Core.ViewModel.EmployeeDocument Fd = new Core.ViewModel.EmployeeDocument();
                Fd.FileName = file.FileName;
                Fd.FileContent = FileDet;
                Fd.CreatedBy = _userId;
                Fd.ModifiedBy = _userId;
                FdList.EmployeeDocuments.Add(Fd);
            }
            if (Request["EmployeeDetailId"] != null)
            {
                FdList.EmployeeDetailId = Convert.ToInt32(Request["EmployeeDetailId"]);
            }
            else
            {
                FdList.EmployeeDetailId = Convert.ToInt32(Session["EmployeeDetailId"]);
            }
            Session["EmployeeDetailId"] = null;


            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<bool>("api/User/AddEmployeeDocument/", HttpMethod.Post, _token, FdList);

            return Json(_responseModel);
        }
        [HttpGet]
        public async Task<FileResult> DownloadEmployeeDocument(int id)
        {
            var response = await _apiService.MakePrivateApiCallAsync<Data.DataModel.EmployeeDocument>("api/User/GetEmployeeDocument/" + id, HttpMethod.Get, _token);
            return File(response.FileContent, "application/pdf", response.FileName);
        }
        [HttpGet]
        public async Task<ActionResult> DisplayEmployeeDocument(int id)
        {
            var response = await _apiService.MakePrivateApiCallAsync<Data.DataModel.EmployeeDocument>("api/User/GetEmployeeDocument/" + id, HttpMethod.Get, _token);
            MemoryStream fileStream = new MemoryStream();
            fileStream.Write(response.FileContent, 0, response.FileContent.Length);
            fileStream.Position = 0;
            string FileExt = Path.GetExtension(response.FileName).ToUpper().ToString();
            switch (FileExt)
            {
                case ".JPG":
                case ".JPEG":
                case ".PNG":
                case ".GIF":
                    return new FileStreamResult(fileStream, "image/jpg");
                case ".PDF":
                    return new FileStreamResult(fileStream, "application/pdf");
                case ".DOC":
                case ".DOCX":
                    try
                    {
                        ViewBag.WordHtml = GetDoc(response);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = ex.Message;
                        return View("_Doc");
                        throw;
                    }
                    return View("_Doc");
                default:
                    return new FileStreamResult(fileStream, "application/pdf");
            }
        }

        private string GetDoc(Data.DataModel.EmployeeDocument docx)
        {
            object documentFormat = 8;
            string randomName = DateTime.Now.Ticks.ToString();
            object htmlFilePath = Server.MapPath("~/Temp/") + randomName + ".htm";
            string directoryPath = Server.MapPath("~/Temp/") + randomName + "_files";
            object fileSavePath = Server.MapPath("~/Temp/") + randomName + "_" + Path.GetFileName(docx.FileName);

            //If Directory not present, create it.
            if (!Directory.Exists(Server.MapPath("~/Temp/")))
            {
                Directory.CreateDirectory(Server.MapPath("~/Temp/"));
            }

            System.IO.File.WriteAllBytes(fileSavePath.ToString(), docx.FileContent);

            //Open the word document in background.
            _Application applicationclass = new Application();
            applicationclass.Documents.Open(ref fileSavePath);
            applicationclass.Visible = true;
            Document document = applicationclass.ActiveDocument;

            //Save the word document as HTML file.
            document.SaveAs(ref htmlFilePath, ref documentFormat);

            //Close the word document.
            document.Close();

            //Read the saved Html File.
            string wordHTML = System.IO.File.ReadAllText(htmlFilePath.ToString());

            //Loop and replace the Image Path.
            foreach (Match match in Regex.Matches(wordHTML, "<v:imagedata.+?src=[\"'](.+?)[\"'].*?>", RegexOptions.IgnoreCase))
            {
                wordHTML = Regex.Replace(wordHTML, match.Groups[1].Value, "Temp/" + match.Groups[1].Value);
            }

            //Delete the Uploaded Word File.
            //System.IO.File.Delete(fileSavePath.ToString());
            //System.IO.File.Delete(htmlFilePath.ToString());
            //System.IO.Directory.Delete(directoryPath.ToString(),true);

            return wordHTML.ToString();
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        public async Task<ActionResult> GetLeaveRules()
        {
            return Json(await GetLeaveRulesAsync(), JsonRequestBehavior.AllowGet);
        }

        private async Task<LeaveRulesViewModel> GetLeaveRulesAsync()
        {
            return await _apiService.MakePrivateApiCallAsync<LeaveRulesViewModel>("api/User/GetLeaveRules/" + _userId, HttpMethod.Get, _token);
        }
        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        public async Task<ActionResult> GetUserProfile()
        {
            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<ProfileViewModel>("api/User/GetProfileData/" + _userId, HttpMethod.Get, _token);
            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddBonusLeaves(int EmployeeId, int Leaves, LeaveType LeaveType)
        {
            _responseModel.Data = await _apiService.MakePrivateApiCallAsync<bool>("api/User/AddBonusLeaves/" + _userId + "/" + EmployeeId + "/" + Leaves + "/" + LeaveType, HttpMethod.Post, _token);
            return Json(_responseModel, JsonRequestBehavior.AllowGet);
        }
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        #endregion
        public ActionResult LogOff()
        {
            return RedirectToAction("LogOff", "Account");
        }
    }
}