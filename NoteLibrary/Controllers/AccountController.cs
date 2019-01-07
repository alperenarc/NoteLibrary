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
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
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
                    //return RedirectToAction(nameof(Index));
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
                    return RedirectToAction("Index", "Account");
                }
            }

            return View();
        }
    }
}
