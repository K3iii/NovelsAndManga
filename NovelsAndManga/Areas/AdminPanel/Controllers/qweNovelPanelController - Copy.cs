using Microsoft.AspNetCore.Mvc;
using NovelsManga.DataAccess.Data;
using NovelsManga.DataAccess.Repository;
using NovelsManga.DataAccess.Repository.IRepository;
using NovelsManga.Models;

namespace NovelsAndManga.Areas.AdminPanel.Controllers
{
    
    public class qweNovelPanelController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public qweNovelPanelController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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


        public IActionResult Create(int? id)
        {
            var existingNovel = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsId == id);

            
            var associatedGenres = _unitOfWork.novelsGenre.GetAll().Where(g => g.NovelsId == id)
                                    .Select(g => g.GenreId)
                                    .ToList();

            var viewModel = new NovelsGenreView
            {
                NewNovel = existingNovel ?? new NovelsModels(),
                Genre = _unitOfWork.Genre.GetAll().ToList(),
                SelectedGenreIds = associatedGenres
            };

            return View(viewModel);


        }

        [HttpPost]
        public IActionResult Create(NovelsGenreView obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                //var selectedGenre = Request.Form["SelectedGenre"].ToString().Split(',');

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() +Path.GetExtension(file.FileName);
                    string novelImgPath = Path.Combine(wwwRootPath, @"images\novels\"+obj.NewNovel.NovelsName);
                    if (!Directory.Exists(novelImgPath))
                    {
                        Directory.CreateDirectory(novelImgPath);
                    }
                    else
                    {
                        Console.WriteLine("Directory exist");
                    }

                    if (!string.IsNullOrEmpty(obj.NewNovel.ImgUrl))
                    {
                        var oldImgPath = Path.Combine(wwwRootPath, obj.NewNovel.ImgUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }

                    using ( var fileStream = new FileStream(Path.Combine(novelImgPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    
                    obj.NewNovel.ImgUrl = @"images\novels\" + obj.NewNovel.NovelsName + @"\" + fileName;
                }
                else
                {
                    obj.NewNovel.ImgUrl = "";
                }

                if(obj.NewNovel.NovelsId != 0)
                {
                    var existingGenres = _unitOfWork.novelsGenre
                    .GetAll()
                    .Where(g => g.NovelsId == obj.NewNovel.NovelsId)
                    .ToList();

                    foreach (var genre in existingGenres)
                    {
                        _unitOfWork.novelsGenre.Remove(genre);
                    }

                    _unitOfWork.novelPage.Update(obj.NewNovel);
                }
                else
                {
                    _unitOfWork.novelPage.Add(obj.NewNovel);
                }

                
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
            //var existingNovel = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsId == id);

            //if (existingNovel == null)
            //{
            //    return NotFound();
            //}
            //var associatedGenres = _unitOfWork.novelsGenre.GetAll().Where(g => g.NovelsId == id)
            //                        .Select(g => g.GenreId)
            //                        .ToList();

            //var viewModel = new NovelsGenreView
            //{
            //    NewNovel = existingNovel,
            //    Genre = _unitOfWork.Genre.GetAll().ToList(),
            //    SelectedGenreIds = associatedGenres
            //};

            return View(/*viewModel*/);


        }

        [HttpPost]
        public IActionResult Edit(NovelsGenreView obj)
        {
            

            if (ModelState.IsValid)
            {
                
                obj.NewNovel.NovelsId = obj.NewNovel.NovelsId; // Make sure this is set correctly
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
