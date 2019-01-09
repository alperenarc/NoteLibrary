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
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.AspNetCore.Http;

namespace NoteLibrary.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("UserId", 0);
            return View();
        }
        public IActionResult Contact()
        {
            HttpContext.Session.SetInt32("UserId", 0);
            return View();
        }
        public IActionResult SendMail(string email, string messagecontent, string subject, string name )
        {
            var message = new MimeMessage(); 
            message.From.Add(new MailboxAddress("alparicieren@gmail.com"));
            message.To.Add(new MailboxAddress("eren.arc.eren@gmail.com"));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = name + " 'den <br> " +
                email + " Mailinden <br> " +
                " Mesaj : " + message
            };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                //587
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("eren.arc.eren@gmail.com", "alparc817ismail.");
                client.Send(message);
                client.Disconnect(true);
            };
            return View("Index");

        }

    }
    
}
