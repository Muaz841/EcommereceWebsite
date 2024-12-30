using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.ObjectMapping;
using EmailSender.EmailSender.EmailSenderEntities;
using EmailSender.EmailSender.EmailTempalateManagers.EmailDto;
using EmailSender.EmailSender.QueueEmail.QueueEmailDto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.EmailSender.EmailTempalateManagers
{
    public class EmailTemplateManager : ITransientDependency, IEmailTemplateManager
    {
        private readonly IRepository<EmailTemplate, int> templateRepository;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public EmailTemplateManager(IUnitOfWorkManager unitOfWorkManager, IRepository<EmailTemplate, int> emailTemplateRepository, IObjectMapper objectMapper)
        {
            templateRepository = emailTemplateRepository;
            this._objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<EmailTemplateDto> CreateTemplateAsync(EmailTemplateDto templateDto)
        {
            var template = _objectMapper.Map<EmailTemplate>(templateDto);
            template.TenantId = templateDto.TenantId;
            await templateRepository.InsertAsync(template);
            var savedTemplateDto = _objectMapper.Map<EmailTemplateDto>(template);
            return savedTemplateDto;

        }


        public Task DeleteTemplateAsync(int id)
        {
            templateRepository.DeleteAsync(id);
            return Task.CompletedTask;
        }

        public async Task<PagedResultDto<EmailTemplateDto>> GetAllTemplatesAsync(EmailTemplatepagedDto input)
        {
            var query = CreateFilteredQuery(input);
            var totalCount = await query.CountAsync();

            
            var pagedEmails = await query
                .OrderBy(p => p.Id)
                .PageBy(input)
                .AsNoTracking()
                .ToListAsync();
            var queuedDto = _objectMapper.Map<List<EmailTemplateDto>>(pagedEmails);
            return new PagedResultDto<EmailTemplateDto>(totalCount, queuedDto);
        }

        public async Task<EmailTemplateDto> GetTemplateByIdAsync(int tenantId, string templatename)
        {
            templatename = templatename?.ToLower();
            var template = await templateRepository.FirstOrDefaultAsync(t => t.TenantId == tenantId && t.Name.ToLower() == templatename);
            return _objectMapper.Map<EmailTemplateDto>(template);
        }

        public async Task UpdateTemplateAsync(EmailTemplateDto updatetemplate)
        {
            var template = await templateRepository.GetAsync(updatetemplate.Id);
            _objectMapper.Map(updatetemplate, template);
        }

        public async Task<PagedResultDto<EmailTemplateDto>> HostGetAllTemplatesAsync(EmailTemplatepagedDto input)
        {

            var query = DisabledDeletedQuery(input);
            var totalCount = await query.CountAsync();

           
            var pagedEmails = await query
                .OrderBy(p => p.Id)
                .PageBy(input)
                .AsNoTracking()
                .ToListAsync();
            var queuedDto = _objectMapper.Map<List<EmailTemplateDto>>(pagedEmails);
            return new PagedResultDto<EmailTemplateDto>(totalCount, queuedDto);
        }

        protected IQueryable<EmailTemplate> CreateFilteredQuery(EmailTemplatepagedDto input)
        {
            return Abp.Linq.Extensions.QueryableExtensions.WhereIf(
                    templateRepository.GetAll(),
                     !string.IsNullOrWhiteSpace(input.Keyword),
                         queue => queue.Subject.Contains(input.Keyword)
              );
        }

        protected IQueryable<EmailTemplate> DisabledDeletedQuery(EmailTemplatepagedDto input)
        {
            _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete);

            {
                return Abp.Linq.Extensions.QueryableExtensions.WhereIf(
                    templateRepository.GetAll(),
                     !string.IsNullOrWhiteSpace(input.Keyword),
                         queue => queue.Subject.Contains(input.Keyword)
              );

            }
        }
    }
}
