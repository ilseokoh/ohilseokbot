using Microsoft.Practices.Unity;
using OhIlSeokBot.KakaoPlusFriend.Services;
using System.Web;
using System.Web.Mvc;
using Unity.Mvc5;

namespace OhIlSeokBot.KakaoPlusFriend
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<ISessionService, DocumentDBSessionService>();
            container.RegisterType<IDirectLineConversationService, DirectLineCoversationService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
