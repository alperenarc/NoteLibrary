using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLibrary.Helper
{
    public class EmailHelper
    {
        public static void SendMail(string email, string GuideCode)
        {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("info@nootelib.com"));
            message.To.Add(new MailboxAddress(email));
            message.Subject = "Email Onaylama";
            message.Body = new TextPart("html")
            {
                Text = "Hesabınızı onaylamak için aşağıdaki linke tıklayınız... <br/>" +
                "<a href='https://nootelib.com/Confirmation/Verification/?guidcode=" + GuideCode + "'>Onaylama Linki<a/>"
                //Confirmation / Verification /
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                //587
                client.Connect("srvm04.turhost.com", 587, false);
                client.Authenticate("info@nootelib.com", "Qwerty123");
                client.Send(message);
                client.Disconnect(true);
            };
            

        }
    }
}
