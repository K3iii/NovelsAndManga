using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.Models
{
    public class MangaGenre
    {
        public int MangaId { get; set; }
        public MangaModels Manga { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
