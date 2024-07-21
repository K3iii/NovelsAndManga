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
    public class MangaRepository : Repository<MangaModels>, IMangaRepository
    {
        private MangaNovelDatabase _db;

        public MangaRepository(MangaNovelDatabase db) : base(db)
        {
            _db = db;
        }

        public void Update(MangaModels obj) 
        {
            _db.MangaPage.Update(obj);
        }
    }
}
