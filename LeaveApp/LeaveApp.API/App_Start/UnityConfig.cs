using LeaveApp.API.Controllers;
using LeaveApp.Core.ViewModel;
using LeaveApp.Service.Employee;
using LeaveApp.Service.Leave;
using LeaveApp.Service.Mail;
using System.Web.Http;
using Unity;
using Unity.Injection;
using Unity.WebApi;

namespace LeaveApp.API
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<ILeaveService, LeaveService>();
            container.RegisterType<IMailService, MailService>();
            container.RegisterType<IEmployeeService, EmployeeService>();
            container.RegisterType<ResponseModel>();
            container.RegisterType<UserController>
                (new InjectionConstructor(
                    typeof(ILeaveService),
                    typeof(ResponseModel),
                    typeof(IMailService),
                    typeof(IEmployeeService)
                    ));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}