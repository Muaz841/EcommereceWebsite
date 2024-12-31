
using Abp.Application.Services.Dto;
using EmailSender.ProductDomain;
using EmailSender.ProductDtos;
using EmailSender.PublicSite.publicDtos;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace EmailSender.PublicSite
{
    public interface IPublicSiteAppService
    {
        Task<List<PublicProductDto>> GetLatest();
        Task<List<PublicProductDto>> GetAllProducts();
        Task<PublicProductDto> GetMainProduct();
        Task<CreateUpdateProductDto> GetById(int input);
        Task AddToCart(CartDto input);
        Task<List<ReviewProductsDto>> GetProductForReview(int orderId);
        Task<List<PublicProductDto>> NewArrivals();
        Task<int> CartCount(long userID);
        Task<List<CartDto>> GetCart(long userID);
        Task ProductQuantityControl(CartQuantityDto input);

        Task AddRatings(List<ProductReviewsDto> input);
        Task RemoveFromCart(long userID, int productId);
        Task<ContactDetailsDto> contactDetails(long userId);        
        Task<List<ProductReviewsDto>> GetReviews();

    }
}
