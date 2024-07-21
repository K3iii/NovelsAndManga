using Microsoft.AspNetCore.Mvc;
using NovelsManga.DataAccess.Repository;
using NovelsManga.Models;

namespace NovelsAndManga.Areas.novelspanel.Controllers
{
    [Area("novelspanel")]
    public class NovelwPanelController : Controller
    {
        private readonly UnitOfWork _unitOfWork;

        public NovelwPanelController(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<NovelsModels> novelsList = _unitOfWork.novelPage.GetAll().ToList();
            return View(novelsList);
        }
    }
}
