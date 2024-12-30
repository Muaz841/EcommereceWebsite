using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.IdentityFramework;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using DocumentFormat.OpenXml.Spreadsheet;
using EmailSender.Authorization;
using EmailSender.Authorization.Accounts;
using EmailSender.Authorization.Roles;
using EmailSender.Authorization.Users;
using EmailSender.EmailSender;
using EmailSender.EmailSender.EmailSender;
using EmailSender.EmailSender.EmailSenderManager;
using EmailSender.Export;
using EmailSender.order.OrderDtos;
using EmailSender.OrderDomain;
using EmailSender.ProductDomain;
using EmailSender.ProductEntities;
using EmailSender.Roles.Dto;
using EmailSender.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmailSender.Users
{
    [AbpAuthorize(PermissionNames.Pages_Users)]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly IEmailSenderManager _emailSenderManager;
        private readonly LogInManager _logInManager;
        private readonly IRepository<ProductDetail, int> _productDetailRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<Order, int> _orderRepository;
        private readonly ExportToExcel _exportToExcel;


        public UserAppService(
            IRepository<User, long> repository,
            IEmailSenderManager emailSenderManager,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IRepository<Order, int> orderRepository,
            IRepository<ProductDetail, int> productDetailRepository,
            IRepository<Product, int> productRepository,
            ExportToExcel exportToExcel)
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _emailSenderManager = emailSenderManager;
            _orderRepository = orderRepository;
            _productDetailRepository = productDetailRepository;
            _productRepository = productRepository;
            _exportToExcel = exportToExcel;
        }

        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;
           // user.IsActive = false;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
          var verificationUrl = $"http://localhost:4200/app/verification/verify/{user.Id}/{Uri.EscapeDataString(token)}";


            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }
                          
            CurrentUnitOfWork.SaveChanges();

            var maildata = new EmailSenderDto
            {
                useremail = user.EmailAddress,
                username = user.UserName,
                verificationurl = verificationUrl,
                userId = user.Id.ToString()
            };
           await _emailSenderManager.SendEmailAsync(maildata, "userverification");
            return MapToEntityDto(user);

        }

        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            CheckUpdatePermission();

            var user = await _userManager.GetUserByIdAsync(input.Id);

            MapToEntity(input, user);

            CheckErrors(await _userManager.UpdateAsync(user));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return await GetAsync(input);
        }

        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task Activate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = true;
            });
        }

        [AbpAuthorize(PermissionNames.Pages_Users_Activation)]
        public async Task DeActivate(EntityDto<long> user)
        {
            await Repository.UpdateAsync(user.Id, async (entity) =>
            {
                entity.IsActive = false;
            });
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);

            var orderscount =  _orderRepository.Count(o => o.CustomerId == user.Id);
            var totalBalance = _orderRepository.GetAll()
                                    .Where(o => o.CustomerId == user.Id)
                                    .Sum(o => o.TotalPrice);

            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();
            userDto.Orders = orderscount;
            userDto.balance = totalBalance;
            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }
            
            if (await _userManager.CheckPasswordAsync(user, input.CurrentPassword))
            {
                CheckErrors(await _userManager.ChangePasswordAsync(user, input.NewPassword));
            }
            else
            {
                CheckErrors(IdentityResult.Failed(new IdentityError
                {
                    Description = "Incorrect password."
                }));
            }

            return true;
        }

        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attempting to reset password.");
            }
            
            var currentUser = await _userManager.GetUserByIdAsync(_abpSession.GetUserId());
            var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            }
            
            if (currentUser.IsDeleted || !currentUser.IsActive)
            {
                return false;
            }
            
            var roles = await _userManager.GetRolesAsync(currentUser);
            if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            {
                throw new UserFriendlyException("Only administrators may reset passwords.");
            }

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            return true;
        }

        public async Task<UserDetailsDto> UserDetails(long UserId)
        {            
            var user =  _userManager.GetUserById(UserId);

            var ordersData = _orderRepository.GetAll().AsNoTracking().
                Include(o => o.OrderProducts).Where(o => o.CustomerId == UserId).ToListAsync();                        

            var orderdetails = new UserDetailsDto
            {
                userId = user.Id,
                Username = user.UserName,
                Useremail = user.EmailAddress,
                UserPhone = user.PhoneNumber,
                UserThumbnail = user.Thumbnail,
                IsActive = user.IsActive,              
                processingOrders =(await ordersData).Count(o => o.Status == 0),
                cancelledOrders = (await ordersData).Count(o => o.Status == 3),

                Orders = (await ordersData).Select(order => new OrderDto
                {
                    TotalPrice = order.TotalPrice,
                    OrderId = order.Id,
                    ShippingAddress = order.ShippingAddress,
                    CreationDate = order.CreationTime,
                    Status = order.Status,
                    OrderProducts = order.OrderProducts.Select(op =>
                    {
                        var product = op.products; 
                        var productDetail = product?.ProductDetails?.FirstOrDefault(); 

                        return new OrderProductDto
                        {
                            ProductName = product?.Name,
                            ProductId = product?.Id ?? 0,
                            Thumbnail = product?.Thumbnail,
                            Quantity = op.Quantity,
                            productPrice = productDetail?.DiscountedPrice > 0
                                ? productDetail.DiscountedPrice
                                : productDetail?.BasePrice ?? 0,
                        };
                    }).ToList()
                }).ToList()
            };

            return orderdetails;

        }
       
    }
}

