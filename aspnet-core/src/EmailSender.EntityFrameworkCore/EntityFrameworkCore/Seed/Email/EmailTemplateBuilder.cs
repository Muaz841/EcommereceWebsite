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
                            Content = "<body>\n  <p>Dear {{username}},</p>\n  <p>We would like to inform you that the current status of your order with Order ID: <strong>{{OrderID}}</strong> is: <strong>{{OrderStatus}}</strong>.</p>\n  <p>If you have any questions or concerns regarding your order, please feel free to reach out to us.</p>\n  <p>Thank you for choosing us!</p>\n  <p>Best regards,<br>Your Company Name</p>\n  <p><a href=\"http://www.FYNEST.com\">www.yourcompany.com</a></p>\n</body>",
                            Bcc = "bcc@example.com",
                            IsActive = true,
                            Token = "{{OrderStatus}}, {{Customer}}, {{OrderID}} , {{OrderStatus}}",
                            Cc = "cc@Fynestowner.com"
                        };

                        await _context.EmailTemplates.AddAsync(template);
                        await _context.SaveChangesAsync();
                    }
                    await unitOfWork.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                
               
            }
        }

    }
}
