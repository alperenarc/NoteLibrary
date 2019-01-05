using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLibrary.Models.Entities
{
    public class Entity:Interfaces.IEntityBase
    {
        public int Id { get; set; }
        public bool State { get; set; }
    }
}
