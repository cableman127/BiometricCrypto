using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BiometricCryptosystem.Startup))]
namespace BiometricCryptosystem
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
