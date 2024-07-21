using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.Models
{
    public class MangaModels
    {
        public MangaModels()
        {
            MangaGenres = new List<MangaGenre>();
        }
        [Key]
        public int MangaId { get; set; }
        [Required]
        public string MangaName { get; set; }
        [Required]
        public string MangaType { get; set; }
        [Required]
        public string MangaSummary { get; set; }
        [Required]
        public string seriesStatus { get; set; }
        [Required]
        public string seriesAuthor { get; set; }
        [ValidateNever]
        public string ImgUrl { get; set; }

        public ICollection<MangaGenre> MangaGenres { get; set; }
    }
}
