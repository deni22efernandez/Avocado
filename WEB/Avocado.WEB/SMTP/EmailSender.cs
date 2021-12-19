using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avocado.WEB.SMTP
{
	public class EmailSender : IEmailSender
	{
		private readonly EmailSenderConfig _config;
		public EmailSender(IOptions<EmailSenderConfig> config)
		{
			_config = config.Value;
		}
		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var emailToSend = new MimeMessage();
			emailToSend.From.Add(MailboxAddress.Parse(_config.From));
			emailToSend.To.Add(MailboxAddress.Parse(email));
			emailToSend.Subject = subject;
			emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

			//send email
			using (var emailClient = new SmtpClient())
			{
				emailClient.Connect(_config.Smtp, _config.Port, MailKit.Security.SecureSocketOptions.StartTls);
				emailClient.Authenticate(_config.Username, _config.Password);
				emailClient.Send(emailToSend);
				emailClient.Disconnect(true);
			}

			return Task.CompletedTask;
		}
	}
}