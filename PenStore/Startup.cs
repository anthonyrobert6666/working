using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PenStore.Startup))]
namespace PenStore
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
