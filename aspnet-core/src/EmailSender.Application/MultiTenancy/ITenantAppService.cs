using Abp.Application.Services;
using EmailSender.MultiTenancy.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSender.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
        Task<List<TenantDto>> GetAllTenantsAsync();
    }
}

