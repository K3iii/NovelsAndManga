using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NovelsManga.Models;

namespace NovelsManga.DataAccess.Data
{
    public class MangaNovelDatabase : IdentityDbContext<IdentityUser>
    {
        public MangaNovelDatabase(DbContextOptions<MangaNovelDatabase> options) : base(options)
        {

        }

        public DbSet<Genre> Genre { get; set; }
        public DbSet<NovelsModels> NovelsPage { get; set; }
        public DbSet<NovelsGenre> novelsGenres { get; set; }
        public DbSet<NovelsChapter> NovelsChapters { get; set; }

        public DbSet<MangaModels> MangaPage { get; set; }
        public DbSet<MangaGenre> mangaGenres { get; set; }
        public DbSet<MangaChapter>  mangaChapters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure NovelGenre as a join table with composite primary key

            modelBuilder.Entity<NovelsGenre>()
                .HasKey(ng => new { ng.NovelsId, ng.GenreId });

            // Configure relationships
            modelBuilder.Entity<NovelsGenre>()
                .HasOne(ng => ng.Novel)
                .WithMany(n => n.NovelGenres)
                .HasForeignKey(ng => ng.NovelsId);

            modelBuilder.Entity<NovelsGenre>()
                .HasOne(ng => ng.Genre)
                .WithMany(g => g.NovelGenres)
                .HasForeignKey(ng => ng.GenreId);


            modelBuilder.Entity<MangaGenre>()
                .HasKey(mg => new { mg.MangaId, mg.GenreId });

            modelBuilder.Entity<MangaGenre>()
                .HasOne(mg => mg.Manga)
                .WithMany(m => m.MangaGenres)
                .HasForeignKey(mg => mg.MangaId);

            modelBuilder.Entity<MangaGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(m => m.MangaGenres)
                .HasForeignKey(mg => mg.GenreId);
        }
    }
}
