using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Session;
using Abp.Zero.EntityFrameworkCore;
using EmailSender.EntityFrameworkCore.Seed;

namespace EmailSender.EntityFrameworkCore
{
    [DependsOn(
        typeof(EmailSenderCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class EmailSenderEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }
        private readonly IAbpSession _abpSession;

        public bool SkipDbSeed { get; set; }

            public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<EmailSenderDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        EmailSenderDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        EmailSenderDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }
    

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EmailSenderEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
