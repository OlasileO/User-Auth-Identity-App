using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;
using User_Identity_App.Helpers;
using User_Identity_App.Interfaces;

namespace User_Identity_App.Services
{
    public class SendGridEmail: ISendGridEmail
    {
        private readonly ILogger<SendGridEmail> _logger;

        public AuthMessageSenderOptions Options { get; set; }   
        public SendGridEmail(IOptions<AuthMessageSenderOptions> options,
            ILogger<SendGridEmail> logger
            )
        {
            Options = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Options.ApiKey))
            {
                throw new Exception("Null SendGridKey");
            }
            await Execute(Options.ApiKey, subject, message, toEmail);
        }

        private async Task Execute(string apiKey, string subject, string message, string toEmail)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("sileowoyinmika@gmail.com", "Password Recovery"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            var response = await client.SendEmailAsync(msg);
            _logger.LogInformation(response.IsSuccessStatusCode
                                   ? $"Email to {toEmail} queued successfully!"
                                   : $"Failure Email to {toEmail}");
        }
    }
}

