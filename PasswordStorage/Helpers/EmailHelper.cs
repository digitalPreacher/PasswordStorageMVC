using System.Diagnostics;
using System.Net.Mail;

namespace PasswordStorage.Helpers
{
    public class EmailHelper
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;

        public EmailHelper()
        {
            _host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? throw new ArgumentException("Invalid SMTP host");
            _port = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out int port) 
                ? _port = port : throw new ArgumentException("Invalid SMTP port");
            _username = Environment.GetEnvironmentVariable("SMTP_HOST_LOGIN") ?? throw new ArgumentException("Invalid login to host");
            _password = Environment.GetEnvironmentVariable("SMTP_HOST_PASSWORD") ?? throw new ArgumentException("Invalid login to host");

        }
        public bool SendEmailPasswordReset(string userEmail, string link)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("info@password-storage.ru");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Сброс пароля";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = link;

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(_username, _password);
            client.Host = _host;
            client.EnableSsl = true;
            client.Port = _port;
            client.UseDefaultCredentials = false;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return false;
        }

        public bool SendEmailUserFeedback(string userEmail, string description)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("feedback@password-storage.ru") ;
            mailMessage.To.Add(new MailAddress("watcman35@yandex.ru"));

            mailMessage.Subject = $"Сообщение о проблеме";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = $"<p>Описание: {description}<p>" +
                $"<p>Email: {userEmail}<p>";

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(_username, _password);
            client.Host = _host;
            client.EnableSsl = true;
            client.Port = _port;
            client.UseDefaultCredentials = false;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return false;

        }
    }
}
