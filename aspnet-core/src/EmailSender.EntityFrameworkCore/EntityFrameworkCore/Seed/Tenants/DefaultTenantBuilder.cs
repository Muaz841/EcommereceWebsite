using System.Linq;
using Microsoft.EntityFrameworkCore;
using Abp.MultiTenancy;
using EmailSender.Editions;
using EmailSender.MultiTenancy;
using EmailSender.EmailSender.EmailSenderEntities;
using System;
using EmailSender.EntityFrameworkCore.Seed.Email;

namespace EmailSender.EntityFrameworkCore.Seed.Tenants
{
    public class DefaultTenantBuilder
    {
        private readonly EmailSenderDbContext _context;

        public DefaultTenantBuilder(EmailSenderDbContext context)
        {
            _context = context;
        }

        public async void Create()
        {
            CreateDefaultTenant();
            CreateDummyTenant("Second", "Dummy Tenant 2");
        }

        private void CreateDefaultTenant()
        {
            // Default tenant

            var defaultTenant = _context.Tenants.IgnoreQueryFilters().FirstOrDefault(t => t.TenancyName == AbpTenantBase.DefaultTenantName);
            if (defaultTenant == null)
            {
                defaultTenant = new Tenant(AbpTenantBase.DefaultTenantName, AbpTenantBase.DefaultTenantName);

                var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
                if (defaultEdition != null)
                {
                    defaultTenant.EditionId = defaultEdition.Id;
                }

                _context.Tenants.Add(defaultTenant);
                _context.SaveChanges();
            }
        }
        private void CreateDummyTenant(string tenancyName, string name)
        {
            var dummyTenant = _context.Tenants.IgnoreQueryFilters().FirstOrDefault(t => t.TenancyName == tenancyName);
            if (dummyTenant == null)
            {
                dummyTenant = new Tenant(tenancyName, name);

                var defaultEdition = _context.Editions.IgnoreQueryFilters().FirstOrDefault(e => e.Name == EditionManager.DefaultEditionName);
                if (defaultEdition != null)
                {
                    dummyTenant.EditionId = defaultEdition.Id;
                }

                _context.Tenants.Add(dummyTenant);
                _context.SaveChanges();
            }
        }
    }
}
