using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template_HardwareStore.PL.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string body)
        {
            MailjetClient client = new MailjetClient(
               "e5d5db5533612115b448eb7980d362e1", "c1640541a76e3808d1f97a0e79b28552"){};

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
                  new JObject {
                   {
                    "From",
                    new JObject {
                     {"Email", "fearofwar@mail.ru"},
                     {"Name", "Илья"}
                    }
                   }, {
                    "To",
                    new JArray {
                     new JObject {
                      {
                       "Email",
                       email
                      }, {
                       "Name",
                       "DotNetMastery"
                      }
                     }
                    }
                   }, {
                    "Subject",
                    subject
                   },  {
                    "HTMLPart",
                    body
                   }, {
                    "CustomID",
                    "AppGettingStartedTest"
                   }
                  }
             });
            MailjetResponse response = await client.PostAsync(request);
        }
    }
}
