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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
