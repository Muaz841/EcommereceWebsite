using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using EmailSender.Authorization.Users;
using EmailSender.CartEntity;
using EmailSender.CategoryEntity;
using EmailSender.EmailSender;
using EmailSender.EmailSender.EmailSenderManager;
using EmailSender.Export;
using EmailSender.order.OrderDtos;
using EmailSender.OrderDomain;
using EmailSender.ProductDtos;
using EmailSender.ProductEntities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace EmailSender.order
{
    public class OrderServices : EmailSenderAppServiceBase, IOrderService
    {

        private readonly IRepository<Order, int> _orderRepository;
        private readonly IRepository<Cart, int> _cartRepository;
        private readonly IRepository<ProductDetail, int> _productDetailRepository;
        private readonly ExportToExcel _exportToExcel;
        private readonly IEmailSenderManager _emailSenderManager;
        private readonly IOrderService _orderService;
        private readonly IRepository<ProductCategory, int> _productCategoryRepository;
        private readonly IRepository<Category, int> _categoryRepository;
        private readonly UserManager _userManager;

        public OrderServices(IRepository<Order, int> orderRepository, IRepository<ProductDetail, int> productDetailRepository, IRepository<Cart, int> cartRepository, ExportToExcel exportToExcel, IEmailSenderManager emailSenderManager, IRepository<ProductCategory, int> productCategoryRepository, IRepository<Category, int> categoryRepository, UserManager userManager)
        {
            _orderRepository = orderRepository;
            _productDetailRepository = productDetailRepository;
            _cartRepository = cartRepository;
            _exportToExcel = exportToExcel;
            _emailSenderManager = emailSenderManager;
            _productCategoryRepository = productCategoryRepository;
            _categoryRepository = categoryRepository;
            _userManager = userManager;
        }
        public async Task PlaceOrder(OrderDto order)
        {
            var data = new Order
            {
                CustomerId = order.CustomerId,
                TotalPrice = order.TotalPrice,
                ShippingAddress = order.ShippingAddress,
                PhoneNumber = order.PhoneNumber,
                OrderProducts = order.OrderProducts.Select(op => new OrderProduct
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity,
                }).ToList()
            };
            var OrderID = await _orderRepository.InsertAndGetIdAsync(data);

            foreach (var product in order.OrderProducts)
            {
                await _productDetailRepository.GetAll().Where(p => p.ProductId == product.ProductId)
                      .UpdateFromQueryAsync(pd => new ProductDetail
                      {
                          Stock = pd.Stock - product.Quantity,
                      });

                var catId = _productCategoryRepository.GetAll().Where(p => p.ProductId == product.ProductId).Select(p => p.CategoryId).FirstOrDefault();
                await _categoryRepository.GetAll().Where(c => c.Id == catId)
                         .UpdateFromQueryAsync(cd => new Category
                         {
                             Sold = cd.Sold + 1
                         });
            }

            await _cartRepository.GetAll().Where(c => c.userID == order.CustomerId).ExecuteDeleteAsync();

            var customerData = _userManager.Users.Where(u => u.Id == order.CustomerId).FirstOrDefault();
            var email = new EmailSenderDto
            {
                OrderId = OrderID.ToString(),
                useremail = customerData.EmailAddress,
                username = customerData.Name,
                OrderStatus = "Placed",
                TotolPrice = order.TotalPrice.ToString(),
            };
            await _emailSenderManager.SendEmailAsync(email, "customerinvoice");
        }


        public async Task<PagedResultDto<OrderListDto>> OrderList(PagedOrderResultRequestDto input)
        {
            string filtertext = string.IsNullOrWhiteSpace(input.FilterText) ? string.Empty : input.FilterText.Trim().ToLower();

            var filteredOrders = _orderRepository.GetAll()
                 .AsNoTracking().Include(o => o.OrderProducts).ThenInclude(op => op.products)
                 .Where(order =>
                                    (string.IsNullOrEmpty(filtertext) || order.Customer.Name.ToLower().Contains(filtertext)) &&
                                    (!input.StartDate.HasValue || order.CreationTime >= input.StartDate.Value) &&
                                    (!input.EndDate.HasValue || order.CreationTime <= input.EndDate.Value));

            var data = await filteredOrders
            .Select(o => new OrderListDto
            {
                OrderID = o.Id,
                CreationDate = o.CreationTime,
                CustomerName = o.Customer.Name,
                TotalAmount = o.TotalPrice,
                PaymentType = "VISA",
                Status = o.Status,
                ProductsCount = o.OrderProducts.Count(),
                ProductThumbnail = o.OrderProducts
                              .OrderBy(op => op.Id)
                              .Select(op => op.products.Thumbnail)
                              .FirstOrDefault()

            }).OrderBy(input.Sorting ?? "OrderID asc")
                .PageBy(input)
                .ToListAsync();


            var totalCount = await filteredOrders.CountAsync();
            return new PagedResultDto<OrderListDto>(totalCount, data);
        }

        public async Task<OrderDetailsDto> OrderByID(int id)
        {
            var order = await _orderRepository.GetAll().AsNoTracking()
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.products)
                        .ThenInclude(p => p.ProductDetails)
                                    .Include(o => o.Customer)
                                    .Where(o => o.Id == id)
                                    .FirstOrDefaultAsync();

            var orderDetails = new OrderDetailsDto
            {
                ShippingAddress = order.ShippingAddress,
                PhoneNumber = order.PhoneNumber,
                CustomerName = order.Customer?.Name,
                CustomerMail = order.Customer?.EmailAddress,
                Customerphone = order.Customer?.PhoneNumber,
                CreationDate = order.CreationTime,
                OrderStatus = order.Status,
                OrderProducts = order.OrderProducts.Select(op =>
                {
                    var product = op.products;
                    var productDetail = product?.ProductDetails?.FirstOrDefault();

                    return new OrderProductDto
                    {
                        ProductName = product.Name,
                        ProductId = product.Id,
                        Thumbnail = product?.Thumbnail,
                        Quantity = op.Quantity,
                        productPrice = productDetail?.DiscountedPrice > 0
                            ? productDetail.DiscountedPrice
                            : productDetail?.BasePrice ?? 0,
                    };
                }).ToList()
            };

            return orderDetails;
        }

        public async Task GetExcelFile()
        {
            var data = _orderRepository.GetAll().AsNoTracking().ToList();
            _exportToExcel.ExportExcelFile(data, "OrderList");
        }

        public async Task<OrderCountDto> OrderCount()
        {
            var orders = await _orderRepository.GetAll().ToListAsync();

            var orderstats = new OrderCountDto
            {
                ProcessingOrders = orders.Count(order => order.Status == 0),
                ShippedOrders = orders.Count(order => order.Status == 1),
                DeliveredOrders = orders.Count(order => order.Status == 2),
                CancelledOrders = orders.Count(order => order.Status == 3),
            };
            return orderstats;
        }

        public async Task OrderStatusChange(int OrderID, int Status)
        {
            await _orderRepository.GetAll().Where(o => o.Id == OrderID)
                .UpdateFromQueryAsync(os => new Order
                {
                    Status = Status,
                });

            var userdata = await _orderRepository.GetAll()
                                    .Where(o => o.Id == OrderID)
                                    .Include(o => o.Customer)
                                    .FirstOrDefaultAsync();

            string orderStatusName = Enum.GetName(typeof(OrderStatus), userdata.Status);
            var email = new EmailSenderDto
            {
                OrderId = OrderID.ToString(),
                useremail = userdata.Customer.EmailAddress,
                username = userdata.Customer.UserName,
                OrderStatus = orderStatusName,
                TotolPrice = userdata.TotalPrice.ToString(),

            };
            await _emailSenderManager.SendEmailAsync(email, "customerinvoice");
        }
    }
}
