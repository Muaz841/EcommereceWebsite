using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using EmailSender.EntityFrameworkCore;
using EmailSender.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace EmailSender.Web.Tests
{
    [DependsOn(
        typeof(EmailSenderWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class EmailSenderWebTestModule : AbpModule
    {
        public EmailSenderWebTestModule(EmailSenderEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EmailSenderWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(EmailSenderWebMvcModule).Assembly);
        }
    }
}