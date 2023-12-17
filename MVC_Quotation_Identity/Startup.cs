using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_Quotation_Identity.Startup))]
namespace MVC_Quotation_Identity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
