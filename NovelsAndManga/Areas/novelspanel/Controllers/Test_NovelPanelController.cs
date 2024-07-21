using Microsoft.AspNetCore.Mvc;
using NovelsManga.DataAccess.Data;
using NovelsManga.DataAccess.Repository;
using NovelsManga.DataAccess.Repository.IRepository;
using NovelsManga.Models;

namespace NovelsAndManga.Areas.novelspanel.Controllers
{
    [Area("novelspanel")]
    public class Test_NovelPanelController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public Test_NovelPanelController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<NovelsModels> novelList = _unitOfWork.novelPage.GetAll().ToList();

            if (novelList == null || novelList.Count == 0)
            {
                // Handle case where no genres are found
                TempData["msg"] = "No genres found in the database.";
                return View();
            }

            return View(novelList);
        }


        public IActionResult CreateGenre()
        {

            return View();
        }

        [HttpPost]
        public IActionResult CreateGenre(NovelsModels obj)
        {
            //if (ModelState.IsValid)
            //{
            //    _db.Genre.Add(obj);
            //    _db.SaveChanges();

            //    return RedirectToAction("Index");

            //}
            ////_db.Genre.Add(obj);
            ////_db.SaveChanges();

            //return View(obj);



            if (ModelState.IsValid)
            {
                _unitOfWork.novelPage.Add(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            // Log or debug model errors
            foreach (var modelStateEntry in ModelState.Values)
            {
                foreach (var error in modelStateEntry.Errors)
                {
                    // Log or debug error here
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            // Return the view with the invalid object
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                Console.WriteLine("Null");
                return NotFound();
            }
            NovelsModels? novelPage = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsId == id);
            if (novelPage == null)
            {
                Console.WriteLine("Null");
                return NotFound();
            }

            return View(novelPage);


        }

        [HttpPost]
        public IActionResult Edit(NovelsModels obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.novelPage.Update(obj);
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
            NovelsModels? novelPage = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsId == id);
            if (novelPage == null)
            {
                return NotFound();
            }

            return View(novelPage);


        }

        [HttpPost]
        public JsonResult DeletePost(int? id)
        {
            NovelsModels? obj = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsId == id);
            Console.WriteLine(id);
            if (obj != null)
            {
                _unitOfWork.novelPage.Remove(obj);
                _unitOfWork.Save();
                return Json(true);
            }

            //var qwe = id;

            return Json(false);
        }
    }
}
