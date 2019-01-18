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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name,Surname,City,University,Department,Email,Password,Id,State,ConfirmPassword")] User user)
        {
            if (user.Password == user.ConfirmPassword)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Login", "Account");
                }
            }
            else
            {
                ModelState.AddModelError("", "Tekrar Girilen Şifre Hatalı ! Lütfen Tekrar Deneyiniz.");
            }
            return View(user);
        }


        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {

            //Girilen bilgilerle uyuşan user var mı
            var kontrol = await _context.UserTable
                .FirstOrDefaultAsync(p => p.Email == user.Email && p.Password == user.Password);
            if (kontrol != null)
            {
                //state durumu nedir
                if (kontrol.State == false)
                {
                    ModelState.AddModelError("", "Geçersiz Kullanıcı");
                }
                else
                {
                    // session'a user Id'sini at ve indexe gönder.
                    HttpContext.Session.SetInt32("UserId", kontrol.Id);
                    HttpContext.Session.SetString("Authorize", "True");
                    return RedirectToAction("Index", "Account");
                }
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
            if (HttpContext.Session.GetInt32("UserId") == null || HttpContext.Session.GetInt32("UserId") ==0)
            {
                return RedirectToAction("Index", "Home");
            }
            int userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            var user = await _context.UserTable.FirstOrDefaultAsync(p => p.Id == userid);
            if(user == null)
            {
                return RedirectToAction("Index", "HomeController");
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Profile([Bind("Name,Surname,City,University,Department,Email,Password,Id,State,ConfirmPassword")] User user)
        {
            if(ModelState.IsValid)
            {
                int userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
                var dbUser = await _context.UserTable.FirstOrDefaultAsync(p => p.Id == userid);
                if(dbUser != null && user.Password ==user.ConfirmPassword)
                {
                    dbUser.Name = user.Name;
                    dbUser.Surname = user.Surname;
                    dbUser.City = user.City;
                    dbUser.University = user.University;
                    dbUser.Department = user.Department;
                    dbUser.Password = user.Password;
                    dbUser.ConfirmPassword = user.ConfirmPassword;
                    dbUser.Email = user.Email;
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Profile));
            }
            return View(user);
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