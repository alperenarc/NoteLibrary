using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Register([Bind("Name,Surname,City,University,Department,Email,Password,Id,State")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}
