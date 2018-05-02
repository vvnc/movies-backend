using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MoviesBackend.Exceptions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MoviesBackend.Services
{
  public class SendGridMessageService : IMessageService
  {
    const string SENDGRID_KEY_ENV_VAR = "MOVIES_SENDGRID_KEY";
    const string SENDER_EMAIL_ENV_VAR = "MOVIES_SENDER_EMAIL";

    public async Task Send(string email, string subject, string message)
    {
      string apiKey = Environment.GetEnvironmentVariable(SENDGRID_KEY_ENV_VAR);
      if (apiKey == null)
      {
        throw new EmailException($"Couldn't find sendgrid key environment variable: ${SENDGRID_KEY_ENV_VAR}");
      }
      var client = new SendGridClient(apiKey);

      string senderEmail = Environment.GetEnvironmentVariable(SENDER_EMAIL_ENV_VAR);
      if (senderEmail == null)
      {
        throw new EmailException($"Couldn't find sender email environment variable: ${SENDER_EMAIL_ENV_VAR}");
      }

      var from = new EmailAddress(senderEmail, "Movies Backend Info");
      var to = new EmailAddress(email, email);
      var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
      Response response = await client.SendEmailAsync(msg);
      if (response.StatusCode != HttpStatusCode.Accepted)
      {
        throw new EmailException($"Send grid request failed with status code:${response.StatusCode}");
      }
    }
  }
}
