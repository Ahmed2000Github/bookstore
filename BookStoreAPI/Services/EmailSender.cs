using System.Net.Mail;

namespace BookStoreAPI.Services
{
    public class EmailSender
    {
        public static EmailSender Default = new EmailSender();
        private EmailSender() { }

        public bool send(string email,string messageObject, string message) {
        
            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            // setup Smtp authentication
            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential("bookstoreaspnetcore@gmail.com", "mpczqadtupdygrtv");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("bookstoreaspnetcore@gmail.com");
            msg.To.Add(new MailAddress(email));

            msg.Subject = messageObject;
            msg.IsBodyHtml = true;
            msg.Body = string.Format("<html><head></head><body><b>" + message + "</b></body>");

            try
            {
                client.Send(msg);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
