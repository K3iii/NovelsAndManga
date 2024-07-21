using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.Models
{
    public class MangaGenreView
    {
        public MangaModels NewManga { get; set; }
        public List<Genre> Genre { get; set; }
        public List<int> SelectedMangaGenreIds { get; set; } // To hold selected genre IDs

        public MangaGenreView()
        {
            Genre = new List<Genre>();
        }
    }
}
