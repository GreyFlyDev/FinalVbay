using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Vbay.Startup))]
namespace Vbay
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
