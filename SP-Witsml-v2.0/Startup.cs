using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SP_Witsml_v2._0.Startup))]
namespace SP_Witsml_v2._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
