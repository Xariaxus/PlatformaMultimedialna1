using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlatformaMultimedialna.Data;
using PlatformaMultimedialna.Models;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using static NuGet.Packaging.PackagingConstants;
using System.Linq.Expressions;
using System.Text;



namespace PlatformaMultimedialna.Controllers
{
    [Authorize]
    public class MediaController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MediaController(UserManager<IdentityUser> userManager, ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _hostingEnvironment = hostingEnvironment;

        }
        public IActionResult Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            var pliki = PobierzListePlikow()?.Select(nazwaPliku => new MediaModel { NazwaPliku = nazwaPliku });
            return View(pliki ?? Enumerable.Empty<MediaModel>());

        }

        [HttpPost]
        public IActionResult PrzeslijPlik(IFormFile plik)
        {

            if (plik != null && plik.Length > 0)
            {
                try
                {

                    var currentUserId = _userManager.GetUserId(User);
                    string rozszerzenie = Path.GetExtension(plik.FileName).ToLower();
                    if (rozszerzenie == ".jpg" || rozszerzenie == ".jpeg" || rozszerzenie == ".mp4" || rozszerzenie == ".mp3" || rozszerzenie == ".png")
                    {

                        string unikalnaNazwaPliku = plik.FileName;
                        string folderUzytkownika = Path.Combine(Directory.GetCurrentDirectory(), "Pliki", currentUserId);
                        if (!Directory.Exists(folderUzytkownika))
                        {
                            Directory.CreateDirectory(folderUzytkownika);
                        }
                        string sciezka = Path.Combine(Directory.GetCurrentDirectory(), "Pliki", folderUzytkownika, unikalnaNazwaPliku);
                        using (var stream = new FileStream(sciezka, FileMode.Create))
                        {
                            plik.CopyTo(stream);
                        }

                        // Konwersja obrazu do określonej szerokości
                        using (var image = Image.Load(sciezka))
                        {
                            image.Mutate(x => x.Resize(new ResizeOptions
                            {
                                Size = new Size(250, 250),
                                Mode = ResizeMode.Stretch
                            }));
                            image.Save(sciezka); // Nadpisuje oryginalny plik skonwertowanym obrazem
                        }

                        // Dodaj nowy rekord do bazy danych
                        var media = new MediaModel
                        {
                            UserId = currentUserId,
                            FileName = unikalnaNazwaPliku,
                            FilePath = sciezka,
                            NazwaPliku = rozszerzenie
                            // Dodaj inne pola w zależności od modelu danych
                        };
                        try
                        {
                            _context.Media.Add(media);
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Nie działa " + ex.Message);
                            Console.WriteLine(ex.ToString());
                        }
                        var mediaData = _context.Media.Select(media => new MediaDto
                        {
                            currentUserId = media.UserId,
                            FileName = media.FileName,
                            NazwaPliku = media.NazwaPliku,
                        }).ToList();
                        var jsonData = JsonConvert.SerializeObject(mediaData, Formatting.None);

                        var formattedData = string.Join(Environment.NewLine, jsonData.Split('}'));

                        var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "dane_z_bazy.txt");

                        System.IO.File.AppendAllText(filePath, formattedData + Environment.NewLine, Encoding.UTF8);

                        ViewBag.Message = $"Dane zapisano do pliku {filePath}.";

                        ViewBag.Message = "Plik przesłany poprawnie!";
                    }
                    else
                    {
                        ViewBag.Message = "Dozwolone są tylko pliki JPG, PNG, MP3 i MP4.";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Wystąpił błąd: " + ex.Message;
                }
            }
            else
            {
                ViewBag.Message = "Proszę wybrać plik do przesłania.";
            }

            // Usuń stare pliki, które są w folderze "Pliki", ale nie ma ich w nowej liście przesłanych plików
            var nowePliki = PobierzListePlikow().ToList();
            foreach (var nazwaPliku in Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Pliki")).Select(Path.GetFileName))
            {
                if (!nowePliki.Contains(nazwaPliku))
                {
                    UsunPlik(nazwaPliku);
                }
            }

            // Przekieruj na akcję Index z odświeżeniem strony po dodaniu pliku
            return RedirectToAction("Index");
        }



        public IActionResult PobierzPlik(string nazwaPliku)
        {
            var currentUserId = _userManager.GetUserId(User);
            string sciezka = Path.Combine(Directory.GetCurrentDirectory(), "Pliki", currentUserId, nazwaPliku);
            if (System.IO.File.Exists(sciezka))
            {

                // Jeśli plik istnieje, zwróć go jako plik do pobrania
                return File(System.IO.File.ReadAllBytes(sciezka), "application/octet-stream", nazwaPliku);

            }
            else
            {
                // Jeśli plik nie istnieje, możesz obsłużyć to w dowolny sposób, na przykład zwracając błąd 404
                return NotFound();
            }
        }

        private string?[] PobierzListePlikow()
        {
            var currentUserId = _userManager.GetUserId(User);
            string folderUzytkownika = Path.Combine(Directory.GetCurrentDirectory(), "Pliki", currentUserId);

            if (Directory.Exists(folderUzytkownika))
            {
                return Directory.GetFiles(folderUzytkownika).Select(Path.GetFileName).ToArray();
            }
            else
            {
                return null;
            }
        }



        [HttpPost]
        public IActionResult UsunPlik(string nazwaPliku)
        {
            var currentUserId = _userManager.GetUserId(User);
            try
            {
                // Ścieżka do pliku
                string sciezka = Path.Combine(Directory.GetCurrentDirectory(), "Pliki", currentUserId, nazwaPliku);

                // Usuń plik, jeśli istnieje
                if (System.IO.File.Exists(sciezka))
                {
                    System.IO.File.Delete(sciezka);
                    ViewBag.Message = "Plik usunięty poprawnie!";
                }
                else
                {
                    ViewBag.Message = "Plik nie istnieje.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Wystąpił błąd podczas usuwania pliku: " + ex.Message;
            }

            // Przekieruj z powrotem do strony z listą plików
            return RedirectToAction("Index");
        }


    }


}


