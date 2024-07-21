using NovelsManga.DataAccess.Data;
using NovelsManga.DataAccess.Repository.IRepository;
using NovelsManga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.DataAccess.Repository
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {

        private MangaNovelDatabase _db;
        public GenreRepository(MangaNovelDatabase db) : base(db)
        {
            _db = db;
        }
        

        public void Update(Genre obj)
        {
            _db.Genre.Update(obj);
        }
    }
}
