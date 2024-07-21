using System.ComponentModel.DataAnnotations;

namespace NovelsManga.Models
{
    public class Genre
    {

        public Genre()
        {
            NovelGenres = new List<NovelsGenre>();
        }

        [Key]
        public int GenreId { get; set; }
        [Required]
        [MaxLength(50)]

        public string GenreTitle { get; set; }

        public ICollection<NovelsGenre> NovelGenres { get; set; }
        public ICollection<MangaGenre> MangaGenres { get; set; }
    }
}
