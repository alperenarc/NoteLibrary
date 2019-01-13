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
                return RedirectToAction("List", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "Geçersiz Kullanıcı");
                return View();
            }
            
        }
        // GET: Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("AdminSecurity")=="True")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = await _context.CategoryTable
                    .FirstOrDefaultAsync(m => m.Id == id);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetString("AdminSecurity") == "True")
            {
                if (id == null)
                {
                    return NotFound();
                }

                var category = await _context.CategoryTable
                    .FirstOrDefaultAsync(m => m.Id == id);
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

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.CategoryTable.FindAsync(id);
            _context.CategoryTable.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.CategoryTable.Any(e => e.Id == id);
        }
    }
}
