using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Arch.EntityFrameworkCore;
using EmailSender.EmailSender.EmailSenderEntities;
using EmailSender.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender.EntityFrameworkCore.Seed.Email
{
    public class EmailTemplateBuilder
    {
        private readonly EmailSenderDbContext _context;
        private readonly IAbpSession _abpSession;
        private readonly IRepository<Tenant, int> _tenantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public EmailTemplateBuilder(EmailSenderDbContext context, IAbpSession abpSession, IRepository<Tenant, int> tenantRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _context = context;
            _abpSession = abpSession;
            _tenantRepository = tenantRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task Create()
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {                   
                    var checkExists =  _context.EmailTemplates.Where(t => t.TenantId== 1).Take(1).SingleOrDefault();
                    if (checkExists != null)
                    {
                        return; 
                    }
                    else
                    {
                       
                        var usertemplate = new EmailTemplate
                        {
                            TenantId = 1, 
                            Name = "userverification",
                            Subject = "User Verification",
                            Content = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Account Verification</title>\r\n</head>\r\n<body style=\"font-family: Arial, sans-serif; background-color: #f4f4f9; margin: 0; padding: 20px;\">\r\n\r\n    <h2 style=\"text-align: center; font-size: 24px; color: #333333;\">Welcome To FYNEST</h2>\r\n\r\n    <p style=\"font-size: 16px; color: #333333;\">Dear {{username}},</p>\r\n\r\n    <p style=\"font-size: 16px; color: #333333;\">\r\n        We are excited to inform you that your account has been successfully created. To get started and verify your email address, \r\n        please click the verification button below.\r\n    </p>\r\n\r\n    <p style=\"font-size: 16px; color: #333333;\">\r\n        If you did not create an account with us, please ignore this email.\r\n    </p>\r\n\r\n    <!-- Verification Button -->\r\n    <div style=\"text-align: center; padding-top: 20px;\">\r\n        <a href=\"{{verificationurl}}\"\r\n           style=\"display: inline-block; padding: 12px 30px; font-size: 16px; color: #ffffff; background-color: #0078D4; text-decoration: none; border-radius: 4px;\">\r\n            Verify Email\r\n        </a>\r\n    </div>\r\n\r\n    <p style=\"font-size: 14px; color: #777777; text-align: center; padding-top: 20px;\">\r\n        If you have any issues or questions, please contact our support team.\r\n    </p>\r\n\r\n</body>\r\n</html>\r\n",
                            Bcc = "bcc@example.com",
                            IsActive = true,
                            Token = "{{userId}}, {{username}}, {{verficationtoken}} ",
                            Cc = "cc@Fynestowner.com"
                        };

                        await _context.EmailTemplates.AddAsync(usertemplate);
                        await _context.SaveChangesAsync();

                        var template = new EmailTemplate
                        {
                            TenantId = 1,
                            Name = "customerinvoice",
                           Subject = "Customer Invoice",
                            Content = "<p>Dear {{username}},</p><p>We would like to inform you that the current status of your order with Order ID: <strong>{{OrderID}}</strong> is: <strong>{{OrderStatus}}</strong>.</p><p>If you have any questions or concerns regarding your order, please feel free to reach out to us.</p>\r\n\r\n<p>Your feedback is important to us! We’d greatly appreciate it if you could take a moment to share your thoughts about your recent purchase. It helps us improve and serve you better.</p>\r\n\r\n\r\n   <div style=\"text-align: center; padding-top: 20px;\">\r\n       <a href=\"http://localhost:8080/app/reviewScreen/reviewScreen/{{OrderID}}\"\r\n   style=\"display: inline-block; padding: 12px 30px; font-size: 16px; color: #ffffff; background-color: #EEA520; text-decoration: none; border-radius: 4px; position: relative; z-index: 100;\">\r\n    ADD REVIEW\r\n</a>\r\n\r\n    </div>\r\n<p>Thank you for choosing us!</p><p>Best regards,</p><p><img src=\"data:image/png;base64,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\"></p><p></p>",
                            Bcc = "bcc@example.com",
                            IsActive = true,
                            Token = "{{OrderStatus}}, {{Customer}}, {{OrderID}} , {{OrderStatus}}",
                            Cc = "cc@Fynestowner.com"
                        };

                        await _context.EmailTemplates.AddAsync(template);
                        await _context.SaveChangesAsync();
                        await unitOfWork.CompleteAsync();
                    }
                   //await  unitOfWork.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                
               
            }
        }

    }
}
