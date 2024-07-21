using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace NovelsManga.Models
{
    public class NovelsModels
    {
        public NovelsModels()
        {
            NovelGenres = new List<NovelsGenre>();
        }
        [Key]
        public int NovelsId { get; set; }
        [Required]
        public string NovelsName { get; set; }
        [Required]
        public string NovelsType { get; set;}
        [Required]
        public string NovelsSummary { get; set; }
        [Required]
        public string seriesStatus { get; set; }
        [Required]
        public string seriesAuthor { get; set; }
        [ValidateNever]
        public string ImgUrl { get; set; }

        public ICollection<NovelsGenre> NovelGenres { get; set; }
    }
}
