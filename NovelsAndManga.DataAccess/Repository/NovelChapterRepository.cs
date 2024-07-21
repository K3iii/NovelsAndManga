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
    public class NovelChapterRepository : Repository<NovelsChapter>, INovelChapterRepository
    {
        private MangaNovelDatabase _db;
        public NovelChapterRepository(MangaNovelDatabase db) : base(db)
        {
            _db = db;
        }

        public void Update(NovelsChapter chapter)
        {
            _db.NovelsChapters.Update(chapter);
        }
    }
}
