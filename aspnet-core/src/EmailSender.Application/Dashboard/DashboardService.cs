using Abp.Domain.Repositories;
using EmailSender.Authorization.Users;
using EmailSender.OrderDomain;
using EmailSender.ProductDomain;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Linq;
using EmailSender.Dashboard.DTO;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;

namespace EmailSender.Dashboard
{
    public class DashboardService : EmailSenderAppServiceBase, IDashboardService
    {
        private readonly IRepository<Order ,int> _orderRepository;
        private readonly IRepository<Product ,int> _productRepository;
        private readonly IRepository<User, long> _userRepository;


        public DashboardService(
                IRepository<Order, int> orderRepository, IRepository<Product, int> productRepository, UserManager userManager, IRepository<User, long> userRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }
        public async Task<DashboardRevenueDto> DashboardRevenue()
        {
            var data = new DashboardRevenueDto
            {
                TotalCustomer = _userRepository.GetAll().Count(),
                TotalOrder = _orderRepository.GetAll().Count(),
                TotalProducts = _productRepository.GetAll().Count(),
                TotalRevenue = _orderRepository.GetAll().Sum(order => order.TotalPrice)
            };
            return data;
        }

        public async Task<List<StatsGraphDto>> GraphStats()
        {
            var data = _orderRepository.GetAll().Select(o => new StatsGraphDto
            { 
                Creationtime = o.CreationTime,
                revenue = o.TotalPrice,
            }).ToList();

            return data;
        }
    }
}
