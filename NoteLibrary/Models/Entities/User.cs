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
        [Display(Name = "Ad")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Şehir")]
        public string City { get; set; }
        [Required]
        [Display(Name = "Üniversite")]
        public string University { get; set; }
        [Required]
        [Display(Name = "Bölüm")]
        public string Department { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //Navigation
        public ICollection<File> Files { get; set; }


    }
}
