using LeaveApp.Core.ViewModel;
using LeaveApp.Service.API;
using LeaveApp.Web.Controllers;
using System.Web.Mvc;
using Unity;
using Unity.Injection;
using Unity.Mvc5;

namespace LeaveApp.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            container.RegisterType<IApiService, ApiService>();
            container.RegisterType<ResponseModel>();
            container.RegisterType<AccountController>(new InjectionConstructor(
                typeof(IApiService), 
                typeof(ResponseModel)
                ));
            container.RegisterType<HomeController>(new InjectionConstructor(typeof(IApiService)));
            container.RegisterType<ManageController>(new InjectionConstructor());
            container.RegisterType<UserController>
                (new InjectionConstructor(
                typeof(IApiService),
                typeof(ResponseModel)
                ));

            // e.g. container.RegisterType<ITestService, TestService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}