using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using NoteLibrary.Models;
using System.Net;

namespace NoteLibrary.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SendMail()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("alperen", "alparicieren@gmail.com"));
            message.To.Add(new MailboxAddress("alperen", "eren.arc.eren@gmail.com"));
            message.Subject = "baslık";
            message.Body = new TextPart("Plain")
            {
                Text = "asd asd asd "
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("alparicieren@gmail.com", "67739599");
                client.Send(message);
                client.Disconnect(true);
            };

            return View();

        }

    }
    
}
