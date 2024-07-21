using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.Models
{
    public class NovelsGenreView
    {
        public NovelsModels NewNovel { get; set; }
        public List<Genre> Genre { get; set; }
        public List<int> SelectedGenreIds { get; set; } // To hold selected genre IDs

        public NovelsGenreView()
        {
            Genre = new List<Genre>();
        }
    }
}
