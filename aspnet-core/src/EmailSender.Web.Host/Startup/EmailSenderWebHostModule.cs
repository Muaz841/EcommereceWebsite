using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using EmailSender.Configuration;

namespace EmailSender.Web.Host.Startup
{
    [DependsOn(
       typeof(EmailSenderWebCoreModule))]
    public class EmailSenderWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public EmailSenderWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EmailSenderWebHostModule).GetAssembly());
        }
    }
}
