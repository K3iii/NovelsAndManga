using Microsoft.AspNetCore.Mvc;
using NovelsManga.DataAccess.Repository.IRepository;
using NovelsManga.Models;
using System.Drawing;

namespace NovelsAndManga.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class MangaPanelController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MangaPanelController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var mangaList = new NovelsMangaGenreView
            {
                MangaPage = _unitOfWork.mangaPage.GetAll().ToList(),
                Genre = _unitOfWork.Genre.GetAll().ToList(),
                mangaGenres = _unitOfWork.mangaGenre.GetAll().ToList()
            };

            return View(mangaList);
            //return View();
        }

        public IActionResult UpsertManga(int? id)
        {
            var existingManga = _unitOfWork.mangaPage.GetFirstOrDefault(u => u.MangaId == id);


            var associatedGenres = _unitOfWork.mangaGenre.GetAll().Where(g => g.MangaId == id)
                                    .Select(g => g.GenreId)
                                    .ToList();

            var viewModel = new MangaGenreView
            {
                NewManga = existingManga ?? new MangaModels(),
                Genre = _unitOfWork.Genre.GetAll().ToList(),
                SelectedMangaGenreIds = associatedGenres
            };

            return View(viewModel);


        }

        [HttpPost]
        public IActionResult UpsertManga(MangaGenreView obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {

                //var selectedGenre = Request.Form["SelectedGenre"].ToString().Split(',');

                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string MangaImgPath = Path.Combine(wwwRootPath, @"images\manga\" + obj.NewManga.MangaName);
                    if (!Directory.Exists(MangaImgPath))
                    {
                        Directory.CreateDirectory(MangaImgPath);
                    }
                    else
                    {
                        Console.WriteLine("Directory exist");
                    }

                    if (!string.IsNullOrEmpty(obj.NewManga.ImgUrl))
                    {
                        var oldImgPath = Path.Combine(wwwRootPath, obj.NewManga.ImgUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(MangaImgPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }


                    obj.NewManga.ImgUrl = @"images\manga\" + obj.NewManga.MangaName + @"\" + fileName;
                }


                if (obj.NewManga.MangaId != 0)
                {
                    var existingGenres = _unitOfWork.mangaGenre
                    .GetAll()
                    .Where(g => g.MangaId == obj.NewManga.MangaId)
                    .ToList();

                    foreach (var genre in existingGenres)
                    {
                        _unitOfWork.mangaGenre.Remove(genre);
                    }

                    _unitOfWork.mangaPage.Update(obj.NewManga);
                }
                else
                {
                    _unitOfWork.mangaPage.Add(obj.NewManga);
                }


                _unitOfWork.Save();

                if (obj.SelectedMangaGenreIds != null)
                {
                    foreach (var genreId in obj.SelectedMangaGenreIds)
                    {
                        var mangaGenres = new MangaGenre
                        {
                            MangaId = obj.NewManga.MangaId, // Assign the newly created NovelsId
                            GenreId = genreId
                        };
                        _unitOfWork.mangaGenre.Add(mangaGenres);
                    }
                    _unitOfWork.Save();
                }


                return RedirectToAction("Index");

            }

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"Error in {key}: {error.ErrorMessage}");
                    }
                }
            }
            return View(obj);
        }


        [HttpPost]
        public IActionResult DeletePost(int? id, string isType)
        {
            Console.WriteLine(isType);
            if (isType == "chapter")
            {

                MangaChapter obj = _unitOfWork.mangaChapter.GetFirstOrDefault(u => u.mangaChapterId == id);
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string mangaName = _unitOfWork.mangaPage.GetFirstOrDefault(u => u.MangaId == obj.MangaId).MangaName;
                Console.WriteLine(mangaName);
                string MangaImgPath = Path.Combine(wwwRootPath, @"images\manga\" + mangaName + @"\" + obj.chapterNum);
                if (Directory.Exists(MangaImgPath))
                {
                    Directory.Delete(MangaImgPath, true);
                }
                

                _unitOfWork.mangaChapter.Remove(obj);
                _unitOfWork.Save();
                return Json(new { success = true, message = "test clicked" });

            }
            else
            {
                MangaModels? obj = _unitOfWork.mangaPage.GetFirstOrDefault(u => u.MangaId == id);
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                Console.WriteLine(id);
                if (obj != null)
                {
                    var associatedGenre = _unitOfWork.mangaGenre.GetAll().Where(u => u.MangaId == id);
                    foreach (var genre in associatedGenre)
                    {
                        _unitOfWork.mangaGenre.Remove(genre);
                    }
                    if (!string.IsNullOrEmpty(obj.ImgUrl))
                    {
                        var imgPath = Path.Combine(wwwRootPath, @"images\manga\" + obj.MangaName);
                        Directory.Delete(imgPath, true);
                    }
                    _unitOfWork.mangaPage.Remove(obj);
                    _unitOfWork.Save();
                    return Json(true);
                }
            }


            return Json(false);
        }



        public IActionResult UpsertChapter(int? id, int mangaid)
        {

            if (id == null || id == 0)
            {

                MangaChapter existChapter = new MangaChapter()
                {
                    MangaId = mangaid
                };
                return View(existChapter);
            }
            MangaChapter mangaChapter = _unitOfWork.mangaChapter.GetFirstOrDefault(u => u.mangaChapterId == id);
            return View(mangaChapter);

        }

        [HttpPost]
        public IActionResult UpsertChapter(MangaChapter obj, IFormFileCollection? file)
        {
            Console.WriteLine(obj.mangaChapterId);
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string mangaName = _unitOfWork.mangaPage.GetFirstOrDefault(u => u.MangaId == obj.MangaId).MangaName;
                    string MangaImgPath = Path.Combine(wwwRootPath, @"images\manga\" + mangaName +@"\"+ obj.chapterNum);
                    if (!Directory.Exists(MangaImgPath))
                    {
                        Directory.CreateDirectory(MangaImgPath);
                    }
                    else
                    {
                        Directory.Delete(MangaImgPath, true);
                        Directory.CreateDirectory(MangaImgPath);
                    }

                    foreach (var chapimages in file)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(chapimages.FileName);
                        using (var fileStream = new FileStream(Path.Combine(MangaImgPath, fileName), FileMode.Create))
                        {
                            chapimages.CopyTo(fileStream);
                        }
                    }
                    

                    //if (!string.IsNullOrEmpty(obj.NewManga.ImgUrl))
                    //{
                    //    var oldImgPath = Path.Combine(wwwRootPath, obj.NewManga.ImgUrl.TrimStart('\\'));
                    //    if (System.IO.File.Exists(oldImgPath))
                    //    {
                    //        System.IO.File.Delete(oldImgPath);
                    //    }
                    //}

                    


                    obj.chapterFolder = @"images\manga\" + mangaName + @"\" + obj.chapterNum;
                }

                if(obj.mangaChapterId == 0 )
                {
                    _unitOfWork.mangaChapter.Add(obj);
                }
                else
                {
                    _unitOfWork.mangaChapter.Update(obj);
                }




                _unitOfWork.Save();
                return RedirectToAction("ChapterList", new { id = obj.MangaId });
            }


            return View(obj);
        }

        public IActionResult ChapterList(int? id)
        {
            ViewData["MangaId"] = id;
            var existingChapter = _unitOfWork.mangaChapter.GetAll().Where(u => u.MangaId == id).ToList();
            return View();
        }

        [HttpGet]
        public IActionResult GetChapter(int? id)
        {
            var existingChapter = _unitOfWork.mangaChapter.GetAll().Where(u => u.MangaId == id).ToList();
            return Json(new { data = existingChapter });
        }
    }
}
