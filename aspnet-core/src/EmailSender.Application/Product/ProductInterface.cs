
using Abp.Application.Services.Dto;
using EmailSender.ProductDomain;
using EmailSender.ProductDtos;
using EmailSender.ProductEntities;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace EmailSender.ProductServices
{
    public interface IProductAppService 
    {
        
        Task CreateAsync(CreateUpdateProductDto input);
         Task<CreateUpdateProductDto> GetById(int? input);

        Task DeleteAsync(List<int> input);

        Task GetExcelFile();
        Task<PagedResultDto<ProductDto>> GetAllAsync(PagedProductResultRequestDto input);
                      
        Task UpdateAsync(CreateUpdateProductDto input);

        Task<List<DiscountType>> GetDiscountTypes();
    }
}
