using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLibrary.Models.Entities
{
    public class User:Entity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        //[Key]
        //[Required]
        //public string UserName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string University { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //Navigation
        public ICollection<File> Files { get; set; }


    }
}
