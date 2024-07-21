using Microsoft.AspNetCore.Mvc;
using NovelsManga.DataAccess.Repository.IRepository;
using NovelsManga.Models;

namespace NovelsAndManga.Areas.manga.Controllers
{
    [Area("manga")]
    [Route("manga")]
    public class MangaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MangaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{mangaId}")]
        public IActionResult Index(int mangaId)
        {
            var manga = _unitOfWork.mangaPage.GetFirstOrDefault(u => u.MangaId == mangaId);
            //var novels = _unitOfWork.novelPage.GetAll().Where(u => u.NovelsName == novelName).ToList();
            //NovelsModels novelsMangaGenreView = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsName == novelName);
            //var novelInfo = _unitOfWork.novelPage.GetAll().Where(u => u.NovelsName == novelName);
            var genre = _unitOfWork.mangaGenre.GetAll().Where(u => u.MangaId == mangaId).Select(u => u.GenreId).ToList();
            var chapterList = _unitOfWork.mangaChapter.GetAll().Where(u => u.MangaId == mangaId).OrderBy(u => u.chapterNum).ToList();

            var mangapage = new NovelsMangaGenreView
            {
                MangaPage = new List<MangaModels> { manga },
                SelectedGenreIds = genre,
                Genre = _unitOfWork.Genre.GetAll().ToList(),
                MangaChapters = chapterList
            };

            return View(mangapage);
        }

        [HttpGet("{mangaId}/{chapterId}")]
        public IActionResult Chapter( int chapterId, int mangaId)
        {
            var chapterList = _unitOfWork.mangaChapter.GetAll().Where(u => u.MangaId == mangaId).OrderBy(u => u.chapterNum).Select(u => u.chapterNum).ToList();
            var currentChapter = chapterList.IndexOf(chapterId);
            var currentDetails = _unitOfWork.mangaChapter.GetFirstOrDefault(u => (u.chapterNum == chapterId) && (u.MangaId == mangaId));
            

            int prevChapter = 0;
            int nextChapter = 0;

            if (currentChapter > 0)
            {
                prevChapter = chapterList[currentChapter - 1];
            }
            if (currentChapter < chapterList.Count - 1)
            {
                nextChapter = chapterList[currentChapter + 1];
            }

            
            var manga = _unitOfWork.mangaPage.GetFirstOrDefault(u => u.MangaId == mangaId);
            string chapterFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "manga", manga.MangaName, chapterId.ToString());

            // Get all image file paths
            List<string> chapterImgPath = new List<string>();
            foreach (var filePath in Directory.GetFiles(chapterFolder))
            {
                string fileName = Path.GetFileName(filePath);
                string relativePath = Path.Combine("/images/manga", manga.MangaName, chapterId.ToString(), fileName).Replace("\\", "/");
                chapterImgPath.Add(relativePath);
            }

            Console.WriteLine(currentDetails.MangaId);
            return View(new MangaNovelChapterView
            {

                mangaChapter = currentDetails,
                prevChapter = prevChapter,
                nextChapter = nextChapter,
                lastChapter = _unitOfWork.mangaChapter.GetAll().OrderBy(u => u.chapterNum).Select(u => u.chapterNum).Last(),
                firstChapter = _unitOfWork.mangaChapter.GetAll().OrderBy(u => u.chapterNum).Select(u => u.chapterNum).First(),
                mangaChapterImages = chapterImgPath
            });
        }
    }
}
