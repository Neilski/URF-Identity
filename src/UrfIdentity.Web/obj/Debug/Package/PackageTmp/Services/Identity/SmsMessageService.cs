using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Twilio;


namespace UrfIdentity.Web.Services.Identity
{
    public class SmsMessagingService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Grab the Twilio account details from Web.config
            var twilioSid = ConfigurationManager
                .AppSettings["IdentityTwilioSid"];
            var twilioAuthToken = ConfigurationManager
                .AppSettings["IdentityTwilioAuthToken"];
            var twilioNumber = ConfigurationManager
                .AppSettings["IdentityTwilioNumber"];

            // Plug in your sms service here to send a text message.
            var twilio = new TwilioRestClient(twilioSid, twilioAuthToken);
            var result = twilio.SendMessage(
                twilioNumber, message.Destination, message.Body);

            return Task.FromResult(0);
        }
    }
}