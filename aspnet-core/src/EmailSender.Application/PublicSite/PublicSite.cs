using Abp.Domain.Repositories;
using EmailSender.ProductDtos;
using EmailSender.ProductDomain;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using EmailSender.PublicSite.publicDtos;
using Abp.UI;
using Z.EntityFramework.Plus;
using EmailSender.CartEntity;
using EmailSender.ProductEntities;
using EmailSender.Authorization.Users;
using Castle.MicroKernel;
using System;
using EmailSender.OrderDomain;

namespace EmailSender.PublicSite
{
    public class PublicSiteAppService : EmailSenderAppServiceBase, IPublicSiteAppService
    {
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<ProductMedia, int> _mediaRepository;
        private readonly IRepository<ProductCategory, int> _ProductCategoryRepository;
        private readonly IRepository<ProductDetail, int> _productDetailRepository;
        private readonly IRepository<DiscountType, int> _discountRepository;
        private readonly IRepository<Cart, int> _CartRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<ProductReview, int> _productReviewRepository;
        private readonly IRepository<Order, int> _orderRepository;

        public PublicSiteAppService(IRepository<Product, int> productRepository, IRepository<ProductMedia, int> mediaRepository,
           IRepository<ProductCategory, int> ProductCategoryRepository,
           IRepository<ProductDetail, int> productDetailRepository, IRepository<ProductReview, int> productReviewRepository, IRepository<Cart, int> CartRepository, IRepository<DiscountType, int> discountRepository, IRepository<User, long> userRepository, IRepository<Order, int> orderRepository)
        {
            _productRepository = productRepository;
            _mediaRepository = mediaRepository;
            _ProductCategoryRepository = ProductCategoryRepository;
            _productDetailRepository = productDetailRepository;
            _discountRepository = discountRepository;
            _CartRepository = CartRepository;
            _userRepository = userRepository;
            _productReviewRepository = productReviewRepository;
            _orderRepository = orderRepository;
        }


        public async Task<List<PublicProductDto>> GetAllProducts()
        {

            var filteredProducts = _productRepository.GetAll()
                   .AsNoTracking()
                   .Include(product => product.ProductDetails)
                   .Include(p => p.ProductReviews)
                   .Include(product => product.ProductCategories)
                     .ThenInclude(pc => pc.Category);

            var data = await filteredProducts
                .Where(x => x.ProductDetails.Any(pd => pd.Stock > 0))
                .OrderByDescending(product => product.CreationTime)
                .Select(x => new PublicProductDto
                {
                    productID = x.Id,
                    description = x.ProductDetails.FirstOrDefault().Description,
                    Thumbnail = x.Thumbnail,
                    BasePrice = x.ProductDetails.FirstOrDefault().DiscountedPrice > 0
                                ? x.ProductDetails.FirstOrDefault().DiscountedPrice
                                : x.ProductDetails.FirstOrDefault().BasePrice,
                    Rating = x.ProductReviews.Any()
                                ? (int)x.ProductReviews.Average(r => r.ratings)
                                : 0
                })
                .ToListAsync();

            return data;
        }

        public async Task<List<PublicProductDto>> NewArrivals()
        {
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);

            var filteredProducts = _productRepository.GetAll()
                   .AsNoTracking()
                   .Include(product => product.ProductDetails)
                   .Include(p => p.ProductReviews)
                   .Include(product => product.ProductCategories)
                     .ThenInclude(pc => pc.Category);

            var data = await filteredProducts
                .Where(x => x.ProductDetails.Any(pd => pd.Stock > 0) && x.CreationTime >= thirtyDaysAgo)
                .OrderByDescending(product => product.CreationTime)
                .Select(x => new PublicProductDto
                {
                    productID = x.Id,
                    description = x.ProductDetails.FirstOrDefault().Description,
                    Thumbnail = x.Thumbnail,
                    BasePrice = x.ProductDetails.FirstOrDefault().DiscountedPrice > 0
                                ? x.ProductDetails.FirstOrDefault().DiscountedPrice
                                : x.ProductDetails.FirstOrDefault().BasePrice,
                    Rating = x.ProductReviews.Any()
                                ? (int)x.ProductReviews.Average(r => r.ratings)
                                : 0
                })
                .ToListAsync();

            return data;
        }
        public async Task<List<PublicProductDto>> GetLatest()
        {

            var filteredProducts = _productRepository.GetAll()
                   .AsNoTracking()
                   .Include(product => product.ProductDetails)
                   .Include(p => p.ProductReviews)
                   .Include(product => product.ProductCategories)
                     .ThenInclude(pc => pc.Category);

            var data = await filteredProducts
                .Where(x => x.ProductDetails.Any(pd => pd.Stock > 0))
                .OrderByDescending(product => product.CreationTime)
                .Take(4)
                .Select(x => new PublicProductDto
                {
                    productID = x.Id,
                    description = x.ProductDetails.FirstOrDefault().Description,
                    Thumbnail = x.Thumbnail,
                    BasePrice = x.ProductDetails.FirstOrDefault().DiscountedPrice > 0
                                ? x.ProductDetails.FirstOrDefault().DiscountedPrice
                                : x.ProductDetails.FirstOrDefault().BasePrice,
                    Rating = x.ProductReviews.Any()
                                ? (int)x.ProductReviews.Average(r => r.ratings)
                                : 0
                })
                .ToListAsync();

            return data;
        }

