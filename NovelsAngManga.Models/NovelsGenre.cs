namespace NovelsManga.Models
{
    public class NovelsGenre
    {
        public int NovelsId { get; set; }
        public NovelsModels Novel { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
