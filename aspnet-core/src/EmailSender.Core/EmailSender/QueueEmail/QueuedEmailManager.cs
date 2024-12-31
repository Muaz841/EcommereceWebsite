using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using EmailSender.EmailSender.EmailSenderEntities;
using EmailSender.EmailSender.QueueEmail.QueueEmailDto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailSender.EmailSender.QueueEmail
{
    public class QueuedEmailManager : ITransientDependency, IQueuedEmailManager
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<EmailQueue, int> _queuedRepository;
        private readonly IObjectMapper _objectMapper;

        public QueuedEmailManager(IUnitOfWorkManager unitOfWorkManager, IRepository<EmailQueue, int> queuedRepository, IObjectMapper objectMapper)
        {
            _queuedRepository = queuedRepository;
            _objectMapper = objectMapper;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<List<QueuedEmailDto>> GetPendingEmailsAsync()
        {
            var queued = await _queuedRepository.GetAllListAsync(q => q.Status != "Sent" && q.RetryCount < 6);
            return _objectMapper.Map<List<QueuedEmailDto>>(queued);
        }

        public async Task IncrementRetryCountAsync(int emailId)
        {
            var email = await _queuedRepository.GetAsync(emailId);
            email.RetryCount++;
            if (email.RetryCount > 4)
            {
                email.Status = "Failed";
            }
                       
            using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                await _queuedRepository.UpdateAsync(email);
                await unitOfWork.CompleteAsync();
            }
        }

        public async Task AddQueueEmailAsync(QueuedEmailDto email)
        {
            var entry = _objectMapper.Map<EmailQueue>(email);
            await _queuedRepository.InsertAsync(entry);
        }

        public async Task UpdateEmailStatusAsync(int emailId, string status)
        {
            var email = await _queuedRepository.GetAsync(emailId);
            email.Status = status;

            using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                await _queuedRepository.UpdateAsync(email);
                await unitOfWork.CompleteAsync();
            }
        }
        public async Task<PagedResultDto<QueuedEmailDto>> GetAllEmailsInQueueAsync(QueuePagedDto input)
        {
            var query =  CreateFilteredQueryAsync(input);
            var totalCount = await query.CountAsync();

            // Apply sorting and pagination
            var pagedEmails = await query
                      .OrderBy(p => p.Id)
                      .PageBy(input.SkipCount, input.MaxResultCount)
                      .AsNoTracking()
                        .ToListAsync();


            var queuedDto = _objectMapper.Map<List<QueuedEmailDto>>(pagedEmails);
            return new PagedResultDto<QueuedEmailDto>(totalCount, queuedDto);
        }

        protected IQueryable<EmailQueue> CreateFilteredQueryAsync(QueuePagedDto input)
        {

            return _queuedRepository.GetAll()
                     .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword),
                         queue => queue.To.Contains(input.Keyword) ||
                     queue.From.Contains(input.Keyword) ||
                     queue.Subject.Contains(input.Keyword));
                                     
        }
        public async Task<QueuedEmailDto> GetQueueMailById(int emailId)
        {
            var email = await _queuedRepository.GetAsync(emailId);
            return _objectMapper.Map<QueuedEmailDto>(email);
        }

        public async Task<List<QueuedEmailDto>> GetPendingEmailsTWAsync(int tenantid)
        {
            using (var unitOfWork = _unitOfWorkManager.Current.SetTenantId(1)) {
                var queued = await _queuedRepository.GetAllListAsync(q => q.Status != "Sent" && q.RetryCount < 6);
                return _objectMapper.Map<List<QueuedEmailDto>>(queued);
            }
            
        }

        public async Task UpdateFailedMails(int emailId)
        {
            var email = await _queuedRepository.GetAsync(emailId);
            email.RetryCount = 0; 
            email.Status = "pending";

            using (var unitOfWork = _unitOfWorkManager.Begin(System.Transactions.TransactionScopeOption.RequiresNew))
            {
                await _queuedRepository.UpdateAsync(email);
                await unitOfWork.CompleteAsync();
            }
        }
    }
}
