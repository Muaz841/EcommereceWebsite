using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using EmailSender.CategoryEntity;
using EmailSender.ProductDomain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.ProductDtos
{
    public class ProductDto : EntityDto<int>
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int Stock { get; set; }
        public decimal BasePrice { get; set; }      
        public bool IsActive => Stock > 0;
        public bool LowStock => Stock > 0 && Stock < 5;
        public DateTime CreationTime { get; set; }
        public byte[] Thumbnail { get; set; }
    }

    public class CreateUpdateProductDto :  IEntityDto<int>
    {
       
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public string? Barcode { get; set; }

        public int? Quantity { get; set; }

        
        public decimal BasePrice { get; set; }

        [Range(0, 100)]
        public decimal? DiscountPercentage { get; set; }

        public int? DiscountId { get; set; }

        [Required]
        public int? CategoryId { get; set; }
        
        public Category? Category { get; set; }
        public string? status { get; set; }
            
        public byte[]? Thumbnail { get; set; }

        public virtual List<ProductMediaDTO>? Images { get; set; }

        public virtual List<ProductReviewsDto>? Reviews { get; set; }

        public int Id { get; set; }

        public bool main { get; set; }

        public decimal? DiscountPrice {  get; set; }
                        
    }

    public class PagedProductResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }
        public DateTime? StartDate { get; set; } 
        public DateTime? EndDate { get; set; }   
    }

    public class ProductMediaDTO 
    {       
        public byte[] image { get; set; }

        [Required]
        public int ProductId { get; set; }
              
        public string imagename { get; set; }
        public int? TenantId { get; set; }
    }
    public class DeleteProductDto
    {
       public int Id { get; set; }
    }

    public class ProductReviewsDto
    {
        public string Reviews { get; set; }

        public string ReviewerName { get; set; }
        public int Ratings { get; set; }

        public DateTime CreationDate { get; set; }

        public long UserId { get; set; }

        public int ProductId { get; set; }
    }
}
