using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using EmailSender.ProductDtos;
using EmailSender.ProductDomain;
using System.Linq.Dynamic.Core;
using System.Linq;
using Abp.Linq.Extensions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Runtime.Session;
using Abp.EntityFrameworkCore.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Abp.Domain.Uow;
using EmailSender.Export;
using System;
using Abp.UI;
using EmailSender.ProductEntities;
using EmailSender.CategoryEntity;
using EFCore.BulkExtensions;
using Z.EntityFramework.Plus;


namespace EmailSender.ProductServices
{
    public class ProductAppService : EmailSenderAppServiceBase, IProductAppService
    { 
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<ProductMedia, int> _mediaRepository;
        private readonly IRepository<Category, int> _categoryRepository;
        private readonly IRepository<ProductCategory, int> _ProductCategoryRepository;
        private readonly IRepository<ProductDetail, int> _productDetailRepository;
        private readonly IRepository<DiscountType, int> _discountRepository;
        private readonly IAbpSession _abpSession;
        private readonly ExportToExcel _exportToExcel;

        public ProductAppService(IRepository<Category, int> categoryRepository, IRepository<Product, int> productRepository,
            IRepository<ProductMedia, int> mediaRepository, IRepository<ProductCategory, int> productCategoryRepository,
            IRepository<ProductDetail, int> productDetailRepository, IAbpSession abpSession, ExportToExcel exportToExcel,
            IRepository<DiscountType, int> discountRepository)
        {

            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mediaRepository = mediaRepository;
            _ProductCategoryRepository = productCategoryRepository;
            _productDetailRepository = productDetailRepository;
            _abpSession = abpSession;
            _exportToExcel = exportToExcel;
            _discountRepository = discountRepository;
        }

        public async Task CreateAsync(CreateUpdateProductDto input)
        {
            var Tenant = _abpSession.TenantId.Value;
            decimal discountedprice = 0 ;

            if (input.DiscountPercentage > 0)
            {
                var discounted = Math.Round((input.DiscountPercentage.Value / 100) * input.BasePrice, 2);
                 discountedprice = input.BasePrice - discounted;
            }

            var product = new Product
            {
                Name = input.Name,
                TenantId = Tenant,
                Thumbnail = input.Thumbnail,
                IsActive = input.IsActive,

                ProductDetails = new List<ProductDetail>
                 {
                    new ProductDetail
                    {
                        Stock = input.Quantity.Value,
                        BasePrice = input.BasePrice,
                        Description = input.Description,
                        DiscountedPrice = discountedprice,
                    }
                },

                ProductMedia = input.Images?.Select(img => new ProductMedia
                {
                    image = img.image,
                    Description = img.imagename,
                    TenantId = Tenant,
                }).ToList(),

                ProductCategories = new List<ProductCategory>
                      {
                        new ProductCategory
                        {
                            CategoryId = input.CategoryId.Value
                        }
                      }
                };
            var productId = await _productRepository.InsertAndGetIdAsync(product);
            
           await _categoryRepository.GetAll().Where
                          (c => c.Id == input.CategoryId.Value)
                              .UpdateFromQueryAsync(c => new Category { quantity = c.quantity + 1 });
            if (input.main)
            {

                await _productRepository.GetAll()
                    .Where(p => p.Id != productId)
                    .UpdateFromQueryAsync(p => new Product
                    {
                        main = false
                    });

                await _productRepository.GetAll()
                        .Where(p => p.Id == productId)
                        .UpdateFromQueryAsync(p => new Product
                        {
                            main = true
                        });
            }
        }

        [UnitOfWork]
        public async Task DeleteAsync([FromBody] List<int> input)
        {          
                await _ProductCategoryRepository.GetAll()
                            .Where(p => input.Contains(p.ProductId))
                            .Select(p => p.Category)
                            .UpdateFromQueryAsync(c => new Category
                            {
                                quantity = c.quantity - 1
                            });                            
                await _productRepository.GetAll().Where(p => input.Contains(p.Id)).DeleteAsync();
        }

