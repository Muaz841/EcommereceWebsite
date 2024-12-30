using Abp.Application.Services;
using Abp.Application.Services.Dto;
using EmailSender.CategoryServices.CategoryDtos;
using System.Threading.Tasks;

namespace EmailSender.CategoryServices
{
    public interface ICategoryService 
    {
        Task<PagedResultDto<CategoryDto>> GetAllAsync(PagedCategoryResultRequestDto input);

        Task Update(CategoryDto input);
        Task<CategoryDto> GetById(int input); 

        Task GetExcelFile();
        Task CreateAsync(CategoryDto input);
    }
}
