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
    public class FileController : Controller
    {
        private readonly NoteContext _context;

        public FileController(NoteContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            if (HttpContext.Session.GetString("Authorize") == "True")
            {
                var file = from m in _context.FileTable
                           select m;

                if (!String.IsNullOrEmpty(searchString))
                {
                    file = file.Where(s => s.CourseName.Contains(searchString));
                }

                return View(await file.ToListAsync());
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz Kullanıcı");
                return View("ErrorPage");
            }
        }
        public IActionResult ErrorPage()
        {
            return View();
        }
        // GET: File/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("Authorize") == "True")
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
            else
            {
                return View();
            }

        }

        // GET: File/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Authorize") == "False")
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public async Task<IActionResult> Create(User user,int categoryid, string courseName, string title, string description, string filePath)
        {
            int userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //var User = _context.UserTable
            //    .FirstOrDefaultAsync(p => p.Id == userid);
            //int ad = user.Id;
            //ad = userid;
            File file = new File();
            file.CourseName = courseName;
            file.Title = title;
            file.Description = description;
            file.FilePath = filePath;
            file.AddedUser.Name= "Alperen";
            file.UploadDate = DateTime.Now;
            _context.Add(file);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        // GET: File/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("Authorize") == "True")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var file = await _context.FileTable.FindAsync(id);
                if (file == null)
                {
                    return NotFound();
                }
                return View(file);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        // POST: File/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseName,Title,Description,FilePath,UploadDate,Id,State")] File file)
        {
            if (id != file.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(file);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(file.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(file);
        }

        // GET: File/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("Authorize") == "True")
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
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: File/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var file = await _context.FileTable.FindAsync(id);
            _context.FileTable.Remove(file);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileExists(int id)
        {
            return _context.FileTable.Any(e => e.Id == id);
        }
    }
}
