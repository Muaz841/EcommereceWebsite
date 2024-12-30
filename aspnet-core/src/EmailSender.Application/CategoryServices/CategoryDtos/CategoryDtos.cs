using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace EmailSender.CategoryServices.CategoryDtos
{
    public  class CategoryDto : Entity<int>
    {
        
            public string Name { get; set; }
            public string Description { get; set; }

            public decimal Sold { get; set; }

            public decimal quantity { get; set; }

            public string Thumbnail { get; set; }
            public int TenantId { get; set; }

            public DateTime Creationtime { get; set; }
        
    }

    public class PagedCategoryResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class CategoryListDto : Entity<int>
    {

        public string Name { get; set; }       

    }

}
