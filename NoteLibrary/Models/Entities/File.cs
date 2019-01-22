using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLibrary.Models.Entities
{
    public class File:Entity
    {
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public DateTime UploadDate { get; set; }

        public string University { get; set; }

        public string Department { get; set; }
        
        //Navigation
        public User AddedUser { get; set; }
        public Category Category { get; set; }

    }
}
