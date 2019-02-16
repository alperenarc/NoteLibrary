using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NoteLibrary.Models.Contexts;
using NoteLibrary.Models.Entities;
using NoteLibrary.ViewModels;

namespace NoteLibrary.Controllers
{
    public class AccountController : Controller
    {
        private readonly NoteContext _context;

        public AccountController(NoteContext context)
        {
            _context = context;
        }

        // GET: Account
        public async Task<IActionResult> Index(string searchString,int? page)
        {
            if (HttpContext.Session.GetInt32("UserId") == null || HttpContext.Session.GetInt32("UserId") == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            
            if (searchString != null)
            {
                page = 1;
            }
            
            IQueryable<Models.Entities.File> file = from m in _context.FileTable.
                                                    Where(p => p.State == true).Include(u => u.AddedUser).OrderByDescending(p=>p.UploadDate)
                                                    select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                file = file.Where(s => s.CourseName.Contains(searchString));
            }
            int pageSize = 15;
            return View(await HomepagePaginationViewModel<Models.Entities.File>.CreateAsync(
                file.AsNoTracking(), page ?? 1, pageSize));
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }
        // GET: Account/Register
        public IActionResult About()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(string Name, string Surname, string University, string Department, string City, string Email, string Password, string ConfirmPassword)
        {
            var User = await _context.UserTable.FirstOrDefaultAsync(p => p.Email == Email);
            if (User != null)
            {
                return Json(new { ok = false, message = "emailInvalid" });
            }
            else
            {
                if (Password == ConfirmPassword && Email != null && Name != null &&
                    Surname != null && University != null && Department != null && City != null)
                {

                    User user1 = new User();
                    string hashedPassword = Helper.PasswordHelper.HashPassword(Password);
                    user1.City = City;
                    user1.Department = Department;
                    user1.Email = Email;
                    user1.Hash = hashedPassword;
                    user1.Name = Name;
                    user1.Surname = Surname;
                    user1.University = University;
                    string GuidKey = Guid.NewGuid().ToString();
                    user1.ConfirmGuid = GuidKey;
                    
                    _context.Add(user1);
                    await _context.SaveChangesAsync();
                    Helper.EmailHelper.SendMail(user1.Email, user1.ConfirmGuid);

                    return Json(new { ok = true, newurl = Url.Action("Login") });

                }
                else
                {
                    return Json(new { ok = false, message = "Şifre veya Kullanıcı Adı Yanlış" });
                    //ModelState.AddModelError("", "Tekrar Girilen Şifre Hatalı ! Lütfen Tekrar Deneyiniz.");
                }
            }
            return View();
        }


        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {

            //Girilen bilgilerle uyuşan user var mı
            var kontrol = await _context.UserTable
                .FirstOrDefaultAsync(p => p.Email == Email);


             if (kontrol == null){
                    return Json(new { ok = false, message = "EmailInvalid" });
                 }

            else {

                try
                {
                    string correctHash = kontrol.Hash;
                    bool Varmi = Helper.PasswordHelper.ValidatePassword(Password, correctHash);

                    if (Varmi == true )
                    {
                        
                        //state durumu nedir
                        if (kontrol.State == false)
                        {
                            return Json(new { ok = false, message = "EmailInvalid" });
                        }
                        else
                        {
                            if (kontrol.IsConfirmed == false)
                            {
                                return Json(new { ok = false, message = "NotConfirmed" });

                            }
                            else
                            {
                                // session'a user Id'sini at ve indexe gönder.
                                HttpContext.Session.SetInt32("UserId", kontrol.Id);
                                HttpContext.Session.SetString("Authorize", "True");
                                Console.Write("Doğru");
                                return Json(new { ok = true, newurl = Url.Action("Index") });
                            }
                            
                        }

                    }
                    else
                    {
                        return Json(new { ok = false, message = "PasswordInvalid" });
                    }
                }
                catch (Exception)
                {
                    return Json(new { ok = false, message = "PasswordInvalid" });
                }



            }
                
        }
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("Authorize", "False");
            HttpContext.Session.SetInt32("UserId", 0);
            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Profile()
        {
            if (HttpContext.Session.GetInt32("UserId") == null || HttpContext.Session.GetInt32("UserId") == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            var user = await _context.UserTable.FirstOrDefaultAsync(p => p.Id == userid);
            if (user == null)
            {
                return RedirectToAction("Index", "HomeController");
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(string Name, string Surname, string City,
            string University, string Department, string Email, string OldPassword,
            string NewPassword, string NewPasswordConfirm)
        {

            int userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            var dbUser = await _context.UserTable.FirstOrDefaultAsync(p => p.Id == userid);

            if (dbUser != null && Name != null && Surname != null && City != null && University != null && Department != null && Email != null && OldPassword == null && NewPassword == null && NewPasswordConfirm == null)
            {
                dbUser.Name = Name;
                dbUser.Surname = Surname;
                dbUser.City = City;
                dbUser.University = University;
                dbUser.Department = Department;
                dbUser.Email = Email;
                await _context.SaveChangesAsync();
                return Json(new { ok = true, newurl = Url.Action("Index") });

            }
            else
            {
                if (dbUser != null && NewPassword == NewPasswordConfirm)
                {
                    try
                    {
                        string oldhash = dbUser.Hash;

                        string NewHash = Helper.PasswordHelper.ChangePassword(OldPassword, oldhash, NewPassword);
                        if (NewHash != "")
                        {
                            dbUser.Hash = NewHash;
                            dbUser.Name = Name;
                            dbUser.Surname = Surname;
                            dbUser.City = City;
                            dbUser.University = University;
                            dbUser.Department = Department;
                            dbUser.Email = Email;
                            await _context.SaveChangesAsync();
                            return Json(new { ok = true, newurl = Url.Action("Index") });
                        }

                    }
                    catch (Exception)
                    {
                        Console.Write("Yanlışlık var");
                        return Json(new { ok = false, message = "Hata oldu tekrar deneyiniz!" });
                    }
                }
            }
          
            return View();
        }

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
        public IActionResult Contact()
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


        public async Task<IActionResult> MyFiles(string searchString, int? page)
        {
            if (HttpContext.Session.GetInt32("UserId") == null || HttpContext.Session.GetInt32("UserId") == 0)
            {
                return RedirectToAction("Index", "Home");
            }


            if (searchString != null)
            {
                page = 1;
            }

            int userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));

            User usr = await _context.UserTable.FirstOrDefaultAsync(p => p.Id == userid);
           

            IQueryable<Models.Entities.File> file = from m in _context.FileTable.
                                                    Where(p => p.State == true && p.AddedUser == usr).Include(u => u.AddedUser)
                                                    select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                file = file.Where(s => s.CourseName.Contains(searchString));
            }
            int pageSize = 15;
            return View(await HomepagePaginationViewModel<Models.Entities.File>.CreateAsync(
                file.AsNoTracking(), page ?? 1, pageSize));
        }

    }
}