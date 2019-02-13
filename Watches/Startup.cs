using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Watches.Startup))]
namespace Watches
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
