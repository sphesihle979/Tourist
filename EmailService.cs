using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Tourist
{
    public static class EmailService
    {
        // Put your sender email and its APP PASSWORD here (Gmail: turn on 2-step verification,
        // then create an "App password"). Never use your normal password.
        public static string Host = "smtp.gmail.com";
        public static int Port = 587;
        public static string User = "lungelobhengu987@gmail.com";        
        public static string Password = "qctaezfdlyfplxli";   
        public static string FromName = "Blue Horizon Tours";

        public static async Task SendTicketsAsync(string toEmail, string guestName, EventInfo ev,
                                                  List<string> ticketNumbers, List<byte[]> pdfs)
        {
            using (var msg = new MailMessage())
            {
                msg.From = new MailAddress(User, FromName);
                msg.To.Add(new MailAddress(toEmail));
                msg.Subject = "Your tickets for " + ev.Name;
                msg.Body = "Hi " + guestName + ",\r\n\r\n" +
                           "Thank you for booking with Blue Horizon.\r\n\r\n" +
                           "Event: " + ev.Name + "\r\n" +
                           "Venue: " + ev.Venue + "\r\n" +
                           "Date & time: " + ev.DateText + "\r\n" +
                           "Tickets attached: " + pdfs.Count + "\r\n\r\n" +
                           "Please bring your tickets to the event.\r\n\r\nBlue Horizon | Explore South Africa";

                var streams = new List<MemoryStream>();
                try
                {
                    for (int i = 0; i < pdfs.Count; i++)
                    {
                        var ms = new MemoryStream(pdfs[i]);
                        streams.Add(ms);
                        msg.Attachments.Add(new Attachment(ms, "Ticket-" + ticketNumbers[i] + ".pdf", "application/pdf"));
                    }

                    using (var smtp = new SmtpClient(Host, Port))
                    {
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential(User, Password);
                        await smtp.SendMailAsync(msg);
                    }
                }
                finally { foreach (var s in streams) s.Dispose(); }
            }
        }
    }
    
}
