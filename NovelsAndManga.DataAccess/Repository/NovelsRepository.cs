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
    public class NovelsRepository : Repository<NovelsModels>, INovelsRepository
    {
        private MangaNovelDatabase _db;
        public NovelsRepository(MangaNovelDatabase db) : base(db)
        {
            _db = db;
        }
        public void Update(NovelsModels obj)
        {
            _db.NovelsPage.Update(obj);
        }
    }
}
