using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using ProjectSem3.Models;

namespace ProjectSem3.App_Start
{
    public class IdentityConfig
    {
    }
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var mail = new MailMessage();
            var smtpSever = new SmtpClient("smtp.gmail.com");
            smtpSever.Credentials = new NetworkCredential("ngockzb12345@gmail.com", "smnqsglqjfmvudbh");
            smtpSever.Port = 587;
            smtpSever.EnableSsl = true;
            mail.From = new MailAddress("ngockzb12345@gmail.com");
            mail.To.Add(message.Destination);
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            smtpSever.Send(mail);
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
    public class ApplicationUserManager : UserManager<AppUser>
    {
        public ApplicationUserManager(IUserStore<AppUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager =
                new ApplicationUserManager(new UserStore<AppUser>(context.Get<MyDbContext>()));
            manager.EmailService = new EmailService();
            return manager;
        }
    }
}