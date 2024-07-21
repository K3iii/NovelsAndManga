using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovelsManga.DataAccess.Data;
using NovelsManga.DataAccess.Repository;
using NovelsManga.DataAccess.Repository.IRepository;
using NovelsManga.Models;
using NovelsManga.Utility;

namespace NovelsAndManga.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class GenrePanelController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public GenrePanelController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Genre> genreList = _unitOfWork.Genre.GetAll().ToList();

            if (genreList == null || genreList.Count == 0)
            {
                // Handle case where no genres are found
                TempData["msg"] = "No genres found in the database.";
                return View();
            }

            return View(genreList);
        }


        public IActionResult CreateGenre()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CreateGenre(Genre obj)
        {


            if (ModelState.IsValid)
            {
                _unitOfWork.Genre.Add(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Genre? genre = _unitOfWork.Genre.GetFirstOrDefault(u => u.GenreId == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);


        }

        [HttpPost]
        public IActionResult Edit(Genre obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Genre.Update(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }


        public IActionResult Delete(int? id)
        {
            Console.WriteLine(id);
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Genre? genre = _unitOfWork.Genre.GetFirstOrDefault(u => u.GenreId == id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);


        }

        [HttpPost]
        public JsonResult DeletePost(int? id)
        {
            Genre? obj = _unitOfWork.Genre.GetFirstOrDefault(u => u.GenreId == id);
            Console.WriteLine(id);
            if (obj != null)
            {
                _unitOfWork.Genre.Remove(obj);
                _unitOfWork.Save();
                return Json(true);
            }

            //var qwe = id;

            return Json(false);
        }
    }
}
