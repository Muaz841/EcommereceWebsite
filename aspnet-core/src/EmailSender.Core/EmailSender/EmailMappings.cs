using Abp.AutoMapper;

using Abp.Modules;
using EmailSender.EmailSender.EmailSenderEntities;
using EmailSender.EmailSender.EmailTempalateManagers.EmailDto;
using EmailSender.EmailSender.QueueEmail.QueueEmailDto;

namespace EmailSender.EmailSender
{

    [DependsOn(typeof(EmailSenderCoreModule), typeof(AbpAutoMapperModule))]
    public class EmailMappings : AbpModule
    {
        public override void PreInitialize()
        {
            base.Initialize();

            Configuration.Modules.AbpAutoMapper().Configurators.Add(config =>
            {
                //EmailTemplateDto -> EmailTemplate
                config.CreateMap<EmailTemplateDto, EmailTemplate>()
                .ForMember(e => e.TenantId, options => options.MapFrom(src => src.TenantId))
                .ForMember(e => e.Name, options => options.MapFrom(src => src.Name))
                .ForMember(e => e.Subject, options => options.MapFrom(src => src.Subject))
                .ForMember(e => e.Content, options => options.MapFrom(src => src.Content))
                .ForMember(e => e.Bcc, options => options.MapFrom(src => src.Bcc))
                .ForMember(e => e.IsActive, options => options.MapFrom(src => src.IsActive))
                .ForMember(e => e.Token, options => options.MapFrom(src => src.Token))
                .ForMember(e => e.Cc, options => options.MapFrom(src => src.Cc));

                //EmailTemplate <- EmailTemplateDto
                config.CreateMap<EmailTemplate, EmailTemplateDto>()
                .ForMember(e => e.TenantId, options => options.MapFrom(src => src.TenantId))
                .ForMember(e => e.Name, options => options.MapFrom(src => src.Name))
                .ForMember(e => e.Subject, options => options.MapFrom(src => src.Subject))
                .ForMember(e => e.Content, options => options.MapFrom(src => src.Content))
                .ForMember(e => e.Bcc, options => options.MapFrom(src => src.Bcc))
                .ForMember(e => e.IsActive, options => options.MapFrom(src => src.IsActive))
                .ForMember(e => e.Token, options => options.MapFrom(src => src.Token))
                .ForMember(e => e.Cc, options => options.MapFrom(src => src.Cc));

                //QueuedEmailDTO -> QueuedEmail
                config.CreateMap<QueuedEmailDto, EmailQueue>()
              .ForMember(e => e.TenantId, options => options.MapFrom(src => src.TenantId))
              .ForMember(e => e.EmailPriority, options => options.MapFrom(src => src.EmailPriority))
              .ForMember(e => e.From, options => options.MapFrom(src => src.From))
              .ForMember(e => e.FromName, options => options.MapFrom(src => src.FromName))
              .ForMember(e => e.To, options => options.MapFrom(src => src.To))
              .ForMember(e => e.ToName, options => options.MapFrom(src => src.ToName))
              .ForMember(e => e.ReplyTo, options => options.MapFrom(src => src.ReplyTo))
              .ForMember(e => e.ReplyToName, options => options.MapFrom(src => src.ReplyToName))
              .ForMember(e => e.Cc, options => options.MapFrom(src => src.Cc))
              .ForMember(e => e.Bcc, options => options.MapFrom(src => src.Bcc))
              .ForMember(e => e.Subject, options => options.MapFrom(src => src.Subject))
              .ForMember(e => e.Body, options => options.MapFrom(src => src.Body))
              .ForMember(e => e.Status, options => options.MapFrom(src => src.Status))
              .ForMember(e => e.RetryCount, options => options.MapFrom(src => src.RetryCount));

                //
                //QueuedEmail -> QueuedEmailDTO
                config.CreateMap<EmailQueue, QueuedEmailDto>()
              .ForMember(e => e.TenantId, options => options.MapFrom(src => src.TenantId))
              .ForMember(e => e.EmailPriority, options => options.MapFrom(src => src.EmailPriority))
              .ForMember(e => e.From, options => options.MapFrom(src => src.From))
              .ForMember(e => e.FromName, options => options.MapFrom(src => src.FromName))
              .ForMember(e => e.To, options => options.MapFrom(src => src.To))
              .ForMember(e => e.ToName, options => options.MapFrom(src => src.ToName))
              .ForMember(e => e.ReplyTo, options => options.MapFrom(src => src.ReplyTo))
              .ForMember(e => e.ReplyToName, options => options.MapFrom(src => src.ReplyToName))
              .ForMember(e => e.Cc, options => options.MapFrom(src => src.Cc))
              .ForMember(e => e.Bcc, options => options.MapFrom(src => src.Bcc))
              .ForMember(e => e.Subject, options => options.MapFrom(src => src.Subject))
              .ForMember(e => e.Body, options => options.MapFrom(src => src.Body))
              .ForMember(e => e.Status, options => options.MapFrom(src => src.Status))
              .ForMember(e => e.RetryCount, options => options.MapFrom(src => src.RetryCount));
            });
        }

    }
}


