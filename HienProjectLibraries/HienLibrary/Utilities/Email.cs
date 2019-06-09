using System;
using System.Net;
using System.Net.Mail;


namespace Utilities
{
    public class Email
    {
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static string SendGmail(string mailTo, string subject, string body, string fromMail = "nomad1234vn@gmail.com", string passFromMail = "ma8635047")
        {
            var msg = "";
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(fromMail)
                };
                mail.To.Add(mailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                var smtp = new SmtpClient
                {
                    Port = 587,
                    Host = "smtp.gmail.com",
                    //danh cho gmail
                    UseDefaultCredentials = false,
                    //...........
                    Credentials = new NetworkCredential(fromMail, passFromMail),
                    EnableSsl = true
                };
                smtp.Send(mail);
                msg = "";
            }
            catch (Exception ex)
            {
                
            }

            return msg;
        }

        public static string SendMail(string mailTo, string subject, string body, string fromemail, string Port,
            string Smtp, string frompass)
        {
            string msg = null;
            try
            {
                var mail = new MailMessage
                {
                    From = new MailAddress(fromemail)
                };
                mail.To.Add(mailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                var smtp = new SmtpClient
                {
                    Port = int.Parse(Port),
                    Host = Smtp,
                    Credentials = new NetworkCredential(fromemail, frompass),
                    EnableSsl = false
                };
                smtp.Send(mail);
                msg = "";
            }
            catch (Exception ex)
            {
                
            }

            return msg;
        }
    }
}