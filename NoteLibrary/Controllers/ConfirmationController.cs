using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteLibrary.Models.Contexts;

namespace NoteLibrary.Controllers
{
    public class ConfirmationController : Controller
    {
        private readonly NoteContext _context;
        public ConfirmationController(NoteContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Verification(string guidcode)
        {
            guidcode = HttpContext.Request.Query["guidcode"];
            if (guidcode == null)
            {
                return NotFound();
            }

            //var user = await _context.Users.FindAsync(guidcode);
            var user = await _context.UserTable.FirstOrDefaultAsync(p => p.ConfirmGuid == guidcode);
            user.IsConfirmed = true;
            _context.Update(user);
            await _context.SaveChangesAsync();

            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}