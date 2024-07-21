using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.Models
{
    public class NovelsChapter
    {
        

        [Key]
        public int novelChapterId { get; set; }
        [Required]
        public int chapterNum { get; set; }
        [Required]
        public string chapterContent { get; set; }
        [ValidateNever]
        public DateTime dateCreated { get; set; }

        public int NovelsId { get; set; }
        [ForeignKey("NovelsId")]
        [ValidateNever]
        public NovelsModels NovelsModels { get; set; }
    }
}
