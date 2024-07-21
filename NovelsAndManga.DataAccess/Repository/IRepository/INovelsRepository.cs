using NovelsManga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.DataAccess.Repository.IRepository
{
    public interface INovelsRepository : IRepository<NovelsModels>
    {
        void Update(NovelsModels obj);
    }
}
