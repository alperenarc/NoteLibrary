using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoteLibrary.Models.Contexts;
using NoteLibrary.Models.Entities;

namespace NoteLibrary.Controllers
{
    public class AdminController : Controller
    {
        private readonly NoteContext _context;

        public AdminController(NoteContext context)
        {
            _context = context;
        }
        // GET: Account/List
        public async Task<IActionResult> List()
        {
            if (HttpContext.Session.GetString("AdminSecurity") == "True")
            {
                return View(await _context.CategoryTable.ToListAsync());
            }
            else
            {
                return View("Index");
            }
        }
        // GET: Account/Login
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if (username == "notelibadmin" && password == "eru123notelib")
            {
                HttpContext.Session.SetString("AdminSecurity", "True");
                return RedirectToAction("HomePage", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz Kullanıcı");
                return View();
            }

        }


        // GET: Admin/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("AdminSecurity") == "True")
            {
                return View();
            }
            else
            {
                return View("Index");
            }
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,UpperId,Id,State")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return View(category);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("AdminSecurity") == "True")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = await _context.CategoryTable.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            else
            {
                return View("Index");
            }

        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,UpperId,Id,State")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("List", "Admin");
            }
            return View(category);
        }
        private bool CategoryExists(int id)
        {
            return _context.CategoryTable.Any(e => e.Id == id);
        }

        //File İşlemleri
        //File Listeleme
        //File Silme

        public async Task<IActionResult> FileList()
        {
            if (HttpContext.Session.GetString("AdminSecurity") == "True")
            {
                return View(await _context.FileTable.ToListAsync());
            }
            else
            {
                return View("Index");
            }
        }

        public async Task<IActionResult> FileEdit(int? id)
        {
            if (HttpContext.Session.GetString("AdminSecurity") == "True")
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
                return View("Index");
            }

        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FileEdit(int id, [Bind("CourseName,Title,Description,FilePath,University,Department,UploadDate,Id,State")] File file)
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
                return RedirectToAction("FileList", "Admin");
            }
            return View(file);
        }

        private bool FileExists(int id)
        {
            return _context.FileTable.Any(e => e.Id == id);
        }

        //User İşlemleri
        //User Listeleme
        //User Silme

        public async Task<IActionResult> UserList()
        {
            if (HttpContext.Session.GetString("AdminSecurity") == "True")
            {
                return View(await _context.UserTable.ToListAsync());
            }
            else
            {
                return View("Index");
            }
        }

        public async Task<IActionResult> UserEdit(int? id)
        {
            if (HttpContext.Session.GetString("AdminSecurity") == "True")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.UserTable.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            else
            {
                return View("Index");
            }

        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserEdit(int id, [Bind("Name,Surname,University,Department,City,IsTeacher,ConfirmGuid,IsConfirmed,Email,Hash,Id,State")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("UserList", "Admin");
            }
            return View(user);
        }

        private bool UserExists(int id)
        {
            return _context.UserTable.Any(e => e.Id == id);
        }


        public IActionResult HomePage()
        {
            if (HttpContext.Session.GetString("AdminSecurity") == "True")
            {
                return View();
            }
            else
            {
                return View("Index");
            }

        }
      
        public IActionResult Waitforapproval()
        {
            if (HttpContext.Session.GetString("AdminSecurity") == "True")
            {
                var file = _context.FileTable.Where(p => p.State == false);
                return View(file);
            }
            else
            {
                return View("Index");
            }
            
        }
       
    }
}
