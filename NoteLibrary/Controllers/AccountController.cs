using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoteLibrary.Models.Contexts;
using NoteLibrary.Models.Entities;

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
        public async Task<IActionResult> Index(string searchString)
        {
            if (HttpContext.Session.GetInt32("UserId") == null || HttpContext.Session.GetInt32("UserId") == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var file = from m in _context.FileTable
                       select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                file = file.Where(s => s.CourseName.Contains(searchString));
            }

            return View(await file.ToListAsync());
        }

        // GET: Account/Register
        public IActionResult Register()
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
                ModelState.AddModelError("", "Bu E-mail'e ait bir hesap vardır ! Lütfen Tekrar Deneyiniz.");
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
                    _context.Add(user1);
                    await _context.SaveChangesAsync();
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

            try
            {
                string correctHash = kontrol.Hash;
                bool Varmi = Helper.PasswordHelper.ValidatePassword(Password, correctHash);

                if (Varmi == true && kontrol != null)
                {

                    //state durumu nedir
                    if (kontrol.State == false)
                    {
                        Console.Write("geçersiz");
                        ModelState.AddModelError("", "Geçersiz Kullanıcı");
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
            catch (Exception)
            {
                ModelState.AddModelError("", "Şifre veya Kullanıcı Adı Yanlış");
                Console.Write("Şifre veya Kullanıcı Adı Yanlış");
                return Json(new { ok = false, message = "Şifre veya Kullanıcı Adı Yanlış" });

            }
            return View();
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



    }
}