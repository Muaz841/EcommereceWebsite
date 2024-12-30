using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using EmailSender.ProductDomain;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Abp.ObjectMapping;
using EmailSender.CategoryServices.CategoryDtos;
using EmailSender.Export;
using EmailSender.CategoryEntity;
using EmailSender.ProductEntities;


namespace EmailSender.CategoryServices
{
    public class CategoryService : EmailSenderAppServiceBase, ICategoryService
    {
        private readonly IRepository<Category, int> _categoryRepository;
        private readonly IRepository<ProductCategory, int> _productCategoryRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly ExportToExcel _excel;

        public CategoryService(IRepository<Category, int> categoryRepository,
            IRepository<ProductCategory, int> productCategoryRepository,
            ExportToExcel excel,IObjectMapper objectMapper)
        {
            _categoryRepository = categoryRepository;
            _productCategoryRepository = productCategoryRepository;
            _objectMapper = objectMapper;
            _excel = excel;
        }

        public async Task<PagedResultDto<CategoryDto>> GetAllAsync(PagedCategoryResultRequestDto input)
        {
           var filter = string.IsNullOrWhiteSpace(input.FilterText) ? string.Empty : input.FilterText.Trim().ToLower();

          var  filteredcategories = _categoryRepository.GetAll().AsNoTracking()
              .WhereIf(!string.IsNullOrEmpty(filter), Category => Category.Name.ToLower().Contains(filter) &&
                       (!input.StartDate.HasValue || Category.CreationTime >= input.StartDate.Value) &&
                       (!input.EndDate.HasValue || Category.CreationTime <= input.EndDate.Value));

           var totalCount = await filteredcategories.CountAsync();
          
           var paged = await  filteredcategories.OrderBy(input.Sorting ?? "id desc")
                                                  .PageBy(input.SkipCount,input.MaxResultCount)
                                                  .ToListAsync();

                            return new PagedResultDto<CategoryDto>(
                                                     totalCount,
                                                     ObjectMapper.Map<List<CategoryDto>>(paged));
        }

        public async Task Update(CategoryDto input)
        {
            await _categoryRepository.GetAll().Where(c=> c.Id  == input.Id)
                      .UpdateFromQueryAsync(c => new Category
                      {
                          Thumbnail = input.Thumbnail,
                          Name = input.Name,
                          Description = input.Description,
                      });
        }

        public async Task CreateAsync(CategoryDto input)
        {
            
            var category = _objectMapper.Map<Category>(input);
            await _categoryRepository.InsertAsync(category);            
        }

        public async Task<List<CategoryListDto>> GetList()
        {
            
            var categories = await _categoryRepository.GetAll()
                .AsNoTracking()
                .Select(category => new CategoryListDto
                {
                    Id = category.Id,
                    Name = category.Name
                })
                .ToListAsync();

            return categories;
        }

        public  async Task GetExcelFile()
        {
            var category = _categoryRepository.GetAll().AsNoTracking().ToList();
             await _excel.ExportExcelFile(category, "CategoryList");           
        }

        public async Task<CategoryDto> GetById(int input)
        {
            var data = await _categoryRepository
                .GetAll()
                .Where(p => p.Id == input)
                .Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Thumbnail = category.Thumbnail,
                    Description = category.Description,
                })
                .FirstOrDefaultAsync();

            return data;
        }

    }

}
