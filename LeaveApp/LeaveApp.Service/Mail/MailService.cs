using LeaveApp.Core.ViewModel;
using LeaveApp.Data.Identity;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;

namespace LeaveApp.Service.Mail
{
    public class MailService : IMailService
    {
        //Fetching Settings from APP.CONFIG file.  
        private string emailSender = WebConfigurationManager.AppSettings["emailsender"].ToString();
        private string toEmail = WebConfigurationManager.AppSettings["toemail"].ToString();
        private string emailSenderPassword = WebConfigurationManager.AppSettings["password"].ToString();
        private string emailSenderHost = WebConfigurationManager.AppSettings["smtp"].ToString();
        private int emailSenderPort = Convert.ToInt16(WebConfigurationManager.AppSettings["portnumber"]); //// Gmail can use ports 25, 465 & 587; but must be 25 for medium trust environment.  ;
        private Boolean emailIsSSL = Convert.ToBoolean(WebConfigurationManager.AppSettings["IsSSL"]);
        //Fetching Email Body Text from EmailTemplate File.  
        private string FilePathTemplate = WebConfigurationManager.AppSettings["filepath"].ToString();
        private string PriviewUrl = WebConfigurationManager.AppSettings["priviewurl"].ToString();
        private readonly ApplicationDbContext _db;
        public MailService(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }
        
        public bool SendApplyLeaveMail(MailModel model, bool ToAdmin = false)
        {
            
            StreamReader str = new StreamReader(FilePathTemplate);
            string MailText = str.ReadToEnd();
            str.Close();

            Data.DataModel.EmployeeLeave employeeLeave = new Data.DataModel.EmployeeLeave();

            employeeLeave = _db.EmployeeLeaves.Where(x => x.EmployeeLeaveId.Equals(model.LeaveId)).FirstOrDefault();
            if (employeeLeave == null)
            {
                return false;
            }
            var User = _db.Users.Where(x => x.Id.Equals(employeeLeave.EmployeeId)).FirstOrDefault();

            //Repalce [newusername] = signup user name   
            MailText = MailText.Replace("[[Employee]]", User.UserName);
            MailText = MailText.Replace("[[LeaveType]]", Regex.Replace(employeeLeave.LeaveType.ToString(), "(\\B[A-Z])", " $1"));
            MailText = MailText.Replace("[[StartDate]]", employeeLeave.LeaveStartDate.ToString("MM/dd/yyyy"));
            MailText = MailText.Replace("[[EndDate]]", employeeLeave.LeaveEndDate.ToString("MM/dd/yyyy"));
            MailText = MailText.Replace("[[Reason]]", employeeLeave.LeaveReason);
            MailText = MailText.Replace("[[NoOfLeave]]", employeeLeave.NumberOfDaysLeave.ToString());
            MailText = MailText.Replace("[[Status]]", Regex.Replace(employeeLeave.LeaveStatus.ToString(), "(\\B[A-Z])", " $1"));

            //EmployeeLeaveDetail employeeLeaveDetail = new EmployeeLeaveDetail();
            StringBuilder subTbl = new StringBuilder();
            foreach (var item in employeeLeave.EmployeeLeaveDetails)
            {
                subTbl.Append("<tr class=\"subValues\">");
                subTbl.Append("<td colspan=\"5\" style=\"vertical-align: top; padding: 0px 0px 5px 40px; \">" + item.LeaveDate.ToString("MM/dd/yyyy") + "</td>");
                subTbl.Append("<td colspan=\"3\" style=\"vertical-align: top; padding: 0px 0px 5px 40px; \">" + Regex.Replace(item.LeaveCategory.ToString(), "(\\B[A-Z])", " $1") + "</td>");
                subTbl.Append("</tr>");
            }

            var url = PriviewUrl + model.LeaveId;
            string ToMsg;
            string Message;
            if (ToAdmin)
            {
                model.To = toEmail;
                model.Subject = User.UserName + " Apply for Leave.";
                ToMsg = "Hi Admin,";
                Message = "You can change the status of leave by clicking on the below link.";
            }
            else
            {
                model.To = User.Email;
                model.Subject = "Admin's reply from AvidClan.";
                ToMsg = "Hi "+ User.UserName + ",";
                Message = "You can check the status of leave by clicking on the below link.";
            }
            

            MailText = MailText.Replace("[[Header]]", model.Subject);
            MailText = MailText.Replace("[[ToMsg]]", ToMsg);
            MailText = MailText.Replace("[[Message]]", Message);
            MailText = MailText.Replace("[[LeaveDetails]]", subTbl.ToString());
            MailText = MailText.Replace("[[URL]]", url);
            //Base class for sending email  
            MailMessage _mailmsg = new MailMessage();

            //Make TRUE because our body text is html  
            _mailmsg.IsBodyHtml = true;
            

            //Set From Email ID  
            _mailmsg.From = new MailAddress(emailSender);

            //Set To Email ID  
            _mailmsg.To.Add(model.To);

            //Set Subject  
            _mailmsg.Subject = model.Subject;

            //Set Body Text of Email   
            _mailmsg.Body = MailText;


            //Now set your SMTP   
            SmtpClient _smtp = new SmtpClient();

            //Set HOST server SMTP detail  
            _smtp.Host = emailSenderHost;

            //Set PORT number of SMTP  
            _smtp.Port = emailSenderPort;

            //Set SSL --> True / False  
            _smtp.EnableSsl = emailIsSSL;

            //Set Sender UserEmailID, Password  
            NetworkCredential _network = new NetworkCredential(emailSender, emailSenderPassword);
            _smtp.Credentials = _network;
            _smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtp.UseDefaultCredentials = false;

            //Send Method will send your MailMessage create above.  
            try
            {
                _smtp.Send(_mailmsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            

            return true;
        }
        //public bool SendEmail(string bodyString, string userMail, string subject)
        //{
        //    bool result = false;
        //    try
        //    {
        //        string UserName = "Prashant.Avidclan@gmail.com";
        //        string Password = "Test@123";
        //        MailMessage message = new MailMessage();
        //        SmtpClient smtp = new SmtpClient();
        //        message.From = new MailAddress(UserName);
        //        message.To.Add(new MailAddress("abhi.avidclan@gmail.com"));
        //        message.Subject = "test";
        //        message.IsBodyHtml = true; //to make message body as html
        //        message.Body = "Test";
        //        smtp.Port = 587;
        //        smtp.Host = "smtp.gmail.com"; //for gmail host
        //        smtp.EnableSsl = true;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new NetworkCredential(UserName, Password);
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtp.Send(message);
        //        result = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    return result;
        //}
       
    }
}
