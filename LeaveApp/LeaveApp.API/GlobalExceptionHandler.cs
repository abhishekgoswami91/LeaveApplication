using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace LeaveApp.API
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            Exception e = context.Exception;
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "MailTemplates/error.txt", e.Message + "log time: "+ DateTime.Now);
            var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("-1"),
                ReasonPhrase = "AvidClan API",
            };

            context.Result = new ResponseMessageResult(response);
        }
    }
}