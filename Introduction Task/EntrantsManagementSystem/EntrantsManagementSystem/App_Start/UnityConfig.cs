using System.Web.Mvc;
using Unity;
using Unity.Mvc5;
using EntrantsManagementSystem.Logging; 
namespace EntrantsManagementSystem
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<ILogger, DatabaseLogger>();
           

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}