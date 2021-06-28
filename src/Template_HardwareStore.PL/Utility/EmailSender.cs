using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
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
            MailjetClient client = new MailjetClient("e5d5db5533612115b448eb7980d362e1", "c1640541a76e3808d1f97a0e79b28552"){ Version = ApiVersion.V3_1, };
             MailjetRequest request = new MailjetRequest
             {
                 Resource = Send.Resource,
             }
                .Property(Send.Messages, new JArray {
                    new JObject {
                        {"From",new JObject {
                                    {"Email", "HardWareStore@gmail.com"},
                                    {"Name", "HardWareStore"}}},
                        {"To",new JArray {new JObject {
                                    {"Email", $"{email}"},
                                    {"Name", "Илья"}}}},
                        {"Subject","Greetings from Mailjet."},
                        {"TextPart", "Used MailJet"}, 
                        {"HTMLPart", $"{body}"},
                        {"CustomID", "AppGettingStartedTest"}}});

            MailjetResponse response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
                Console.WriteLine(response.GetData());
            }

            else
            {
                Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
                Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
                Console.WriteLine(response.GetData());
                Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
            }
        }
    }
}
