using Abp.Domain.Entities;
using System;
using System.Collections.Generic;


namespace EmailSender.order.OrderDtos
{
    public class OrderDto
    {        
        public long CustomerId { get; set; }
       
        public decimal TotalPrice { get; set; }

        public string PaymentMethod { get; set; }
        
        public virtual List<OrderProductDto> OrderProducts { get; set; }

        public string ShippingAddress { get; set; }

        public string PhoneNumber { get; set; }

        public int OrderId { get; set; }

        public DateTime CreationDate { get; set; }

        public int Status { get; set; }

        public byte[] ProductThumbnail { get; set; }

        public int ProductsCount { get; set; }
    }
    public class OrderProductDto : Entity<int>
    {
        public byte[] Thumbnail { get; set; }

        public int ProductId { get; set; }   
        public int Quantity { get; set; }

        public decimal productPrice { get; set; }

        public string ProductName {  get; set; }
        
    }

}
