using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IGenreRepository Genre { get; }
        INovelsRepository novelPage { get; }
        INovelsGenreRepository novelsGenre { get; }
        INovelChapterRepository novelChapter { get; }
        IMangaRepository mangaPage { get; }
        IMangaGenreRepository mangaGenre { get; }
        IMangaChapterRepository mangaChapter { get; }
        void Save();
    }
}
