using NovelsManga.DataAccess.Data;
using NovelsManga.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelsManga.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private MangaNovelDatabase _db;
        public IGenreRepository Genre { get; private set; }
        public INovelsRepository novelPage { get; private set; } 
        public INovelsGenreRepository novelsGenre { get; private set; }
        public INovelChapterRepository novelChapter { get; private set; }
        public IMangaRepository mangaPage { get; private set; }
        public IMangaGenreRepository mangaGenre { get; private set; }
        public IMangaChapterRepository mangaChapter { get; private set; }
        public UnitOfWork(MangaNovelDatabase db)
        {
            _db = db;
            Genre = new GenreRepository(_db);
            novelPage = new NovelsRepository(_db);
            novelsGenre = new NovelsGenreRepository(_db);
            novelChapter = new NovelChapterRepository(_db);
            mangaPage = new MangaRepository(_db);
            mangaGenre = new MangaGenreRepository(_db);
            mangaChapter = new MangaChapterRepository(_db);
        }
        

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