        public async Task<PublicProductDto> GetMainProduct()
        {
            var filteredProducts = _productRepository.GetAll()
                   .AsNoTracking()
                   .Include(product => product.ProductDetails)
                   .Include(product => product.ProductCategories)
                     .ThenInclude(pc => pc.Category)
                     .Where(p => p.main == true && p.ProductDetails.Any(pd => pd.Stock > 0));

            var data = await filteredProducts
                .Select(x => new PublicProductDto
                {
                    productID = x.Id,
                    description = x.ProductDetails.FirstOrDefault().Description,
                    Thumbnail = x.Thumbnail,
                    BasePrice = x.ProductDetails.FirstOrDefault().DiscountedPrice > 0
                                ? x.ProductDetails.FirstOrDefault().DiscountedPrice
                                : x.ProductDetails.FirstOrDefault().BasePrice,
                }).FirstOrDefaultAsync();




            return data;

        }

        public async Task<CreateUpdateProductDto> GetById(int input)
        {
            var product = await _productRepository.GetAll()
                          .AsNoTracking()
                          .Include(p => p.ProductDetails)
                          .Include(p => p.ProductCategories)
                          .Include(p => p.ProductMedia).Include(p => p.ProductReviews).ThenInclude(r => r.User)
                          .Where(p => p.Id == input)
                          .FirstOrDefaultAsync();

            var productDto = new CreateUpdateProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Thumbnail = product.Thumbnail,
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
                }).ToList(),
                Reviews = product.ProductReviews.Select(r => new ProductReviewsDto
                {
                    ReviewerName = r.User.Name,
                    Ratings = r.ratings,
                    Reviews = r.review,
                    CreationDate = r.CreationTime,
                }).ToList(),


            };
            return productDto;
        }


        public async Task AddToCart(CartDto input)
        {
            var updateResult = await _CartRepository.GetAll().Where(p => p.ProductId == input.productID && p.userID == input.userID)
                        .ExecuteUpdateAsync(set => set.SetProperty(c => c.Quantity, c => c.Quantity + input.quantity));

            if (updateResult == 0)
            {
                var data = new Cart
                {
                    userID = input.userID,
                    ProductId = input.productID,
                    Quantity = input.quantity,
                };
                await _CartRepository.InsertAsync(data);
            }
        }

        public async Task<int> CartCount(long UserID)
        {
            var cartcount = _CartRepository.Count(c => c.userID == UserID);
            return cartcount;
        }


        public async Task<List<CartDto>> GetCart(long userID)
        {
            var items = await _CartRepository.GetAll()
                .AsNoTracking()
                .Where(p => p.userID == userID)
                .Include(c => c.Product)
                .ThenInclude(p => p.ProductDetails)
                .ToListAsync();

            var data = items
                .Select(x => new CartDto
                {
                    CartID = x.Id,
                    userID = x.userID,
                    productID = x.ProductId,
                    quantity = x.Quantity,
                    products = new ProductDto
                    {
                        Id = x.ProductId,
                        Name = x.Product.Name,
                        Thumbnail = x.Product.Thumbnail,
                        BasePrice = x.Product.ProductDetails.FirstOrDefault().BasePrice,                       
                        Stock = x.Product.ProductDetails.FirstOrDefault().Stock,

                    }
                })
                .ToList();

            return data;
        }

        public async Task ProductQuantityControl(CartQuantityDto input)
        {
            _CartRepository.GetAll().Where(c => c.Id == input.cartId).UpdateFromQuery(pc => new Cart
            {
                Quantity = pc.Quantity + input.quantitychange,
            });
        }
        public async Task RemoveFromCart(long userID, int productId)
        {
            await _CartRepository.GetAll().Where(p => p.userID == userID && p.ProductId == productId).ExecuteDeleteAsync();
        }

        public async Task<ContactDetailsDto> contactDetails(long userId)
        {
            var data = await _userRepository.GetAll().Where(p => p.Id == userId)

                .Select(p => new ContactDetailsDto
                {
                    email = p.EmailAddress,
                    phoneNumber = p.PhoneNumber,
                    firstName = p.UserName,
                    lastName = p.Surname
                }).FirstOrDefaultAsync();

            return data;
        }

        public async Task AddRatings(List<ProductReviewsDto> input)
        {
            foreach (var review in input)
            {
                var data = new ProductReview
                {
                    UserId = review.UserId,
                    ProductId = review.ProductId,
                    ratings = review.Ratings,
                    review = review.Reviews,
                };
                await _productReviewRepository.InsertAsync(data);
            }
        }


        public async Task<List<ProductReviewsDto>> GetReviews()
        {
            return await _productReviewRepository.GetAll()
                .OrderByDescending(r => r.CreationTime)
                .Take(4)
                .Select(r => new ProductReviewsDto
                {
                    Reviews = r.review,
                    ReviewerName = r.User.Name,
                    Ratings = r.ratings,
                })
                .ToListAsync();
        }

        public async Task<List<ReviewProductsDto>> GetProductForReview(int orderId)
        {
            var products = await _orderRepository.GetAll()
                .Where(o => o.Id == orderId)
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.products)
                .SelectMany(o => o.OrderProducts)
                .Select(op => new ReviewProductsDto
                {
                    ProductId = op.products.Id,
                    ProductName = op.products.Name,
                    ProductThumbnail = op.products.Thumbnail
                })
                .ToListAsync();

            return products;
        }

    }
}
