using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PBDE401.Startup))]
namespace PBDE401
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
