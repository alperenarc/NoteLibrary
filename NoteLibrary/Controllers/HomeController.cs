﻿using System;
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
using NoteLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NoteLibrary.Controllers
{
    public class HomeController : Controller
    {
        private readonly NoteContext _context;

        public HomeController(NoteContext context)
        {

            _context = context;
        }
        public async Task<IActionResult> Index(string searchString, string DepartmentFile, int? page)
        {
            HttpContext.Session.SetString("Authorize", "False");
            

            if (searchString != null)
            {
                page = 1;
            }

            IQueryable<Models.Entities.File> file = from m in _context.FileTable.
                                                    Where(p => p.State == true).Include(u => u.AddedUser).OrderByDescending(p=>p.UploadDate)
                                                    select m;

            IQueryable<string> genreQuery = from m in _context.FileTable
                                            orderby m.Department
                                            select m.Department;

            

            if (!String.IsNullOrEmpty(searchString))
            {
                file = file.Where(s => s.CourseName.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(DepartmentFile))
            {
                file = file.Where(s => s.Department.Contains(DepartmentFile));
            }

            var VMDep = new DepartmentViewModel
            {
                Departments = new SelectList(await genreQuery.Distinct().ToListAsync()),
                Files = await file.ToListAsync()
            };


            int pageSize = 15;
            return View(await HomepagePaginationViewModel<Models.Entities.File>.CreateAsync(
                file.AsNoTracking(), page ?? 1, pageSize));
           
        }
        public IActionResult Contact()
        {
            HttpContext.Session.SetInt32("UserId", 0);
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult ErrorPage()
        {
            return View();
        }
        public IActionResult SendMail(string email, string messagecontent, string subject, string name)
        {
            if (email == null || messagecontent == null || subject == null || name == null)
            {
                ModelState.AddModelError("Hata", "Eksik Bilgi");
                return View("ErrorPage");
            }
            else
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("info@nootelib.com"));
                message.To.Add(new MailboxAddress("info@nootelib.com"));
                message.Subject = subject;
                message.Body = new TextPart("html")
                {
                    Text = "BAŞLIK:" + subject + "<br>" +
                    name + " 'dan <br> " +
                    email + " 'dan <br> " +
                    " Mesaj : " + messagecontent
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    //587
                    client.Connect("srvm04.turhost.com", 587, false);
                    client.Authenticate("info@nootelib.com", "Qwerty123");
                    client.Send(message);
                    client.Disconnect(true);
                };
                return RedirectToAction("Index", "Home");
            }
        }
        // GET: File/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.FileTable.Include(p => p.AddedUser).Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }

    }

}
