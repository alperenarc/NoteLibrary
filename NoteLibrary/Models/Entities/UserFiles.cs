using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLibrary.Models.Entities
{
    public class UserFiles:Entity
    {
        [Required]
        public string FilePath { get; set; }
        [Required]
        public DateTime UploadDate { get; set; }

        //Navigation
        public User AddedUser { get; set; }
        public Category Category { get; set; }
    }
}
