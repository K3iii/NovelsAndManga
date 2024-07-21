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
    public class NovelsGenreRepository : Repository<NovelsGenre>, INovelsGenreRepository
    {
        private MangaNovelDatabase _db;
        public NovelsGenreRepository(MangaNovelDatabase db) : base(db)
        {
            _db = db;
        }

        public void Update(NovelsGenre obj)
        {
            _db.novelsGenres.Update(obj);
        }
    }
}
