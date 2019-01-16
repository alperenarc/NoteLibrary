using Microsoft.AspNetCore.Mvc.Rendering;
using NoteLibrary.Models.Contexts;
using NoteLibrary.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLibrary.ViewModels
{
    public class FileCreateViewModel
    {
        
        public string CourseName { get; set; }
        
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public string FilePath { get; set; }
       
        public DateTime UploadDate { get; set; }
       
        public User AddedUser { get; set; }

        //Category Ayarlamak için gerekli fieldlar
        public int City { get; set; }
        public int University { get; set; }
        public int Department { get; set; }
        public string Lesson { get; set; }
        ///Category Ayarlamak için gerekli fieldlar
        public Category Category { get; set; }

       
    }
}
