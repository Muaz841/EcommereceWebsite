using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.order.OrderDtos
{
    public  class OrderListDto
    {
        public int OrderID { get; set; }
        
        public DateTime CreationDate { get; set; }

        public string CustomerName { get; set; }

        public decimal TotalAmount { get; set; }

        public string PaymentType { get; set; }

        public int Status { get; set; }
        public int ProcessingCount {  get; set; }
        public int ShippedCount { get; set; }
        public int DeliveredCount { get; set; }
        public int CancelledCount { get; set; }

        public byte[] ProductThumbnail { get; set; }

        public int ProductsCount { get; set; }

    }

    public class PagedOrderResultRequestDto : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
