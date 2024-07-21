using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.Models
{
    public class MangaChapter
    {
        [Key]
        public int mangaChapterId { get; set; }
        [Required]
        public int chapterNum { get; set; }
        [ValidateNever]
        public string chapterFolder { get; set; }
        [ValidateNever]
        public DateTime dateCreated { get; set; }

        public int MangaId { get; set; }
        [ForeignKey("MangaId")]
        [ValidateNever]
        public MangaModels MangaModels { get; set; }
    }
}
