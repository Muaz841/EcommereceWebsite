using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using EmailSender.EntityFrameworkCore.Seed.Host;
using EmailSender.EntityFrameworkCore.Seed.Tenants;
using EmailSender.EntityFrameworkCore.Seed.Email;
using System.Threading.Tasks;
using Abp.Runtime.Session;
using Abp.Domain.Repositories;
using EmailSender.MultiTenancy;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EmailSender.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        public static async Task SeedHostDb(IIocResolver iocResolver)
        {
            await WithDbContextAsync<EmailSenderDbContext>(iocResolver, SeedHostDb);
        }

        public static async Task SeedHostDb(EmailSenderDbContext context)
        {
            context.SuppressAutoSetTenantId = true;

           
            new InitialHostDbBuilder(context).Create();

         
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();

            
            var abpSession = context.GetService<IAbpSession>();
            var tenantRepository = context.GetService<IRepository<Tenant, int>>();
            var unitOfWorkManager = context.GetService<IUnitOfWorkManager>();

            
            await new EmailTemplateBuilder(context, abpSession, tenantRepository, unitOfWorkManager).Create();
        }

        private static async Task WithDbContextAsync<TDbContext>(IIocResolver iocResolver, Func<TDbContext, Task> contextAction)
     where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    await contextAction(context);

                    await uow.CompleteAsync();
                }
            }
        }
    }
}
