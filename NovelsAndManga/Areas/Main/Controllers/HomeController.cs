using Microsoft.AspNetCore.Mvc;
using NovelsManga.DataAccess.Repository;
using NovelsManga.DataAccess.Repository.IRepository;
using NovelsManga.Models;
using System.Diagnostics;
using System.Net.Http;

namespace NovelsAndManga.Areas.Main.Controllers
{
    [Area("Main")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            List<NovelsModels> novelList = _unitOfWork.novelPage.GetAll().ToList();
            List<MangaModels> mangaList = _unitOfWork.mangaPage.GetAll().ToList();
            List<NovelsChapter> novelChapters = _unitOfWork.novelChapter.GetAll().ToList();
            List<MangaChapter> mangaChapters = _unitOfWork.mangaChapter.GetAll().ToList();

            var manganovelList = new NovelsMangaGenreView {
                NovelsPage = novelList,
                MangaPage = mangaList,
                Chapters = novelChapters,
                MangaChapters = mangaChapters
            };
                

            return View(manganovelList);
        }
        [Route("Main/Novel/")]
        public IActionResult Novel()
        {
            var getNovels = _unitOfWork.novelPage.GetAll();
            var novelList = _unitOfWork.novelPage.GetAll().ToList();
            var chapterList = _unitOfWork.novelChapter.GetAll().ToList();

            var novels = new NovelsMangaGenreView
            {
                NovelsPage = novelList,
                Chapters = chapterList

            };
            return View(novels);
        }

        [Route("Main/Manga/")]
        public IActionResult Manga()
        {
            var getNovels = _unitOfWork.mangaPage.GetAll();
            var mangaList = _unitOfWork.mangaPage.GetAll().ToList();
            var chapterList = _unitOfWork.mangaChapter.GetAll().ToList();

            var novels = new NovelsMangaGenreView
            {
                MangaPage = mangaList,
                MangaChapters = chapterList

            };
            return View(novels);
        }


        public IActionResult Privacy()
            {
                return View();
            }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }

}
