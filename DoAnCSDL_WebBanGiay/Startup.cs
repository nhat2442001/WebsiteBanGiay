using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DoAnCSDL_WebBanGiay.Startup))]
namespace DoAnCSDL_WebBanGiay
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
