using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Business.Mvc.Startup))]
namespace Business.Mvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
