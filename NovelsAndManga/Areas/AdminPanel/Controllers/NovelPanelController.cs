using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NovelsManga.DataAccess.Data;
using NovelsManga.DataAccess.Repository;
using NovelsManga.DataAccess.Repository.IRepository;
using NovelsManga.Models;
using NovelsManga.Utility;

namespace NovelsAndManga.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class NovelPanelController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public NovelPanelController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
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


        public IActionResult UpsertNovel(int? id)
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
        public IActionResult UpsertNovel(NovelsGenreView obj, IFormFile? file)
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
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {

            if(id == null || id == 0)
            {
                return NotFound();
            }
            var novelList = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsId == id);
            if(novelList == null)
            {
                return NotFound();
            }

            return View(novelList);


        }

        [HttpPost]
        public IActionResult DeletePost(int? id, string isType)
        {
            Console.WriteLine(isType);
            if (isType == "chapter")
            {
                
                    NovelsChapter obj = _unitOfWork.novelChapter.GetFirstOrDefault(u => u.novelChapterId == id);
                    _unitOfWork.novelChapter.Remove(obj);
                    _unitOfWork.Save();
                    return Json(new { success = true, message = "test clicked" });

            }
            else
            {
                NovelsModels? obj = _unitOfWork.novelPage.GetFirstOrDefault(u => u.NovelsId == id);
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                Console.WriteLine(id);
                if (obj != null)
                {
                    var associatedGenre = _unitOfWork.novelsGenre.GetAll().Where(u => u.NovelsId == id);
                    foreach (var genre in associatedGenre)
                    {
                        _unitOfWork.novelsGenre.Remove(genre);
                    }
                    if (!string.IsNullOrEmpty(obj.ImgUrl))
                    {
                        var imgPath = Path.Combine(wwwRootPath, @"images\novels\" + obj.NovelsName);
                        Directory.Delete(imgPath, true);
                    }
                    _unitOfWork.novelPage.Remove(obj);
                    _unitOfWork.Save();
                    return Json(true);
                }
            }


            return Json(false);
        }
        
        public IActionResult ChapterList(int? id)
        {
            ViewData["NovelsId"] = id;
            var existingChapter = _unitOfWork.novelChapter.GetAll().Where(u => u.NovelsId == id).ToList();
            return View();
        }
        

        public IActionResult UpsertChapter(int? id, int novelid)
        {
            
            if (id == null || id == 0)
            {
                
                NovelsChapter existChapter = new NovelsChapter()
                {
                    NovelsId = novelid
                };
                return View(existChapter);
            }
            NovelsChapter novelsChapter = _unitOfWork.novelChapter.GetFirstOrDefault(u => u.novelChapterId == id);
            return View(novelsChapter);

        }

        [HttpPost]
        public IActionResult UpsertChapter(NovelsChapter obj)
        {
            Console.WriteLine(obj.novelChapterId);
            if (ModelState.IsValid)
            {
                if (obj == null)
                {
                    
                    _unitOfWork.novelChapter.Add(obj);
                }
                else
                {
                    _unitOfWork.novelChapter.Update(obj);
                }
                
                


                _unitOfWork.Save();
                return RedirectToAction("ChapterList", new { id = obj.NovelsId });
            }
           

            return View(obj);
        }

        [HttpGet]
        public IActionResult GetChapter(int? id)
        {
            var existingChapter = _unitOfWork.novelChapter.GetAll().Where(u => u.NovelsId == id).ToList();
            return Json(new { data = existingChapter });
        }

    }
}
