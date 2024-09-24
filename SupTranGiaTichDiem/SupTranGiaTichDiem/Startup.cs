using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartup(typeof(SupTranGiaTichDiem.Startup))]

namespace SupTranGiaTichDiem
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //app.UseCookieAuthentication(new CookieAuthenticationOptions()
            //{
            //    AuthenticationType =DefaultAuthenticationTypes.ApplicationCookie,
            //    LoginPath=new PathString("/DangNhap/login")
            //}
            //    );

            
        }
    }
}
