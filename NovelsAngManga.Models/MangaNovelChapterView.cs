using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.Models
{
    public class MangaNovelChapterView
    {
        public NovelsChapter novelChapter {  get; set; }
        public MangaChapter mangaChapter { get; set; }
        public int prevChapter { get; set; }
        public int nextChapter { get; set; }
        public int firstChapter { get; set; }
        public int lastChapter { get; set; }

        public List<string> mangaChapterImages { get; set; }
    }
}
