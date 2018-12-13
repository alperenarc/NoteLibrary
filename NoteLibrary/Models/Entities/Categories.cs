using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLibrary.Models.Entities
{
    public class Categories
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }
        public bool State { get; set; }

        public Categories CategoriesID { get; set; }

    }
}
