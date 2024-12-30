using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using EmailSender.Authorization;
using EmailSender.EmailSender;
using EmailSender.EmailSender.EmailWorker;

namespace EmailSender
{
    [DependsOn(
        typeof(EmailSenderCoreModule), 
        typeof(AbpAutoMapperModule),
         typeof(CategoryMappings),
         typeof(AbpAutoMapperModule),
        typeof(EmailMappings))]
    public class EmailSenderApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<EmailSenderAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(EmailSenderApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);
            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
        public override void PostInitialize()
        {
            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
           workManager.Add(IocManager.Resolve<EmailSendingWorker>());
        }
    }
}
