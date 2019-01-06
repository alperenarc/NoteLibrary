using Microsoft.EntityFrameworkCore;
using NoteLibrary.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NoteLibrary.Models.Contexts
{
    public class NoteContext : DbContext
    {
        public NoteContext(DbContextOptions<NoteContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(b => b.State)
                .HasDefaultValue(true);
            modelBuilder.Entity<Category>()
                .Property(b => b.State)
                .HasDefaultValue(true);
            modelBuilder.Entity<File>()
                .Property(b => b.State)
                .HasDefaultValue(true);
        }

        public DbSet<Category> CategoryTable { get; set; }
        public DbSet<User> UserTable { get; set; }
        public DbSet<File> FileTable { get; set; }

        
    }
}
