using NovelsManga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.DataAccess.Repository.IRepository
{
    public interface IMangaChapterRepository : IRepository<MangaChapter>
    {
        void Update(MangaChapter obj);
    }
}
