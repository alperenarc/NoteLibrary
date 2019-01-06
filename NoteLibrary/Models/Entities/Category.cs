using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLibrary.Models.Entities
{
    public class Category:Entity
    {
        [Required]
        public string Name { get; set; }

        //Navigation
        public ICollection<File> Files { get; set; }
        public Category CategoryId { get; set; }
    }
}
