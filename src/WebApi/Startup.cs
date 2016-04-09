using Microsoft.Owin;
using Owin;
using System.Web;
using System.Web.Http;


[assembly: OwinStartupAttribute(typeof(WebApi.Startup))]
namespace WebApi
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
        }

    }
}

