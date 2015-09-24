using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;


namespace UrfIdentity.Web.Services.Identity
{
    public class EmailMessagingService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var fromEmailAddress = ConfigurationManager
                .AppSettings["IdentityFromEmailAddress"];

            var text = message.Body;
            var html = message.Body;

            // Do whatever you want to the message
            using (var msg = new MailMessage())
            {
                msg.From = new MailAddress(fromEmailAddress);
                msg.To.Add(new MailAddress(message.Destination));
                msg.Subject = message.Subject;

                msg.AlternateViews.Add(
                    AlternateView.CreateAlternateViewFromString(
                        text, null, MediaTypeNames.Text.Plain)
                    );

                msg.AlternateViews.Add(
                    AlternateView.CreateAlternateViewFromString(
                        html, null, MediaTypeNames.Text.Html)
                    );


                // var smtpClient = new SmtpClient("smtp.whatever.net", Convert.ToInt32(587));
                // var credentials = new System.Net.NetworkCredential(Keys.EmailUser, Keys.EMailKey);
                // smtpClient.Credentials = credentials;

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Send(msg);
                }
            }
            return Task.FromResult(0);
        }
    }
}