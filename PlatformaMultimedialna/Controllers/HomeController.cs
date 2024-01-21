using Microsoft.AspNetCore.Mvc;
using PlatformaMultimedialna.Models;
using System.Diagnostics;

namespace PlatformaMultimedialna.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var photos = GetPhotosFromDatabase();
            return View(photos);
        }
        private List<FotoModel> GetPhotosFromDatabase()
        {
            

            return new List<FotoModel>
            {
            new FotoModel { Id = 1, ImagePath = "/images/1,1.jpg" },
            new FotoModel { Id = 2, ImagePath = "/images/2,2.jpg" },
            new FotoModel { Id = 3, ImagePath = "/images/3,3.jpg" },
            new FotoModel { Id = 4, ImagePath = "/images/4,4.jpg" },
            new FotoModel { Id = 5, ImagePath = "/images/5,5.jpg "},
            new FotoModel { Id = 6, ImagePath = "/images/6,6.jpg "},
            };
            
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult omnie()
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