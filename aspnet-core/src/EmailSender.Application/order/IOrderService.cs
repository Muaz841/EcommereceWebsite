using Abp.Application.Services.Dto;
using EmailSender.order.OrderDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.order
{
    public interface IOrderService
    {
        Task PlaceOrder(OrderDto order);
        Task<PagedResultDto<OrderListDto>> OrderList(PagedOrderResultRequestDto input);

        Task<OrderDetailsDto> OrderByID(int id);
    }
}
