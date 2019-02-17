using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NoteLibrary.Models.Contexts;
using NoteLibrary.Models.Entities;
using NoteLibrary.ViewModels;

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
            if (HttpContext.Session.GetString("AdminSecurity") == "True")
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
        public async  Task<IActionResult> Create()
        {
            if (HttpContext.Session.GetString("Authorize") == "False")
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var City= await _context.CategoryTable.Where(p => p.UpperId == 0).ToListAsync();
                City.Insert(0, new Category { Id = 0, Name = "Select" });
                ViewBag.ListofCities = City;

                return View();
            }

        }
        [HttpPost]
        public async Task<IActionResult> Create(FileCreateViewModel vm,string categoryLessonId,int uniCategoryId,int depCategoryId)
        {
            int userid = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            var categoryid = Convert.ToInt32(categoryLessonId); 
            var ctrgCourseName = await _context.CategoryTable.FirstOrDefaultAsync(p => p.Id == categoryid );
            var ctrgUni = await _context.CategoryTable.FirstOrDefaultAsync(p => p.Id == uniCategoryId );
            var ctrgDep = await _context.CategoryTable.FirstOrDefaultAsync(p => p.Id == depCategoryId );
            var usr = await _context.UserTable.FirstOrDefaultAsync(p => p.Id == userid);

            if (vm.FilePath == null || vm.Title == null || categoryid == 0 || uniCategoryId == 0 || depCategoryId == 0 || usr == null)
            {
                return Json(new { ok = false});
            }
            else {
                File file = new File();
                file.CourseName = ctrgCourseName.Name;
                file.Title = vm.Title;
                file.Description = vm.Description;
                file.FilePath = vm.FilePath;
                file.AddedUser = usr;
                file.UploadDate = DateTime.Now;
                file.Category = ctrgCourseName;
                file.University = ctrgUni.Name;
                file.Department = ctrgDep.Name;
                _context.Add(file);
                await _context.SaveChangesAsync();
                return Json(new { ok = true });
            }

            
        }

        // GET: File/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        //// GET: File/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (HttpContext.Session.GetString("AdminSecurity") == "True")
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var file = await _context.FileTable
        //            .FirstOrDefaultAsync(m => m.Id == id);
        //        if (file == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(file);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //}

        // POST: File/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var file = await _context.FileTable.FindAsync(id);
            _context.FileTable.Remove(file);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Account");
        }

        private bool FileExists(int id)
        {
            return _context.FileTable.Any(e => e.Id == id);
        }

        public JsonResult GetUniversities(int cityCategoryId)
        {
            List<Category> unis = _context.CategoryTable.Where(p => p.UpperId == cityCategoryId).ToList();
            unis.Insert(0, new Category { Id = 0, Name = "Select" });
            return Json(new SelectList(unis,"Id","Name"));
        }
        public JsonResult GetDepartment(int uniCategoryId)
        {
            List<Category> dep = _context.CategoryTable.Where(p => p.UpperId == uniCategoryId).ToList();
            dep.Insert(0, new Category { Id = 0, Name = "Select" });
            return Json(new SelectList(dep, "Id", "Name"));
        }
        public JsonResult GetLesson(int depCategoryId)
        {
            List<Category> les = _context.CategoryTable.Where(p => p.UpperId == depCategoryId).ToList();
            les.Insert(0, new Category { Id = 0, Name = "Select" });
            return Json(new SelectList(les, "Id", "Name"));
        }
        
    }
}
