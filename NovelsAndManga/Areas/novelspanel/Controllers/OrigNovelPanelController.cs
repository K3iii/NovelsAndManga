using Microsoft.AspNetCore.Mvc;
using NovelsManga.DataAccess.Data;
using NovelsManga.DataAccess.Repository;
using NovelsManga.DataAccess.Repository.IRepository;
using NovelsManga.Models;

namespace NovelsAndManga.Areas.novelspanel.Controllers
{
    [Area("novelspanel")]
    public class OrigNovelPanelController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public OrigNovelPanelController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //List<NovelsModels> novelsPage = _unitOfWork.novelPage.GetAll().ToList();

            var novelList = new NovelsMangaGenreView
            {
                NovelsPage = _unitOfWork.novelPage.GetAll().ToList(),
                Genre = _unitOfWork.Genre.GetAll().ToList(),
                novelsGenres = _unitOfWork.novelsGenre.GetAll().ToList()
            };

            return View(novelList);
        }


        public IActionResult Create()
        {
            var viewModel = new NovelsGenreView
            {
                NewNovel = new NovelsModels(),
                Genre = _unitOfWork.Genre.GetAll().ToList() 
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(NovelsGenreView obj)
        {
            if (ModelState.IsValid)
            {

                //var selectedGenre = Request.Form["SelectedGenre"].ToString().Split(',');

                

                _unitOfWork.novelPage.Add(obj.NewNovel);
                _unitOfWork.Save();

                if (obj.SelectedGenreIds != null)
                {
                    foreach (var genreId in obj.SelectedGenreIds)
                    {
                        var novelsGenre = new NovelsGenre
                        {
                            NovelsId = obj.NewNovel.NovelsId, // Assign the newly created NovelsId
                            GenreId = genreId
                        };
                        _unitOfWork.novelsGenre.Add(novelsGenre);
                    }
                    _unitOfWork.Save();
                }


                return RedirectToAction("Index");

            }

            


            Console.WriteLine("ModelState is invalid.");
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                foreach (var error in state.Errors)
                {
                    Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
                }
            }

            // Return the view with the invalid object
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            //if (id == null || id == 0)
            //{
            //    return NotFound();
            //}
            //NovelsModels? novelList = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsId == id);
            //if (novelList == null)
            //{
            //    return NotFound();
            //}

            var existingNovel = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsId == id);

            if (existingNovel == null)
            {
                return NotFound();
            }

            // Fetch genres associated with the existing novel
            var associatedGenres = _unitOfWork.novelsGenre.GetAll().Where(g => g.NovelsId == id)
                                    .Select(g => g.GenreId)
                                    .ToList();

            // Build the view model for editing
            var viewModel = new NovelsGenreView
            {
                NewNovel = existingNovel,
                Genre = _unitOfWork.Genre.GetAll().ToList(),
                SelectedGenreIds = associatedGenres
            };

            return View(viewModel);


        }

        [HttpPost]
        public IActionResult Edit(NovelsGenreView obj)
        {
            //if (ModelState.IsValid)
            //{
            //    _unitOfWork.novelPage.Update(obj);
            //    _unitOfWork.Save();
            //    return RedirectToAction("Index");
            //}
            //return View();

            if (ModelState.IsValid)
            {
                // Update the existing NovelsModels object
                obj.NewNovel.NovelsId = obj.NewNovel.NovelsId; // Make sure this is set correctly

                // Update the existing NovelsModels object
                _unitOfWork.novelPage.Update(obj.NewNovel);
                _unitOfWork.Save();

                // Fetch existing NovelsGenre entries using LINQ and remove them
                var existingGenres = _unitOfWork.novelsGenre
                    .GetAll()
                    .Where(g => g.NovelsId == obj.NewNovel.NovelsId)
                    .ToList();

                foreach (var genre in existingGenres)
                {
                    _unitOfWork.novelsGenre.Remove(genre);
                }

                // Add the updated selected genres
                if (obj.SelectedGenreIds != null)
                {
                    foreach (var genreId in obj.SelectedGenreIds)
                    {
                        var novelsGenre = new NovelsGenre
                        {
                            NovelsId = obj.NewNovel.NovelsId,
                            GenreId = genreId
                        };
                        _unitOfWork.novelsGenre.Add(novelsGenre);
                    }
                }

                _unitOfWork.Save();

                return RedirectToAction("Index");
            }

            // If model state is not valid, redisplay the form with errors
            
            return View(obj);
        }


        public IActionResult Delete(int? id)
        {
            Console.WriteLine(id);
            if (id == null || id == 0)
            {
                return NotFound();
            }
            NovelsModels? novelList = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsId == id);
            if (novelList == null)
            {
                return NotFound();
            }

            return View(novelList);


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
        //[HttpGet("{novelName}")]
        //public IActionResult Read(string novelName)
        //{
        //    var novelPage = new NovelsMangaGenreView
        //    {
        //        NovelsPage = _unitOfWork.novelPage.GetAll().ToList()
        //};

        //    return View(novelPage);
        //}

        //[HttpGet("{chapterId}")]
        //public IActionResult Chapter(string novelName, int chapterId)
        //{
        //    var novelPage = new NovelsMangaGenreView
        //    {
        //        NovelsPage = _unitOfWork.novelPage.GetAll().ToList()
        //    };

        //    return View(novelPage);
        //}
    }
}
