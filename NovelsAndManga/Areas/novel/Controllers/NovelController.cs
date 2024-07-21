using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NovelsManga.DataAccess.Repository.IRepository;
using NovelsManga.Models;

namespace NovelsAndManga.Areas.novel.Controllers
{
    [Area("novel")]
    [Route("novel")]
    public class NovelController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public NovelController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{novelId}")]
        public IActionResult Index(int novelId)
        {
            //List<NovelsModels> novelsModelsList = _unitOfWork.novelPage.Get(u => u.NovelsName == novelName).ToList();
            var novel = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsId == novelId);
            //var novels = _unitOfWork.novelPage.GetAll().Where(u => u.NovelsName == novelName).ToList();
            //NovelsModels novelsMangaGenreView = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsName == novelName);
            //var novelInfo = _unitOfWork.novelPage.GetAll().Where(u => u.NovelsName == novelName);
            var genre = _unitOfWork.novelsGenre.GetAll().Where(u=>u.NovelsId == novelId).Select(u=>u.GenreId).ToList();
            var chapterList = _unitOfWork.novelChapter.GetAll().Where(u => u.NovelsId == novelId).OrderBy(u => u.chapterNum).ToList();

            var novelpage = new NovelsMangaGenreView
            {
                NovelsPage = new List<NovelsModels> { novel },
                SelectedGenreIds = genre,
                Genre = _unitOfWork.Genre.GetAll().ToList(),
                Chapters = chapterList
            };

            return View(novelpage);
        }

        [HttpGet("{novelId}/{chapterId}")]
        public IActionResult Chapter(string novelName, int chapterId, int novelId)
        {
            var chapterList = _unitOfWork.novelChapter.GetAll().Where(u => u.NovelsId == novelId).OrderBy(u => u.chapterNum).Select(u => u.chapterNum).ToList();
            var currentChapter = chapterList.IndexOf(chapterId);
            var currentDetails = _unitOfWork.novelChapter.GetFirstOrDefault(u => (u.chapterNum == chapterId) && (u.NovelsId == novelId));
            Console.WriteLine(novelId);
            foreach(var chapter in chapterList)
            {
                Console.WriteLine(chapter);
            }

            int prevChapter = 0;
            int nextChapter = 0;

            if (currentChapter > 0 )
            {
                prevChapter = chapterList[currentChapter - 1];
            }
            if(currentChapter < chapterList.Count - 1)
            {
                nextChapter = chapterList[currentChapter + 1];
            }

            Console.WriteLine(currentDetails.NovelsId);
            return View(new MangaNovelChapterView
            {
                
                novelChapter = currentDetails,
                prevChapter = prevChapter,
                nextChapter = nextChapter,
                lastChapter = _unitOfWork.novelChapter.GetAll().OrderBy(u => u.chapterNum).Select(u => u.chapterNum).Last(),
                firstChapter = _unitOfWork.novelChapter.GetAll().OrderBy(u => u.chapterNum).Select(u => u.chapterNum).First()
            });
        }

        





        public IActionResult Read()
        {
            return View();
        }
    }
}
