using Abp.AutoMapper;

using Abp.Modules;
using EmailSender.CategoryEntity;
using EmailSender.CategoryServices.CategoryDtos;
using EmailSender.EmailSender.EmailSenderEntities;
using EmailSender.EmailSender.EmailTempalateManagers.EmailDto;
using EmailSender.EmailSender.QueueEmail.QueueEmailDto;
using EmailSender.ProductDomain;

namespace EmailSender.EmailSender
{

    [DependsOn(typeof(EmailSenderCoreModule), typeof(AbpAutoMapperModule))]
    public class CategoryMappings : AbpModule
    {
        public override void PreInitialize()
        {
            base.Initialize();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(config =>
            {
                config.CreateMap<CategoryDto, Category>()
                    .ForMember(c => c.TenantId, options => options.MapFrom(src => src.TenantId))
                    .ForMember(c => c.Name, options => options.MapFrom(src => src.Name))
                    .ForMember(c => c.Description, options => options.MapFrom(src => src.Description))
                    .ForMember(c => c.Sold, options => options.MapFrom(src => src.Sold))
                    .ForMember(c => c.quantity, options => options.MapFrom(src => src.quantity))
                    .ForMember(c => c.Thumbnail, options => options.MapFrom(src => src.Thumbnail))
                    .ForMember(c => c.Id, options => options.MapFrom(src => src.Id));



                config.CreateMap<Category, CategoryDto>()
                        .ForMember(c => c.TenantId, options => options.MapFrom(src => src.TenantId))
                        .ForMember(c => c.Name, options => options.MapFrom(src => src.Name))
                        .ForMember(c => c.Description, options => options.MapFrom(src => src.Description))
                        .ForMember(c => c.Sold, options => options.MapFrom(src => src.Sold))
                        .ForMember(c => c.quantity, options => options.MapFrom(src => src.quantity))
                        .ForMember(c => c.Thumbnail, options => options.MapFrom(src => src.Thumbnail))
                        .ForMember(c => c.Id, options => options.MapFrom(src => src.Id));


            });
        }

    }
}


