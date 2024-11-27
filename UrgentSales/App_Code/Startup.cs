using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UrgentSales.Startup))]
namespace UrgentSales
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
