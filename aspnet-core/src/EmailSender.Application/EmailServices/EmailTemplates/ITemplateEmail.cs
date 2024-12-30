using Abp.Application.Services.Dto;
using EmailSender.EmailSender.EmailTempalateManagers.EmailDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailSender.EmailServices.EmailTemplates
{
    public interface ITemplateEmailService
    {

        Task<PagedResultDto<EmailTemplateDto>> GetTemplate(EmailTemplatepagedDto input);

        Task CreateOrEditTemplate(EmailTemplateDto templateDto);

        Task DeleteTemplate(int id);
    }
}
