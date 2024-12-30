using Abp.Application.Services.Dto;
using EmailSender.EmailSender.EmailSenderEntities;
using EmailSender.EmailSender.EmailTempalateManagers.EmailDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EmailSender.EmailTempalateManagers
{
    public interface IEmailTemplateManager
    {
        Task<EmailTemplateDto> CreateTemplateAsync(EmailTemplateDto template);
        Task<EmailTemplateDto> GetTemplateByIdAsync(int tenantId, string templatename);
        Task<PagedResultDto<EmailTemplateDto>> GetAllTemplatesAsync(EmailTemplatepagedDto input);
        Task UpdateTemplateAsync(EmailTemplateDto updatetemplate);
        Task DeleteTemplateAsync(int id);

        Task<PagedResultDto<EmailTemplateDto>> HostGetAllTemplatesAsync(EmailTemplatepagedDto input);
    }
}
