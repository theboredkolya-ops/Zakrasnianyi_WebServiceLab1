using Lab2_Zakrasnianyi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebServiceLab1.Models;

namespace WebServiceLab1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmailSender _emailSender;
        public HomeController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Email()
        {
            return View(new EmailViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Email(EmailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _emailSender.SendEmailAsync(
            model.Email,
            "Лабораторна робота №2",
            model.Message);

            ViewBag.Success = "Email успішно надіслано.";

            return View(new EmailViewModel());
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Оберіть файл для завантаження.";
                return View();
            }

            var filesPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "Files");

            Directory.CreateDirectory(filesPath);

            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(filesPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            ViewBag.Message = "Файл успішно завантажено!";
            return View();
        }

        [HttpGet]
        public IActionResult Files()
        {
            var filesPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "Files");

            Directory.CreateDirectory(filesPath);

            var files = Directory.GetFiles(filesPath)
            .Select(Path.GetFileName)
            .ToList();

            return View(files);
        }

        [HttpGet]
        public IActionResult ViewFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return NotFound();
            }

            var safeFileName = Path.GetFileName(fileName);

            var filePath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "Files",
            safeFileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var contentType = GetContentType(safeFileName);

            return PhysicalFile(filePath, contentType);
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                _ => "application/octet-stream"
            };
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
