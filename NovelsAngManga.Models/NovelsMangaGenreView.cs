using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.Models
{
    public class NovelsMangaGenreView
    {
        public List<NovelsModels> NovelsPage { get; set; }
        public List<MangaModels> MangaPage { get; set; }
        public List<Genre> Genre { get; set; }
        public List<NovelsGenre> novelsGenres { get; set; }
        public List<MangaGenre> mangaGenres { get; set; }
        public List<int> SelectedGenreIds { get; set; }
        public List<NovelsChapter> Chapters { get; set; }
        public List<MangaChapter> MangaChapters { get; set;}
    }
}
