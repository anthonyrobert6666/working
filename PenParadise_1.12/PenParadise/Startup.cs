using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PenParadise.Startup))]
namespace PenParadise
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
