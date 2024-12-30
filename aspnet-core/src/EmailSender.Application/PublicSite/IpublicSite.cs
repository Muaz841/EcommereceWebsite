
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
        Task<List<CreateUpdateProductDto>> GetProducts();

        Task<List<PublicProductDto>> Getlatest();

        Task<CreateUpdateProductDto> GetById(int input);

        Task AddToCart(CartDto input);

        Task<List<CartDto>> GetCart(long userID);

        Task RemoveFromCart(long userID, int productId);

        Task<ContactDetailsDto> contactDetails(long userId);
        Task AddRatings(ProductReviewsDto input);

        Task<List<ProductReviewsDto>> GetReviews();

    }
}
