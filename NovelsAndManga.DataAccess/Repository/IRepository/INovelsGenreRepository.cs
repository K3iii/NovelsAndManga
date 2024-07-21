using NovelsManga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.DataAccess.Repository.IRepository
{
    public interface INovelsGenreRepository : IRepository<NovelsGenre>
    {
        void Update(NovelsGenre obj);
    }
}
