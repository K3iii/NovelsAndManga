using NovelsManga.DataAccess.Data;
using NovelsManga.DataAccess.Repository.IRepository;
using NovelsManga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.DataAccess.Repository
{
    public class MangaChapterRepository : Repository<MangaChapter>, IMangaChapterRepository
    {
        private MangaNovelDatabase _db;
        public MangaChapterRepository(MangaNovelDatabase db) : base(db)
        {
            _db = db;
        }

        public void Update(MangaChapter chapter)
        {
            _db.mangaChapters.Update(chapter);
        }
    }
}
