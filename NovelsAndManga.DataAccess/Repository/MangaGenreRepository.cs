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
    public class MangaGenreRepository : Repository<MangaGenre>, IMangaGenreRepository
	{
        private MangaNovelDatabase _db;
        public MangaGenreRepository(MangaNovelDatabase db) : base(db)
        {
            _db = db;
        }

        public void Update(MangaGenre obj)
        {
            _db.mangaGenres.Update(obj);
        }
    }
}
