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
using NoteLibrary.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Korzh.EasyQuery.Linq;

namespace NoteLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly NoteContext _context;

        public HomeController(NoteContext context)
        {

            _context = context;
        }
        public async Task<IActionResult> Index(string searchString)
        {
            HttpContext.Session.SetString("Authorize", "False");
            
            var file = from m in _context.FileTable
                       select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                file = file.Where(s => s.CourseName.Contains(searchString));
            }

            return View(await file.ToListAsync());
        }
        public IActionResult Contact()
        {
            HttpContext.Session.SetInt32("UserId", 0);
            return View();
        }
       
        public IActionResult ErrorPage()
        {
            return View();
        }
        public IActionResult SendMail(string email, string messagecontent, string subject, string name )
        {
            if (email == null || messagecontent==null|| subject==null|| name==null)
            {
                ModelState.AddModelError("Hata", "Eksik Bilgi");
                return View("ErrorPage");
            }
            else
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
        // GET: File/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.FileTable
                .FirstOrDefaultAsync(m => m.Id == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

    }
    
}