        public async Task<PagedResultDto<ProductDto>> GetAllAsync(PagedProductResultRequestDto input)
        {
          
            string filterText = string.IsNullOrWhiteSpace(input.FilterText) ? string.Empty : input.FilterText.Trim().ToLower();

            var filteredProducts = _productRepository.GetAll()
                   .AsNoTracking()
                   .Where(product =>
                       (string.IsNullOrEmpty(filterText) || product.Name.ToLower().Contains(filterText)) &&
                       (!input.StartDate.HasValue || product.CreationTime >= input.StartDate.Value) &&
                       (!input.EndDate.HasValue || product.CreationTime <= input.EndDate.Value))
                             .Include(product => product.ProductDetails)
                                .Include(product => product.ProductCategories)
                                 .ThenInclude(pc => pc.Category);


            var data = await filteredProducts
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Thumbnail = x.Thumbnail,
                    CreationTime = x.CreationTime,
                    CategoryName = x.ProductCategories.FirstOrDefault().Category.Name, 
                    BasePrice = x.ProductDetails.FirstOrDefault().BasePrice,
                    Stock = x.ProductDetails.FirstOrDefault().Stock,
                    Ismain = x.main,
                    IsActive = x.IsActive,
                })
                .OrderBy(input.Sorting ?? "id desc")
                .PageBy(input)  
                .ToListAsync();

            var totalCount = await filteredProducts.CountAsync();

            return new PagedResultDto<ProductDto>(totalCount, data);
        }

        public async Task UpdateAsync(CreateUpdateProductDto input)
        {
            var Tenant = _abpSession.TenantId.Value;
            decimal discountedprice = 0;

            if (input.DiscountPercentage > 0)
            {
                var discounted = Math.Round((input.DiscountPercentage.Value / 100) * input.BasePrice, 2);
                discountedprice = input.BasePrice - discounted;
            }

            await _productRepository.GetAll().Where(p => p.Id == input.Id)
                                       .UpdateFromQueryAsync(p => new Product
                                       {
                                           Name = input.Name,
                                           Thumbnail = input.Thumbnail,
                                           IsActive = input.IsActive,
                                       });

            await _productDetailRepository.GetAll().Where(pd => pd.ProductId == input.Id)
                                        .UpdateFromQueryAsync(pd => new ProductDetail
                                        {
                                            Stock = input.Quantity.Value,   
                                            Description = input.Description,
                                            BasePrice = input.BasePrice,
                                            DiscountedPrice = discountedprice,
                                            DiscountId = input.DiscountId.Value
                                        });

           
                await _mediaRepository.GetAll().Where(m => m.ProductId == input.Id).ExecuteDeleteAsync();
                var media = input.Images.Select(img => new ProductMedia
                {
                    ProductId = input.Id,
                    image = img.image,
                    Description = img.imagename,
                    TenantId = Tenant
                });
                //await _mediaRepository.GetDbContext().BulkInsertAsync(media);
                await _mediaRepository.GetDbContext().AddRangeAsync(media);

            

            await _ProductCategoryRepository.GetAll().Where(pc => pc.ProductId == input.Id)
                     .UpdateFromQueryAsync(pc => new ProductCategory
                     {
                         CategoryId = input.CategoryId.Value,
                     });
         
            if (input.main)
            {

                await _productRepository.GetAll()
                    .Where(p => p.Id != input.Id)
                    .UpdateFromQueryAsync(p => new Product
                    {
                        main = false
                    });

                        await _productRepository.GetAll()
                                .Where(p => p.Id == input.Id)
                                .UpdateFromQueryAsync(p => new Product
                                {
                                    main = true
                                });
            }
        }

       
        public async Task GetExcelFile()
        {
            var data = _productRepository.GetAll().AsNoTracking().ToList();
            _exportToExcel.ExportExcelFile(data, "ProductList");            
        }

        public async Task<CreateUpdateProductDto> GetById(int? input)
        {
            var product = await _productRepository.GetAll()
                          .AsNoTracking()                          
                          .Include(p => p.ProductDetails)       
                          .Include(p => p.ProductCategories)                          
                          .Include(p => p.ProductMedia)         
                          .Where(p => p.Id == input)
                          .FirstOrDefaultAsync();            

            var productDto = new CreateUpdateProductDto
            {
                Name = product.Name,
                Thumbnail = product.Thumbnail,
                IsActive = product.IsActive,
                DiscountId = product.ProductDetails.FirstOrDefault()?.DiscountId, 
                DiscountPrice = product.ProductDetails.FirstOrDefault()?.DiscountedPrice, 
                BasePrice = product.ProductDetails.FirstOrDefault().BasePrice, 
                Quantity = product.ProductDetails.FirstOrDefault()?.Stock, 
                CategoryId = product.ProductCategories.FirstOrDefault()?.CategoryId, 
                Description = product.ProductDetails.FirstOrDefault()?.Description,
                Images = product.ProductMedia.Select(m => new ProductMediaDTO
                {
                    image = m.image,
                    imagename = m.Description
                }).ToList()
            };
        

            return productDto;
        }

        public async Task<List<DiscountType>> GetDiscountTypes()
        {
             var data = await _discountRepository.GetAll().ToListAsync();
            return data;
        }

    }
}
