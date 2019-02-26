using Microsoft.AspNetCore.Mvc.Rendering;
using NoteLibrary.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLibrary.ViewModels
{
    public class DepartmentViewModel
    {
        public List<File> Files;
        public SelectList Departments;
        public string DepartmentFile { get; set; }
        public string SearchString { get; set; }
    }
}
