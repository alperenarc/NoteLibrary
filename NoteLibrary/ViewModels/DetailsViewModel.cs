using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using NoteLibrary.Models.Entities;

namespace NoteLibrary.ViewModels
{
    public class DetailsViewModel
    {
        public File file;
        public bool isMyFile;
    }
}