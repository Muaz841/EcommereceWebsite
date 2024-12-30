using Abp.Application.Services.Dto;
using Abp.Runtime.Session;
using EmailSender.EmailSender.EmailTempalateManagers;
using EmailSender.EmailSender.EmailTempalateManagers.EmailDto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSender.EmailServices.EmailTemplates
{
    public class TemplateEmailService : EmailSenderAppServiceBase, ITemplateEmailService
    {
        private readonly IEmailTemplateManager _emailtemplate;
        private readonly IAbpSession _abpSession;
        public TemplateEmailService(IEmailTemplateManager emailtemplate, IAbpSession abpsession)
        {
            _emailtemplate = emailtemplate;
            _abpSession = abpsession;
        }

       
            public async Task<PagedResultDto<EmailTemplateDto>> GetTemplate(EmailTemplatepagedDto input)
            {
                return await _emailtemplate.GetAllTemplatesAsync(input);
            }

        public async Task<PagedResultDto<EmailTemplateDto>> GetHostTemplate(EmailTemplatepagedDto input)
        {
                return await _emailtemplate.HostGetAllTemplatesAsync(input);
         }

        public async Task CreateOrEditTemplate(EmailTemplateDto templateDto)
        {
            
            templateDto.TenantId = _abpSession.TenantId ?? 1;
            await(templateDto.Id <= 0
                ? _emailtemplate.CreateTemplateAsync(templateDto)
                : _emailtemplate.UpdateTemplateAsync(templateDto));
        }

        public Task DeleteTemplate(int id)
        {
            return _emailtemplate.DeleteTemplateAsync(id);

        }
       
    }
}
